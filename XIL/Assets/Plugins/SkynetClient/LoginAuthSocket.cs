
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;


namespace SkynetClient
{
    public class AuthPackage
    {
        public string openId = "1";
        public string sdk = "2";
        public string serverId = "1";
        public string pf = "1";
        public string userData = "userData";
    }

    public class AuthPackageResp
    {
        public string uid = "";
        public string subId = "";
        public string gate = "";
        public int port = 0;
        public string secret = "";
    }

public enum Enum_Login_Auth_State : int
    {
        NIL, //无效
        GET_CHALLENGE,
        GET_SECRET,
        SEND_LOGIN,
        LOGIN_RESULT,
        LOGIN_FINISHED,
    }

    public delegate void LoginAuthResultFun(int code);
    public delegate void LoginAuthActionFun();

    public class LoginAuthSocket
    {
        private static string name = "";
        private static int overtime = 5;
        private static int max_recon_count = 2;
        private static int cur_recon_count = 0;

        private static TcpClient client = null;
        private static MemoryStream mem_stream = null;
        private static BinaryReader reader = null;

        private static int MAX_READ = 8192;
        private static byte[] byte_buffer = new byte[MAX_READ];

        private static string host;
        private static int port;

        private static Enum_Login_Auth_State state = Enum_Login_Auth_State.GET_CHALLENGE;
        private static LoginAuthResultFun logincb = null;
        private static LoginAuthActionFun loginaction = null;
        private static LoginAuthActionFun disconnectaction = null;

        public  static void Init(LoginAuthActionFun doAction, LoginAuthActionFun disAction,  LoginAuthResultFun cb)
        {
            logincb = cb;
            loginaction = doAction;
            disconnectaction = disAction;
        }

        public static void Connect(string h, int p, int ot)
        {
            Close();

            host = h;
            port = p;

            if (overtime > 0)
            {
                overtime = ot;
            }

            name = string.Format("LoginAuthSocket host:{0} port:{1}", host, port);
            mem_stream = new MemoryStream();
            reader = new BinaryReader(mem_stream);

            IPAddress[] addres = Dns.GetHostAddresses(host);
            if (addres.Length == 0)
            {
                Debug.LogError(string.Format("DNS 解析失败:{0}", name));
            }
            else
            {
                try
                {
                    client = null;
                    client = new TcpClient();
                    client.SendTimeout = 3000;
                    client.ReceiveTimeout = 3000;
                    client.NoDelay = true;
                    client.BeginConnect(addres, port, new AsyncCallback(OnConnect), null);
                }
                catch (Exception e)
                {
                    if (cur_recon_count <= max_recon_count)
                    {
                        cur_recon_count++;
                        Debug.LogError(string.Format("连接服务器异常:{0} 当前重连次数{1}", e.Message.ToString(), cur_recon_count));
                        Connect(host, port, overtime);
                    }
                    else
                    {
                        Debug.LogError(string.Format("连接服务器失败:{0}", name));
                    }
                }
            }
        }

        static void OnConnect(IAsyncResult asr)
        {
            try
            {
                client.EndConnect(asr);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("连接服务器异步结果异常:{0} ", e.Message.ToString()));
                return;
            }

            state = Enum_Login_Auth_State.GET_CHALLENGE;
            client.GetStream().BeginRead(byte_buffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
        }

        static public bool Connected()
        {
            return client != null && client.Client != null && client.Client.Connected;
        }

        static public void Close()
        {
            if (client != null)
            {
                if (client.Connected)
                {
                    client.Close();
                }
                client = null;
            }
        }

