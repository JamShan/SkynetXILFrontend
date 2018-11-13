public sealed class GameSystems : Feature {

    public GameSystems(Contexts contexts) {

        Add(new InputSystem(contexts));
        Add(new ShootSystem(contexts));
        Add(new HitAsteroidSystem(contexts));

        //Init
        Add(new InitializePlayerSystem(contexts));
        Add(new InitializeAsteroidsSystem(contexts));

        Add(new InstantiateViewSystem(contexts));

        Add(new RotateLaserSystem(contexts));
        Add(new RotatePlayerSystem(contexts));

        Add(new MoveSystem(contexts));

        Add(new ReplaceAccelerationSystem(contexts));

        Add(new MapAsteroidLevelToResourceSystem(contexts));

        //// Events

        // Cleanup
        Add(new DestroyEntitySystem(contexts));
    }
}
