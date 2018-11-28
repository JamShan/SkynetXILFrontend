using SkynetClient;
using UnityEngine;

public class InputLoginService
{
    public static InputLoginService singleton = new InputLoginService();

    private Contexts m_contexts;

    private string host = "192.168.1.25";
    private int port = 15110;

    public void Initialize(Contexts contexts)
    {
        m_contexts = contexts;
    }

    public void CreateLoginCMDEntity()
    {
        AuthPackage cmd = new AuthPackage();
        cmd.openId = "test_xil_001";
        cmd.sdk = "2";
        cmd.serverId = "1";
        cmd.pf = "1010";
        cmd.userData = "test_xil_001";

        m_contexts.input.ReplaceLoginCMD(cmd);
    }

    public void DoLoginLogic(InputEntity entity)
    {
        Debug.Log("Login Result DoLoginLogic");
        //XIL.AI.Behavior3Sharp.TestB3.Test();
        //LoginAuthHttp.Test();
        AuthPackage cmd = m_contexts.input.loginCMDEntity.loginCMD.cmd;
        LoginAuthHttp.DoLoginReqAction(cmd, DoLoginRespAction);

        m_contexts.input.RemoveLoginCMD();
    }

    private void DoLoginRespAction(AuthPackageResp resp)
    {
        string str = DebugHelper.DumpJson<AuthPackageResp>(resp);
        Debug.Log(str);
        //NLog.Log.Debug("+++++DoLoginRespAction Resp+++++++++" + resp.gate);
    }
}
