//created by snippet - ecs_react_cleanup_system
using System.Collections.Generic;
using Entitas;

public sealed class GameFSMProcessingSystem : ReactiveSystem<GameEntity> {
	//connect services
	FSMDebugService fsm_debug_service = FSMDebugService.singleton;

	public GameFSMProcessingSystem(Contexts contexts) : base(contexts.game){
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
		return context.CreateCollector (GameMatcher.FSMProcessingCnt);
	}

	protected override bool Filter(GameEntity entity) {
		return entity.isFSMProcessing & entity.hasFSMProcessingCnt;
	}

	protected override void Execute(List<GameEntity> entities) {
		fsm_debug_service.fsm_msg_report ("fsm processing execute : ");
		foreach (var e in entities)
		{
			//fsm_debug_service.fsm_msg_report ( e.fSMProcessingCnt.listener_busy_cnt.ToString() );
			if (e.fSMProcessingCnt.listener_busy_cnt == 0) {
				e.isFSMProcessing = false;
				e.RemoveFSMProcessingCnt ();
			}
		}
	}
		

}