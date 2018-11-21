using SkynetClient;
using UnityEngine;

public class InputLoginService
{
    public static InputLoginService singleton = new InputLoginService();

    private Contexts m_contexts;

    private bool is_do = false;

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
        if (is_do)
        {
            return;
        }
        Debug.Log("Login Result DoLoginLogic");
        is_do = true;

        LoginAuthHttp.Test();
        AuthPackage cmd = m_contexts.input.loginCMDEntity.loginCMD.cmd;
        //LoginAuthHttp.DoLoginReqAction(cmd, DoLoginRespAction);
    }

    private void DoLoginRespAction(AuthPackageResp resp)
    {
        NLog.Log.Debug("+++++DoLoginRespAction Resp+++++++++" + resp.gate);
        //Debug.Log("#################" + resp.gate);
    }
}
