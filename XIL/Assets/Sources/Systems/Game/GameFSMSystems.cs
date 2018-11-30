public sealed class GameFSMSystems : Feature {

    public GameFSMSystems(Contexts contexts) {
        //以下順序綁定
        //為了達成處理System hold著時, FSMSwitch依然留著, 當FSMContinue 解開FSMHold時
        //留在entity裡的FSMSwitch仍然可以被處裡
        //手法是當 Hold System處理FSMContine ,remove FSMHold時,會copy 一份原FSMSwitch
        //删除原來的FSMSwitch, 再新增加一個FSMSwitch,目的是為了trigger FSMSwitchSystem
        //達成以上功能
        Add(new GameFSMUniTestSystem(contexts)); //UniTest

        Add(new GameFSMHoldSystem(contexts));
        Add(new GameFSMReturnSystem(contexts));
        Add(new GameFSMSwitchSystem(contexts));
        Add(new GameFSMResetSystem(contexts));

        //Event Processing
        Add(new GameFSMAdapterSystem(contexts));
        Add(new GameFSMProcessingSystem(contexts));
        // auto gen event system
        Add(new FSMProcessingEventSystem(contexts));
    }
}
