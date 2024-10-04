using KS.Reactor.Server;
using KS.Reactor;

public class ServerBullet : ksServerEntityScript
{
    // The entity that shot the bullet.
    public ksIServerEntity Owner;

    // The bullet destroys itself after this many seconds.
    private float m_lifetime = 1f;

    // Called when the script is attached.
    public override void Initialize()
    {
        Room.OnUpdate[0] += Update;

        // Register an overlap event handler.
        Entity.OnOverlapStart += OnOverlap;
    }

    // Called when the script is detached.
    public override void Detached()
    {
        Room.OnUpdate[0] -= Update;
        Entity.OnOverlapStart -= OnOverlap;
    }

    private void Update()
    {
        // Destroy the entity when m_lifetime reaches zero.
        m_lifetime -= Time.Delta;
        if (m_lifetime <= 0)
        {
            Entity.Destroy();
        }
    }

    private void OnOverlap(ksOverlap overlap)
    {
        // Ignore overlaps with the owner that shot the bullet.
        if (overlap.Entity1 != Owner)
        {
            // Apply an impulse to the entity we hit.
            ksRigidBody rigidBody = Scripts.Get<ksRigidBody>();
            ksRigidBody otherBody = overlap.Entity1.Scripts.Get<ksRigidBody>();
            if (rigidBody != null && otherBody != null)
            {
                otherBody.AddForce(rigidBody.Velocity * .25f, ksForceMode.IMPULSE);
            }

            // Destroy the bullet.
            Entity.Destroy();
        }
    }
}