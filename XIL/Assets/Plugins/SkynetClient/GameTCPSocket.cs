using SprotoUnity;
using Sproto;

namespace SkynetClient
{
    public class HandShakePackage
    {
        public string openId = "";
        public int sdk = 0;
        public int worldId = 1;
        public int pf = 1;
        public string userData = "";
    }

    public class GameTCPSocket
    {
        private string name = "";
        private HandShakePackage handshake_package = null;


        public void Init(string host, int port, HandShakePackage handshake_package)
        {
            this.handshake_package = handshake_package;
            this.name = string.Format("Game Server host:{0} port:{1}", host, port);
            NetCore.Connect(host, port, OnConnected);
        }

        public void OnConnected()
        {
            NetSender.Init();
            NetReceiver.Init();
            // TODO: do handshake rpc
        }

        //SprotoType.Handshake.request req = new SprotoType.Handshake.request();
        //req.uid = uid;
        //req.token = token;

        //NetSender.Send<Protocol.Handshake>(req, (_) =>
        //{
	       // SprotoType.Handshake.response rsp = _ as SprotoType.Handshake.response;
	       // if (rsp.result == 0)
	       // {
	       // }
        //});

        public  void Send<T>(SprotoTypeBase rpcReq = null, RpcRspHandler rpcRspHandler = null)
        {
            NetSender.Send<T>(rpcReq, rpcRspHandler);
        }

        public void Dispatch()
        {
            NetCore.Dispatch();
        }

        public void Disconnect()
        {
            NetCore.Disconnect();
        }

    }
}

