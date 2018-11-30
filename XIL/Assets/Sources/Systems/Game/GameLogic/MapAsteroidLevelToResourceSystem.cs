using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class MapAsteroidLevelToResourceSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts m_contexts;

    public MapAsteroidLevelToResourceSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAsteroid;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Asteroid);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var setup = m_contexts.game.gameDataSetup.value;
        foreach (var entity in entities)
        {
            GameObject game_obj = MapAsteroidLevelToResource(entity.asteroid.level, setup);
            entity.AddResource(game_obj);
            var random_angle = Random.Range(0f, 2f);
            var speed = setup.asteroidSpeed;
            entity.AddAcceleration(new Vector3(
                speed * Mathf.Cos(random_angle),
                speed * Mathf.Sin(random_angle),
                0f));
        }
    }

    private GameObject MapAsteroidLevelToResource(int level, GameDataSetup setup)
    {
        switch (level)
        {
            case 0:
                return setup.tinys[Random.Range(0, setup.tinys.Length)];
            case 1:
                return setup.smalls[Random.Range(0, setup.smalls.Length)];
            case 2:
                return setup.meds[Random.Range(0, setup.meds.Length)];
            default:
                return setup.bigs[Random.Range(0, setup.bigs.Length)];
        }
    }
}