        static void OnReadLine(IAsyncResult asr)
        {
            try
            {
                int bytes_read = 0;
                lock (client.GetStream())
                {
                    bytes_read = client.GetStream().EndRead(asr);
                }
                if (bytes_read < 1)
                {         //服务器主动关闭，断线处理
                    OnDisconnected("server close socket", "bytesRead < 1");
                }
                else
                {
                    OnReceiveLine(byte_buffer, bytes_read);

                    Array.Clear(byte_buffer, 0, MAX_READ);   //清空数组
                    client.GetStream().BeginRead(byte_buffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
                }
            }
            catch (Exception e)
            {
                OnDisconnected("read byte exception: ", e.Message.ToString());
            }
        }

        static void OnReceiveLine(byte[] bytes, int length)
        {
            mem_stream.Seek(0, SeekOrigin.End);
            mem_stream.Write(bytes, 0, length);
            //Reset to beginning
            mem_stream.Seek(0, SeekOrigin.Begin);

            int lineLength = 0;
            bool checkEnd = false;
            while (checkEnd == false)//这里要变成找换行符
            {
                bool fullLine = false;
                int count = length - lineLength;
                int readIndex = 0;
                for (int i = 0; i < count; i++)
                {
                    char c = (char)reader.ReadByte();
                    lineLength++;
                    readIndex = i;
                    if (lineLength == length)
                        checkEnd = true;

                    if (c.Equals('\n'))
                    {
                        fullLine = true;
                        break;
                    }
                }

                mem_stream.Position = mem_stream.Position - (readIndex + 1);//回到句首

                //有换行符，完整的read line
                if (fullLine == true)
                {
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(ms);
                    writer.Write(reader.ReadBytes(lineLength));

                    ms.Seek(0, SeekOrigin.Begin);

                    BinaryReader r = new BinaryReader(ms);
                    char[] result = r.ReadChars(lineLength);
                    OnReceivedMessage(new string(result));
                }
                else
                {
                    Debug.LogError("密匙收到数据,但是找不到换行符号");
                }
            }

            //Create a new stream with any leftover bytes
            int remain_byte = (int)(mem_stream.Length - mem_stream.Position);
            byte[] leftover = reader.ReadBytes(remain_byte);
            mem_stream.SetLength(0);    
            mem_stream.Write(leftover, 0, leftover.Length);
        }

        static void OnReceivedMessage(string socketline)
        {
            Debug.Log("+++OnReceivedMessage: state:" + state  + " msg:" + socketline);
            switch (state)
            {
                case Enum_Login_Auth_State.GET_CHALLENGE:
                    GetChallenge(socketline);
                    break;
                case Enum_Login_Auth_State.GET_SECRET:
                    GetSecret(socketline);
                    break;
                case Enum_Login_Auth_State.SEND_LOGIN:
                    loginaction();
                    break;
                case Enum_Login_Auth_State.LOGIN_RESULT:
                    OnLoginResult(socketline);
                    break;
            }

        }

        private static ulong clientkey;
        private static ulong challenge;
        public static ulong secret;
        public static string game_ip;
        public static string game_port;
        public static string useid;
        public static string subid;

        static void GetChallenge(string socketline)
        {
            state = Enum_Login_Auth_State.GET_SECRET;

            byte[] challengeByte = Crypt.Base64Decode(socketline);
            if (challengeByte.Length == 8)
            {
                challenge = BitConverter.ToUInt64(challengeByte, 0);
                clientkey = Crypt.GenerateRandomUlong();
                ulong dhexchangekey = Crypt.dhexchange(clientkey);
                byte[] buf = BitConverter.GetBytes(dhexchangekey);
                SendMessage(buf);
            }
        }

        static void GetSecret(string socketstr)
        {
            byte[] lineByte = Crypt.Base64Decode(socketstr);
            if (lineByte.Length == 8)
            {
                ulong line = BitConverter.ToUInt64(lineByte, 0);
                secret = Crypt.dhsecret(line, clientkey);
                ulong hmac = Crypt.hmac64(challenge, secret);
                byte[] buf = BitConverter.GetBytes(hmac);

                SendMessage(buf);
                state = Enum_Login_Auth_State.SEND_LOGIN;
            }
        }

        public static void DoLoginAction(AuthPackage package)
        {
            if (state != Enum_Login_Auth_State.SEND_LOGIN)
            {
                Debug.LogError("不是第一次登录，也不是重新登录的情况下，执行DoLoginAction函数,loginState:" + state);
                return;
            }

            string token = EncodeToken(package);
            Crypt.SetDesKey(BitConverter.GetBytes(secret));
            byte[] etoken = Crypt.DesEncrypt(Encoding.UTF8.GetBytes(token));

            state = Enum_Login_Auth_State.LOGIN_RESULT;
            SendMessage(etoken);
        }

        static void OnLoginResult(string socketstr)
        {
            int code = int.Parse(socketstr.Substring(0, 3));
            byte[] subidByte = Crypt.Base64Decode(socketstr.Substring(4));
            string resultString = Encoding.UTF8.GetString(subidByte);
            ByteBuffer buffer = new ByteBuffer(UTF8Encoding.UTF8.GetBytes(200 + " " + resultString));
            if (code != 200)
            {
                Debug.LogError("Login Result" + resultString);
            }
            else
            {
                Debug.Log("OnLoginResult :" + resultString);

                Regex reg = new Regex(@"([^:]+):([^:]+)@([^@]+)@(.+)");
                string[] splites = Regex.Split(resultString, "[':', '@']");
                //ip, port, uid, subid
                game_ip = splites[0];
                game_port = splites[1];
                useid = splites[2];
                subid = splites[3];
            }

            state = Enum_Login_Auth_State.LOGIN_FINISHED;
            logincb(code);
        }

        static void SendMessage(byte[] buf)
        {
            if (!Connected())
            {
                Close();
                return;
            }

            try
            {
                NetworkStream stream = client.GetStream();
                if (stream.CanWrite)
                {
                    string base64key = Crypt.Base64Encode(buf) + '\n';
                    byte[] message = Encoding.UTF8.GetBytes(base64key);

                    stream.Write(message, 0, message.Length);
                    Debug.Log("密匙写数据：" + message.Length + " state: " + state + " msg:" + base64key);
                    DumpBytes(message);
                }
            }
            catch (SocketException e)
            {
                Debug.LogError("SendMessage Socket exception: " + e.Message.ToString());
            }
        }

        static void OnDisconnected(string desc, string msg)
        {
            Close();   //关掉客户端链接
            if (state != Enum_Login_Auth_State.LOGIN_FINISHED)
            {
                Debug.LogError("链接关闭,但loginState != LoginStateEnum.LOGIN_FINISHED,loginState:" + state);
            }
            Debug.LogError(string.Format("LoginAuth: OnDisconnected Msg:[{0}]==== Desc:[{1}]", msg, desc));

            disconnectaction();
        }

        static string EncodeToken(AuthPackage auth_package)
        {
            return string.Format("{0}@{1}@{2}@{3}@{4}", Crypt.Base64Decode(auth_package.openId),
                        Crypt.Base64Decode(auth_package.sdk),
                       Crypt.Base64Decode(auth_package.serverId),
                       Crypt.Base64Decode(auth_package.pf),
                       Crypt.Base64Decode(auth_package.userData));
        }

        static public void DumpBytes(byte[] buf)
        {
            string returnStr = string.Empty;
            for (int i = 0; i < buf.Length; i++)
            {
                returnStr += buf[i].ToString("X2");
            }
            Debug.Log(returnStr);
        }

    }


}