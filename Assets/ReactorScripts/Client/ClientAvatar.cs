using UnityEngine;
using KS.Reactor.Client.Unity;
using KS.Reactor.Client;
using KS.Reactor;

public class ClientAvatar : ksEntityScript
{
    // Called after properties are initialized.
    public override void Initialize()
    {
        // The entity will have a player controller if it is controlled by the local player.
        if (Entity.PlayerController != null)
        {
            // Set the camera to follow the avatar.
            FollowCamera.Target = gameObject;
        }
    }

    // Called when the script is detached.
    public override void Detached()
    {

    }

    private void Update()
    {
        if (Entity.PlayerController != null)
        {
            // Calculate the mouse position relative to the center of the screen.
            ksVector2 mouse = new ksVector2();
            mouse.X = -(Input.mousePosition.x - Screen.width / 2f);
            mouse.Y = -(Input.mousePosition.y - Screen.height / 2f);
            // Convert the mouse position to degrees, divide by 180 and subtract 1 to get a value between -1 and 1.
            float value = mouse.ToDegrees() / 180 - 1;
            // Set the rotation input value. Axis values must be between -1 and 1.
            ksReactor.InputManager.SetAxis(Axes.ROTATION, value);
        }
    }
}