//created by snippet - ecs_react_init_system
using System.Collections.Generic;
using Entitas;


public sealed class GameFSMAdapterSystem : ReactiveSystem<GameEntity>, ICleanupSystem {

	FSMAdapterService fsm_adapter_service = FSMAdapterService.singleton;

	readonly IGroup<GameEntity> m_fsm_adapter_entities;

	public GameFSMAdapterSystem(Contexts contexts) : base(contexts.game){
        m_fsm_adapter_entities = contexts.game.GetGroup(GameMatcher.FSMAdapter);
	}

	protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) {
		return context.CreateCollector (GameMatcher.FSMAdapter);
	}

	protected override bool Filter(GameEntity entity) {
		return entity.hasFSMAdapter;
	}

	protected override void Execute(List<GameEntity> entities) {
		foreach (var e in entities)
		{
			fsm_adapter_service.fsm_create_prefab_adapter (e);
		}
	}


	public void Cleanup() {
		foreach(var e in m_fsm_adapter_entities.GetEntities()) {
			e.RemoveFSMAdapter ();
		}
	}

}