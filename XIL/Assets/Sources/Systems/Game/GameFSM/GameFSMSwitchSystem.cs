using System.Collections;
using System.Collections.Generic;
using Entitas;

public sealed class GameFSMSwitchSystem : ReactiveSystem<GameEntity>, ICleanupSystem
{
    FSMDebugService fsm_debug_service = FSMDebugService.singleton;

    readonly IGroup<GameEntity> m_fsm_switch_entities;

    public GameFSMSwitchSystem(Contexts contexts):base(contexts.game)
    {
        m_fsm_switch_entities = contexts.game.GetGroup(GameMatcher.FSMSwitch);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasFSMState & entity.hasFSMSwitch & !entity.hasFSMHold & !entity.isFSMProcessing;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.FSMSwitch.Added());
    }

    protected override void Execute(List<GameEntity> entities)
    {
        //fsm_debug_service.fsm_msg_report ("==> switch execute");
        foreach (var e in entities)
        {
            if (e.fSMSwitch.from_state == GAMESTATE.GAME_IGNORE)
            { //don't care from state
                e.fSMState.previous_state = e.fSMState.current_state;
                e.fSMState.current_state = e.fSMSwitch.to_state;

                if (e.hasFSMStack & !e.isFSMReturn & e.fSMSwitch.stack)
                {
                    e.fSMStack.fsm_stack.Add(e.fSMSwitch.to_state);
                }

                fsm_trigger_processing(e);
            }
            else
            {
                if (e.fSMSwitch.from_state != e.fSMState.current_state)
                {
                    //something wrong! report error
                    //debug_service report
                    fsm_debug_service.fsm_msg_report("Error!! FSM from_state / current_state mismatch!!");
                }
                else
                {
                    e.fSMState.previous_state = e.fSMState.current_state;
                    e.fSMState.current_state = e.fSMSwitch.to_state;

                    if (e.hasFSMStack & !e.isFSMReturn & e.fSMSwitch.stack)
                    {
                        e.fSMStack.fsm_stack.Add(e.fSMSwitch.to_state);
                    }

                    fsm_trigger_processing(e);
                }
            }
        }
    }

    private void fsm_trigger_processing(GameEntity e)
    {
        if (e.hasFSMProcessingListener)
        {
            fsm_debug_service.fsm_msg_report("Trigger FSM processing, listener_cnt = " + e.fSMProcessingListener.value.Count);
            //記錄該entity 內的 FSMProcessingListener 的 listerner 個數
            //每個listener 會call-back ,並倒扣 FSMProcessing 的value - 1
            e.isFSMProcessing = true;
            e.AddFSMProcessingCnt(e.fSMProcessingListener.value.Count);
        }
    }

    public void Cleanup()
    {
            foreach (var e in m_fsm_switch_entities.GetEntities())
            {
                if (!e.hasFSMHold)
                    e.RemoveFSMSwitch();
            }
    }

}
