using UnityEngine;
using Entitas;

public class InitializeLoginCMDSystem : IInitializeSystem
{
    readonly Contexts m_contexts;

    public InitializeLoginCMDSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Initialize()
    {
        InputLoginService.singleton.CreateLoginCMDEntity();
    }
}


