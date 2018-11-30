//created by snippet - ecs_entity_services
using UnityEngine;

public class FSMDebugService {
	public static FSMDebugService singleton = new FSMDebugService();
	public void Initialize(Contexts contexts) {
	}

	public void fsm_msg_report(string message) {
		Debug.Log(message);
	}

}