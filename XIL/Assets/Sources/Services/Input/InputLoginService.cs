using SkynetClient;
using UnityEngine;

public class InputLoginService
{
    public static InputLoginService singleton = new InputLoginService();

    private Contexts m_contexts;

    private LoginAuthSocket socket = new LoginAuthSocket();
    private bool is_do = false;

    private string host = "192.168.1.25";
    private int port = 15110;

    public void Initialize(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public InputEntity CreateLoginCMDEntity()
    {
        var entity = m_contexts.input.CreateEntity();
        AuthPackage cmd = new AuthPackage();
        cmd.openId = "test_xil_001";
        cmd.sdk = "2";
        cmd.serverId = "1";
        cmd.pf = "1010";
        cmd.userData = "test_xil_001";
        entity.AddLoginCMD(cmd);
        return entity;
    }

    public void DoLoginLogic(InputEntity entity)
    {
        if(is_do)
        {
            return;
        }
        Debug.Log("Login Result DoLoginLogic");
        is_do = true;
        socket.Init(entity.loginCMD.cmd, OnLoginResult);
        socket.Connect(host, port, 5);
    }

    private void OnLoginResult(int code)
    {
        socket.Close();
        Debug.Log("Login Result code:" + code);
        // TODO code == 200 为成功
    }
}
