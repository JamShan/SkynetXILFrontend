//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public FSMStackComponent fSMStack { get { return (FSMStackComponent)GetComponent(GameComponentsLookup.FSMStack); } }
    public bool hasFSMStack { get { return HasComponent(GameComponentsLookup.FSMStack); } }

    public void AddFSMStack(System.Collections.Generic.List<GAMESTATE> newFsm_stack) {
        var index = GameComponentsLookup.FSMStack;
        var component = (FSMStackComponent)CreateComponent(index, typeof(FSMStackComponent));
        component.fsm_stack = newFsm_stack;
        AddComponent(index, component);
    }

    public void ReplaceFSMStack(System.Collections.Generic.List<GAMESTATE> newFsm_stack) {
        var index = GameComponentsLookup.FSMStack;
        var component = (FSMStackComponent)CreateComponent(index, typeof(FSMStackComponent));
        component.fsm_stack = newFsm_stack;
        ReplaceComponent(index, component);
    }

    public void RemoveFSMStack() {
        RemoveComponent(GameComponentsLookup.FSMStack);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherFSMStack;

    public static Entitas.IMatcher<GameEntity> FSMStack {
        get {
            if (_matcherFSMStack == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.FSMStack);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherFSMStack = matcher;
            }

            return _matcherFSMStack;
        }
    }
}
