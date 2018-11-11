
using System;

public class GameServices 
{
    public static GameServices singleton = new GameServices();

    public void Initialize(Contexts contexts, GameController gameController)
    {
        var random = new Random(DateTime.UtcNow.Millisecond);
        UnityEngine.Random.InitState(random.Next());

        GameEntityService.singleton.Initialize(contexts);
    }
}
