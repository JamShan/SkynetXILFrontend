public sealed class InputSystems : Feature
{
    public InputSystems(Contexts contexts)
    {
        Add(new LoginCMDSystem(contexts));

        //TODO: 暂时 立刻出发 稍后 添加 UI 触发
        Add(new InitializeLoginCMDSystem(contexts));
    }
}
