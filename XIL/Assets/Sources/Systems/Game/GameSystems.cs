public sealed class GameSystems : Feature {

    public GameSystems(Contexts contexts) {
        //Test
        Add(new HelloWorldSystem(contexts));

        //Init
        Add(new InitializePlayerSystem(contexts));

        Add(new InstantiateViewSystem(contexts));

        Add(new InputSystem(contexts));

        //// Events
        //Add(new InputEventSystems(contexts));
        //Add(new GameEventSystems(contexts));
        //Add(new GameStateEventSystems(contexts));

        //// Cleanup
        //Add(new DestroyEntitySystem(contexts));
    }
}
