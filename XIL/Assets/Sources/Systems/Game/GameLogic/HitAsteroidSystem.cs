using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class HitAsteroidSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts m_contexts;

    public HitAsteroidSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollision;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(
            GameMatcher.Collision ));
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var first = entity.collision.first;
            var second = entity.collision.second;

            var first_entity = m_contexts.game.GetEntitiesWithView(first).SingleEntity();
            var second_entiy = m_contexts.game.GetEntitiesWithView(second).SingleEntity();

            if(second_entiy.asteroid.level > 0)
            {
                for (int i=0; i<2; i++)
                {
                    Vector3 pos = second.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0f);
                    var n_entity = GameEntityService.singleton.CreateAsteroid(second_entiy.asteroid.level - 1, pos);
                }
            }

            first_entity.isDestroyed = true;
            second_entiy.isDestroyed = true;
        }
    }

}
