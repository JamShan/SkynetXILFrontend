using Entitas;

public class InitializePlayerSystem : IInitializeSystem
{
    readonly Contexts m_contexts;

    public InitializePlayerSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Initialize()
    {
        GameEntityService.singleton.CreatePlayer();
    }
}
