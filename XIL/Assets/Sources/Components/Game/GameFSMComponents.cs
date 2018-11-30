﻿using System.Collections.Generic;
using UnityEngine;

using Entitas;
using Entitas.CodeGeneration.Attributes;

public enum GAMESTATE
{
    GAME_INIT, //初始化
    GAME_LOGIN,  //登录
    GAME_AUTH, //游戏服验证
    GAME_SCENE, //游戏场景中
    GAME_END, //结束
    GAME_IGNORE	//不指定state, 相当于-1 don't care
}

[Game]
public sealed class FSMStateComponent : IComponent
{
    public GAMESTATE current_state;
    public GAMESTATE previous_state;
}

[Game]
public sealed class FSMSwitchComponent : IComponent
{
    public bool stack;           //決定这次switch是否要stack 目前的 fsm_state, =true : 需要stack
    public GAMESTATE from_state; //GAME_IGNORE: don't care, otherwise need match from GAMESTATE
    public GAMESTATE to_state;   //to GAMESTATE
}

[Game]
public sealed class FSMHoldComponent : IComponent
{
    public string reason; 
}

[Game]
public sealed class FSMContinueComponent : IComponent
{
    public string reason;
}

[Game]
public sealed class FSMResetComponent : IComponent
{
    public string reason;
}

//記錄 fsm stack , 實現stack fsm 功能
[Game]
public sealed class FSMStackComponent : IComponent
{
    public List<GAMESTATE> fsm_stack;
}

//popup fsm stack, return fsm到上一個state
[Game]
public sealed class FSMReturnComponent : IComponent
{

}


//=======================================================
//a real fsm processing event
[Game]
[Event(EventTarget.Any)] // TODO:待确定使用哪种类型
public sealed class FSMProcessingComponent : IComponent
{
    //這個是一個flag ,代表FSM正在 processing (Listener 沒全做完)
    //Note : 原本 listener_busy_cnt做在這,但是這個component跟event結合
    //eventSystem會在此component 有變動時trigger event listener
    //這樣無法讓每個event listener 在生成一次FSMProcessing時只做一次
    //(會因為listener_cnt變動做很多次) 故這樣修改
}
[Game]
public sealed class FSMProcessingCntComponent : IComponent
{
    public int listener_busy_cnt;
    //這個會在此component建立時收集該entity的 listerner 個數 (並同時發出 fsmhold)
    //當 listener收到event時, 會將此busy_cnt - 1,
    //當 busy_cnt == 0 時,表示fsm processing完成 (並發出 fsmcontinue)
}
//========================================================
//這是FSM trigger event 送到Mono gameobject 所需要的component
//當此component一新增時, 會觸發 FSMAdapterSystem 
//進行 gameobject 生成,附帶 FSMAdapter script 上去
//新增 Listerner 上去
//再進行 Listener 連結
[Game]
public sealed class FSMAdapterComponent : IComponent
{
    public GameObject game_object;
}