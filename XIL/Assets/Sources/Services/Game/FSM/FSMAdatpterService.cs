//created by snippet - ecs_entity_services
using Entitas.Unity;
using UnityEngine;

public class FSMAdapterService {

	FSMDebugService fsm_debug_service = FSMDebugService.singleton;

	public static FSMAdapterService singleton = new FSMAdapterService();

	Contexts m_contexts;

	public void Initialize(Contexts contexts, GameFSMController controller) {
        m_contexts = contexts;
	}

	//這個表示Scene內已有 掛有 event_listener 的gameobject
	//透過這個 function call來跟某個entity 做連結
	public void fsm_link_static_event_listener_gameobjects (GameEntity e) {
		fsm_debug_service.fsm_msg_report ("fsm_link_static_event_listener_gameobjects");
		//foreach (GameObject obj in _static_event_listener_objs) {
		//	obj.Link (e, _contexts.game);
		//	e.AddFSMProcessingListener (obj.GetComponent<IFSMProcessingListener>());
		//}
	}

	//這個是動態生成 event adapter時所需要的 function
	public void fsm_create_prefab_adapter(GameEntity e) {
		//GameObject _obj = (GameObject) GameObject.Instantiate (adapter_prefab, Vector3.zero, Quaternion.identity);
		//_obj.transform.SetParent (root_transform);
		//_obj.Link (e, _contexts.game);

		////_obj.AddComponent<IFSMProcessingListener>;

		//e.AddFSMProcessingListener (_obj.GetComponent<IFSMProcessingListener>());
	}

}