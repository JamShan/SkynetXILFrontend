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
        entity.AddAcceleration(Vector3.zero);
        return entity;
    }

    public GameEntity CreateLaser()
    {
        var entity = m_contexts.game.CreateEntity();
        var setup = m_contexts.game.gameDataSetup.value;
        entity.AddResource(setup.laser);
        var player_transform = m_contexts.game.playerEntity.view.value.transform;
        var player_forward = player_transform.up;
        entity.AddAcceleration(player_forward * setup.laserSpeed);
        entity.AddInitialPosition(player_transform.position);
        entity.isLaser = true;
        return entity;
    }

    public GameEntity CreateAsteroid(int level, Vector3 pos)
    {
        var entity = m_contexts.game.CreateEntity();
        entity.AddAsteroid(level);
        entity.AddInitialPosition(pos);
        return entity;
    }

}
