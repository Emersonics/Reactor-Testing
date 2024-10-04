using KS.Reactor;
using KS.Reactor.Server;

public class ServerRoom : ksServerRoomScript
{
    // The player controller asset used to control avatars.
    [ksEditable]
    public ksPlayerController Controller;

    private ksRandom m_random = new ksRandom();

    // Initialize the script. Called once when the script is loaded.
    public override void Initialize()
    {
        // Register a event handler that will be called when a new player joins the server.
        Room.OnPlayerJoin += OnPlayerJoin;

        // Register a event handler that will be called when a player leaves the server.
        Room.OnPlayerLeave += OnPlayerLeave;

        // Register an update event to be called at every frame at order 0.
        Room.OnUpdate[0] += Update;
    }

    // Cleanup the script. Called once when the script is unloaded.
    public override void Detached()
    {
        Room.OnPlayerJoin -= OnPlayerJoin;
        Room.OnPlayerLeave -= OnPlayerLeave;
        Room.OnUpdate[0] -= Update;
    }

    public const float BULLET_SPEED = 16f;

    // Spawn an avatar for a player.
    public void SpawnAvatar(ksIServerPlayer player)
    {
        // Spawn the avatar at a random position in a circle with radius 5 at a height of 10.
        ksVector2 position = m_random.NextVector2() * 5f;
        ksIServerEntity avatar = Room.SpawnEntity("Avatar", new ksVector3(position.X, 10f, position.Y));
        if (Controller != null)
        {
            // Create and attach a copy of the controller asset.
            avatar.SetController(Controller.Clone(), player);
        }
        else
        {
            // If there is no controller asset, construct a new AvatarController and attach it.
            avatar.SetController(new AvatarController(), player);
        }

        AvatarController controller = avatar.PlayerController as AvatarController;
        if (controller != null)
        {
            // Spawn a bullet when the shoot delegate is called.
            controller.OnShoot = () =>
            {
                ksIServerEntity bullet = Room.SpawnEntity("Bullet", avatar.Transform.Position);
                bullet.Scripts.Get<ksRigidBody>().Velocity = avatar.Transform.Forward() * BULLET_SPEED;

                // Set the bullet's owner to the avatar who fired the bullet.
                bullet.Scripts.Get<ServerBullet>().Owner = avatar;
            };
        }
    }

    // Handle player join events.
    private void OnPlayerJoin(ksIServerPlayer player)
    {
        ksLog.Info("Player " + player.Id + " joined");
        SpawnAvatar(player);
    }

    // Handle player leave events.
    private void OnPlayerLeave(ksIServerPlayer player)
    {
        ksLog.Info("Player " + player.Id + " left");
        // Destroy the avatar when the player disconnects.
        player.DestroyControlledEntities();
    }

    // Called once per frame.
    private void Update()
    {
        // Check the Y position of all entities and destroy those with values less than -10.
        // This prevents dynamic entities such as the cube from falling forever after they fall off the platform.
        foreach (ksIServerEntity entity in Room.Entities)
        {
            if (entity.Transform.Position.Y < -10.0f)
            {
                entity.Destroy();
            }
        }
    }
}