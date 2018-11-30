using UnityEngine;
using Entitas;


public class GameFSMController : MonoBehaviour
{
    public GameObject[] static_event_listerner_objs;
    public GameFSMServices m_services = GameFSMServices.singleton;
    Systems m_systems;

    void Awake()
    {
        var contexts = Contexts.sharedInstance;
        m_services.Initialize(contexts, this);

        m_systems = new GameFSMSystems(contexts);
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
