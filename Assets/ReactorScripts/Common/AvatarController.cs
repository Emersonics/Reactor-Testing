using System;
using KS.Reactor;

// IDs for axes inputs
public class Axes
{
    public const uint X = 0;
    public const uint Y = 1;
    public const uint ROTATION = 2;
}

// IDs for button inputs
public class Buttons
{
    public const uint JUMP = 0;
    public const uint SHOOT = 1;
}

public class AvatarController : ksPlayerController
{
    [ksEditable] private float m_speed = 5f;
    [ksEditable] private float m_turnSpeed = 360f;
    [ksEditable] private float m_jumpSpeed = 6f;
    // How long in seconds before we can shoot again
    [ksEditable] private float m_shootInterval = 1f / 3f;

    // We use a delegate to handle shooting. The delegate is bound on the server and does nothing on the client.
    public Action OnShoot;

    private float m_shootTimer = 0f;
    private ksSweepParams m_sweepParams;

    // Unique non-zero identifier for this player controller class.
    public override uint Type
    {
        get { return 1; }
    }

    // Register all buttons and axes you will be using here.
    public override void RegisterInputs(ksInputRegistrar registrar)
    {
        registrar.RegisterAxes(Axes.X, Axes.Y, Axes.ROTATION);
        registrar.RegisterButtons(Buttons.JUMP, Buttons.SHOOT);
    }

    // Called after properties are initialized.
    public override void Initialize()
    {
        // Create sweep parameters for ground detection. CreateSimulationQueryParams preconfigures the params to only detect
        // entities the avatar entity can collide with.
        m_sweepParams = Physics.CreateSimulationQueryParams(Entity);
        m_sweepParams.Direction = ksVector3.Down;
        m_sweepParams.Distance = .1f;
    }

    // Called during the update cycle.
    public override void Update()
    {
        // Convert input to rotation in degrees between -180 and 180.
        float targetRotation = Input.GetAxis(Axes.ROTATION) * 180;

        // Convert rotation to a direction vector.
        ksVector2 direction = ksVector2.FromDegrees(targetRotation);

        // Add the direction vector to the entity's position to get the target.
        ksVector3 target = Transform.Position + new ksVector3(direction.X, 0f, direction.Y);

        // Rotate the entity towards the target.
        Transform.RotateTowards(target, m_turnSpeed * Time.Delta);

        // Convert input to horizontal velocity
        ksVector3 velocity = new ksVector3(Input.GetAxis(Axes.X), 0f, Input.GetAxis(Axes.Y)).Normalized() * m_speed;

        // Preserve vertical velocity from the rigid body.
        velocity.Y = RigidBody.Velocity.Y;

        // If jump is pressed, do a sweep test to see if we can jump (if we are touching the ground).
        if (Input.IsPressed(Buttons.JUMP))
        {
            // Sweeps don't always detect hits if the entity is touching an object at the start of the sweep, so we start the sweep .05
            // above the entity to ensure we aren't touching the ground at the beginning of the sweep.
            m_sweepParams.Origin = Transform.Position + ksVector3.Up * .05f;
            if (Physics.SweepAny(m_sweepParams))
            {
                // Set vertical jump velocity
                velocity.Y = m_jumpSpeed;
            }
        }

        // Set velocity
        RigidBody.Velocity = velocity;

        // Check if we can shoot
        if (m_shootTimer > 0f)
        {
            // Decrement shoot timer
            m_shootTimer -= Time.Delta;
        }
        else if (OnShoot != null && Input.IsDown(Buttons.SHOOT))
        {
            // Set the shoot timer and call the shoot delegate.
            m_shootTimer = m_shootInterval;
            OnShoot();
        }
    }
}