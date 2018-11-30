using System.Collections;
using System.Collections.Generic;
using Entitas;

public sealed class GameFSMHoldSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    readonly Contexts m_contexts;
    readonly IGroup<GameEntity> m_fsm_continue_entities;

    public GameFSMHoldSystem(Contexts contexts):base(contexts.game)
    {
        m_contexts = contexts;
        m_fsm_continue_entities = contexts.game.GetGroup(GameMatcher.FSMContinue);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFSMHold && entity.hasFSMContinue;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.FSMContinue.Added());
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var e in entities)
        {
            e.RemoveFSMHold();
            if(e.hasFSMSwitch)
            {
                var from_state = e.fSMSwitch.from_state;
                var to_state = e.fSMSwitch.to_state;
                bool stack = e.fSMSwitch.stack;
                e.RemoveFSMSwitch();
                e.AddFSMSwitch(stack, from_state, to_state);
            }

            if(e.isFSMReturn)
            {
                //如果有殘留FSMReturn, 刪除,並重生FSMReturn trigger FSMSwitch System
            }
        }
    }

    public void Cleanup()
    {
       foreach(var e in m_fsm_continue_entities.GetEntities())
        {
            e.RemoveFSMContinue();
        }
    }

}
