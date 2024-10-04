using KS.Reactor.Server;
using KS.Reactor;

public class ServerAvatar : ksServerEntityScript
{
    // Called when the script is attached.
    public override void Initialize()
    {

    }

    // Called when the script is detached.
    public override void Detached()
    {
        // Spawn a new avatar for the player when the avatar is destroyed (the player dies).
        // To prevent spawning an avatar when the player disconnects, we check Entity.Owner.Connected.
        if (Entity.Owner != null && Entity.Owner.Connected)
        {
            Room.Scripts.Get<ServerRoom>().SpawnAvatar(Entity.Owner);
        }
    }
}