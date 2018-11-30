using UnityEngine;
using Entitas;

public class GameController : MonoBehaviour
{
    public GameDataSetup m_setup;
    public GameServices m_services = GameServices.singleton;
    Systems m_systems;

    private void Awake()
    {
        var contexts = Contexts.sharedInstance;
        contexts.game.SetGameDataSetup(m_setup);
        m_services.Initialize(contexts, this);
        m_systems = new GameLogicSystems(contexts);
    }

    void Start()
    {
        m_systems.Initialize();
    }

    void Update()
    {
        m_systems.Execute();
        m_systems.Cleanup();
    }

    void OnDestroy()
    {
        m_systems.TearDown();
    }
}
