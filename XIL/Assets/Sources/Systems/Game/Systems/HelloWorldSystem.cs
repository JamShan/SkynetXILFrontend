using Entitas;

public class HelloWorldSystem : IInitializeSystem
{
    readonly Contexts m_contexts;

    public HelloWorldSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Initialize()
    {
        UnityEngine.Debug.Log("++++++++++Hello World+++++++++++");
    }
}
