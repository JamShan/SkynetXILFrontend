//created by snippet - ecs_react_cleanup_system
using System.Collections.Generic;
using Entitas;


public sealed class GameFSMResetSystem : ReactiveSystem<GameEntity>, ICleanupSystem{

	readonly IGroup<GameEntity> m_fsm_reset_entities;

	public GameFSMResetSystem(Contexts contexts) : base(contexts.game){
		m_fsm_reset_entities = contexts.game.GetGroup(GameMatcher.FSMReset);
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
		return context.CreateCollector (GameMatcher.FSMReset.Added());
	}

	protected override bool Filter(GameEntity entity) {
		return entity.hasFSMState & entity.hasFSMReset;
	}

	protected override void Execute(List<GameEntity> entities) {
		//_contexts.gameState.ReplaceScore (_contexts.gameState.score.value + entities.Count);
		foreach (var e in entities)
		{
			e.fSMState.previous_state = GAMESTATE.GAME_INIT;
			e.fSMState.current_state = GAMESTATE.GAME_INIT;

			if (e.hasFSMStack) {
				e.fSMStack.fsm_stack.Clear ();
				e.fSMStack.fsm_stack.Add (GAMESTATE.GAME_INIT);
			}

			if (e.isFSMProcessing) {
				e.isFSMProcessing = false;
				e.RemoveFSMProcessingCnt ();
			}
		}
	}

	public void Cleanup() {
		foreach(var e in m_fsm_reset_entities.GetEntities()) {
			if (e.hasFSMHold)
				e.RemoveFSMHold ();
			
			e.RemoveFSMReset();
		}
	}

}