using System;

public class GameFSMServices
{
    public static GameFSMServices singleton = new GameFSMServices();

    public void Initialize(Contexts contexts, GameFSMController controller)
    {
        FSMDebugService.singleton.Initialize(contexts);
        FSMSwitchService.singleton.Initialize(contexts);
        FSMAdapterService.singleton.Initialize(contexts, controller);
    }
}
