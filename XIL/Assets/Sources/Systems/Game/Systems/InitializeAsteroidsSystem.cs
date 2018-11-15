using UnityEngine;
using Entitas;

public class InitializeAsteroidsSystem : IInitializeSystem
{
    readonly Contexts m_contexts;

    public InitializeAsteroidsSystem(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void Initialize()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);
            GameEntityService.singleton.CreateAsteroid(3, pos);
        }
    }
}

