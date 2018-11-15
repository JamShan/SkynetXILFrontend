using UnityEngine;
using Entitas;

public class InputController : MonoBehaviour
{
    public InputServices m_services = InputServices.singleton;
    Systems m_systems;

    private void Awake()
    {
        var contexts = Contexts.sharedInstance;
        m_services.Initialize(contexts, this);
        m_systems = new InputSystems(contexts);
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