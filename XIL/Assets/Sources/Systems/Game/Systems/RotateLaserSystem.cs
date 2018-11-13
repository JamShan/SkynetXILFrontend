using UnityEngine;
using Entitas;
using System.Collections.Generic;

public class RotateLaserSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts m_contexts;

    public RotateLaserSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView && entity.isLaser && entity.hasAcceleration;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(
            GameMatcher.View,
            GameMatcher.Laser,
            GameMatcher.Acceleration) );
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var entity in entities)
        {
            var view = entity.view.value.transform;
            var acceleration = entity.acceleration.value;
            view.transform.up = acceleration.normalized;
        }
    }

}
