
using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;

public sealed class DestroyEntitySystem : ICleanupSystem
{
    readonly IGroup<GameEntity> m_group;
    readonly List<GameEntity> m_buffer = new List<GameEntity>();

    public DestroyEntitySystem(Contexts contexts)
    {
        m_group = contexts.game.GetGroup(GameMatcher.Destroyed);
    }

    public void Cleanup()
    {
        foreach (var entity in m_group.GetEntities(m_buffer))
        {
            Debug.Log("++++++++Cleanup+++++++");
            if (entity.hasView)
            {
                var view = entity.view.value;
                view.Unlink();
                Object.Destroy(view);
            }

            entity.Destroy();
        }
    }
}