﻿//created by snippet - ecs_init_system
using System.Collections.Generic;
using Entitas;
using UnityEngine;


public sealed class GameFSMUniTestSystem : IExecuteSystem , IInitializeSystem {
	//connect services
	FSMDebugService fsm_debug_service = FSMDebugService.singleton;
	FSMSwitchService fsm_switch_service = FSMSwitchService.singleton;
	FSMAdapterService fsm_adapter_service = FSMAdapterService.singleton;

	GameEntity fsm_entity;

	public GameFSMUniTestSystem(Contexts contexts) {
	}

	public void Initialize() {
		fsm_entity = fsm_switch_service.fsm_init();

		//將static event_listener 連結起來
		fsm_adapter_service.fsm_link_static_event_listener_gameobjects (fsm_entity);

	}


	public void Execute() {
		//GameEntity e = game_fsm_entity.GetSingleEntity(); //對Game FSM而言,應該是singletone entity才對
		GameEntity e = fsm_entity;

		if (Input.GetKeyDown (KeyCode.A))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed A");
			fsm_switch_service.fsm_switch (e, GAMESTATE.GAME_IGNORE, GAMESTATE.GAME_LOGIN);
		}
        else if (Input.GetKeyDown(KeyCode.B))
        {
            fsm_debug_service.fsm_msg_report("FSM Unit Test : pressed B");
            fsm_switch_service.fsm_switch(e, GAMESTATE.GAME_LOGIN, GAMESTATE.GAME_AUTH);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            fsm_debug_service.fsm_msg_report("FSM Unit Test : pressed C");
            fsm_switch_service.fsm_switch(e, GAMESTATE.GAME_AUTH, GAMESTATE.GAME_SCENE);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            fsm_debug_service.fsm_msg_report("FSM Unit Test : pressed D");
            fsm_switch_service.fsm_switch(e, GAMESTATE.GAME_SCENE, GAMESTATE.GAME_END);
        }
        else if (Input.GetKeyDown (KeyCode.F))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed F");
			//e.AddFSMReset ("Keycode E pressed test");
			fsm_switch_service.fsm_reset (e, "Keycode E pressed test");

		}
        else if (Input.GetKeyDown (KeyCode.H))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed H, Hold flag");
			//e.AddFSMHold ("Add Hold flag to hold FSM state");
			fsm_switch_service.fsm_hold (e, "Keycode E pressed test");
		}
        else if (Input.GetKeyDown (KeyCode.S))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed S, Continue flag");
			//e.AddFSMContinue ("Add Continue flag to Continue FSM state");
			fsm_switch_service.fsm_continue (e, "Add Continue flag to Continue FSM state");
		}
        else if (Input.GetKeyDown (KeyCode.R))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed R, Return flag");
			//e.isFSMReturn = true;
			fsm_switch_service.fsm_return (e);
		}
        else if (Input.GetKeyDown (KeyCode.P))
        {
			fsm_debug_service.fsm_msg_report ("FSM Unit Test : pressed P, Create one event adapter");
			fsm_adapter_service.fsm_create_prefab_adapter (fsm_entity);
		}
	}

}