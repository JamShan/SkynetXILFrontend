using System.Collections;
using System.Collections.Generic;
using Entitas;

public sealed class GameFSMReturnSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    FSMDebugService fsm_debug_service = FSMDebugService.singleton;

    readonly IGroup<GameEntity> m_fsm_return_entities;

    public GameFSMReturnSystem(Contexts contexts):base(contexts.game)
    {
        m_fsm_return_entities = contexts.game.GetGroup(GameMatcher.FSMReturn);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFSMState & entity.hasFSMStack & entity.isFSMReturn & !entity.hasFSMHold & !entity.hasFSMSwitch;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.FSMReturn.Added());
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach(var e in entities)
        {
            //Return FSM
            if (e.fSMStack.fsm_stack.Count <= 1)
            {
                fsm_debug_service.fsm_msg_report("Warning!! FSM is alredy in last state");
            }
            else
            {
                //e.fSMState.previous_state = e.fSMState.current_state;
                //e.fSMState.current_state = e.fSMStack.fsm_stack [0];
                GAMESTATE _state = e.fSMStack.fsm_stack[e.fSMStack.fsm_stack.Count - 2];
                e.AddFSMSwitch(false, GAMESTATE.GAME_IGNORE, _state);
                e.fSMStack.fsm_stack.RemoveAt(e.fSMStack.fsm_stack.Count - 1);
            }
        }
    }

    public void Cleanup()
    {
       foreach(var e in m_fsm_return_entities.GetEntities())
        {
            if (!e.hasFSMHold)
                e.isFSMReturn = false;
        }
    }

}
