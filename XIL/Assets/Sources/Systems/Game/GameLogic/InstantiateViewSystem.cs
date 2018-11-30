using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Entitas.Unity;


public class InstantiateViewSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts m_contexts;

    public InstantiateViewSystem(Contexts contexts) : base(contexts.game)
    {
        m_contexts = contexts;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasResource;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Resource);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var entity in entities)
        {
            var gameobj = Object.Instantiate(entity.resource.prefab);
            entity.AddView(gameobj);
            gameobj.Link(entity, m_contexts.game);

            if(entity.hasInitialPosition)
            {
                gameobj.transform.position = entity.initialPosition.value;
            }
        }
    }
}
