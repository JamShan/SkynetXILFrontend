using UnityEngine;

public class GameEntityService 
{
    public static GameEntityService singleton = new GameEntityService();

    Contexts m_contexts;

    public void Initialize(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public GameEntity CreatePlayer()
    {
        var entity = m_contexts.game.CreateEntity();
        entity.isPlayer = true;
        entity.AddResource(m_contexts.game.gameDataSetup.value.player);
        entity.AddInitialPosition(Vector3.zero);
        //entity.AddInput(Vector3.zero);
        return entity;
    }
}
