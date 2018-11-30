using UnityEngine;
using Entitas;

public class MoveSystem : IExecuteSystem
{
    readonly Contexts m_contexts;
    private IGroup<GameEntity> m_group;

    public MoveSystem(Contexts contexts)
    {
        m_contexts = contexts;

        m_group = m_contexts.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Acceleration, GameMatcher.View));
    }

    public void Execute()
    {
        foreach(var entity in m_group)
        {
            var view = entity.view.value;
            var acceleration = entity.acceleration.value;
            var position = view.transform.position;
            position += acceleration * Time.deltaTime;
            view.transform.position = position;
        }
    }
}
