
using System;
using System.IO;
using System.Collections.Generic;
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

    public enum Enum_Login_Auth_State: int
    {
        NIL, //无效
        GET_CHALLENGE,
        GET_SECRET,
        SEND_LOGIN,
        LOGIN_RESULT,
        LOGIN_FINISHED,
    }

    public delegate void LoginAuthResultFun(int code);


    public class LoginAuthSocket
    {
        private AuthPackage auth_package;

        private string name = "";
        private bool connected = false;
        private int overtime = 3;
        private int max_recon_count = 2;
        private int cur_recon_count = 0;

        private TcpClient client = null;
        private NetworkStream out_stream = null;
        private MemoryStream mem_stream = null;
        private BinaryReader reader = null;

        private const int MAX_READ = 8192;
        private byte[] byte_buffer = new byte[MAX_READ];

        private string host;
        private int port;

        private Enum_Login_Auth_State state = Enum_Login_Auth_State.NIL;
        private LoginAuthResultFun cb = null;

        public LoginAuthSocket()
        {
        }

        public void Init(AuthPackage auth_package, LoginAuthResultFun cb)
        {
            this.auth_package = auth_package;
            this.cb = cb;
        }

        public void Connect(string host, int port, int overtime)
        {
            this.Close();

            this.host = host;
            this.port = port;

            if(overtime > 0)
            {
                this.overtime = overtime;
            }

            this.name = string.Format("LoginAuthSocket host:{0} port:{1}", host, port);

            IPAddress[] addres = Dns.GetHostAddresses(host);
            if(addres.Length == 0)
            {
                Debug.LogError(string.Format("DNS 解析失败:{0}", this.name) );
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
                catch(Exception e)
                {
                    if (cur_recon_count <= max_recon_count)
                    {
                        cur_recon_count++;
                        Debug.LogError(string.Format("连接服务器异常:{0} 当前重连次数{1}" , e.Message, cur_recon_count) );
                        Connect(host, port, overtime);
                    }
                    else
                    {
                        Debug.LogError(string.Format("连接服务器失败:{0}", this.name));
                    }
                }
            }
        }

         void OnConnect(IAsyncResult asr)
        {
            connected = true;
            try
            {
                client.EndConnect(asr);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("连接服务器异步结果异常:{0} ", e.Message));
                return;
            }

            this.state = Enum_Login_Auth_State.GET_CHALLENGE;
            ReadLineBytes();
        }

        public bool Connected()
        {
            return client != null && client.Client != null && client.Client.Connected;
        }

        public void Close()
        {
            connected = false;
            if (client != null)
            {
                if (client.Connected)
                {
                    client.Close();
                }
                client = null;
            }
        }

        public void DumpBytes()
        {
            string returnStr = string.Empty;
            for (int i = 0; i < byte_buffer.Length; i++)
            {
                returnStr += byte_buffer[i].ToString("X2");
            }
            Debug.LogError(returnStr);
        }


        private void ReadLineBytes()
        {
            out_stream = client.GetStream();
            client.GetStream().BeginRead(byte_buffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
            //ByteStream buffer = new ByteStream(UTF8Encoding.UTF8.GetBytes("OnConnect"));
            //q_byte_stream.Enqueue(new KeyValuePair<int, ByteStream>((int)state, buffer));
        }

        void OnReadLine(IAsyncResult asr)
        {
            try
            {
                int bytes_read = 0;

                lock (client.GetStream() )
                {
                    bytes_read = client.GetStream().EndRead(asr);
                }

                if (bytes_read < 1)
                {         //包尺寸有问题，断线处理
                    OnDisconnected("server close socket", "bytesRead < 1");
                    return;
                }

                OnReceiveLine(byte_buffer, bytes_read);

                lock (client.GetStream())
                {         //分析完，再次监听服务器发过来的新消息
                    Array.Clear(byte_buffer, 0, byte_buffer.Length);   //清空数组
                    client.GetStream().BeginRead(byte_buffer, 0, MAX_READ, new AsyncCallback(OnReadLine), null);
                }

            }
            catch(Exception e)
            {
                OnDisconnected("read byte error: " , e.Message);
            }
        }

        void OnReceiveLine(byte[] bytes, int length)
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
                    if (lineLength == length) checkEnd = true;
                    if (c.Equals('\n') || c.Equals('\0') || c.Equals('\r'))
                    {
                        fullLine = true;
                        break;
                    }
                }

                mem_stream.Position = mem_stream.Position - (readIndex + 1);//回到句首

                //有换行符，完整的read line
                if (fullLine == true)
                {
                    //Debug.Log("密匙收到一句数据");
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
            int remain_byte =(int) (mem_stream.Length - mem_stream.Position );
            byte[] leftover = reader.ReadBytes(remain_byte);
            mem_stream.SetLength(0);     //Clear
            mem_stream.Write(leftover, 0, leftover.Length);
        }

        void OnReceivedMessage(string socketline)
        {
            switch (this.state)
            {
                case Enum_Login_Auth_State.GET_CHALLENGE:
                    GetChallenge(socketline);
                    break;
                case Enum_Login_Auth_State.GET_SECRET:
                    GetSecret(socketline);
                    break;
                case Enum_Login_Auth_State.SEND_LOGIN:
                    OnLogin();
                    break;
                case Enum_Login_Auth_State.LOGIN_RESULT:
                    OnLoginResult(socketline);
                    break;
            }

        }

        private byte[] challenge;
        private byte[] clientkey;
        public byte[] secret;
        public string game_ip;
        public string game_port;
        public string useid;
        public string subid;

        void GetChallenge(string socketline)
        {
            byte[] challengeByte = Crypt.Base64Decode(socketline);
            if (challengeByte.Length == 8)
            {
                challenge = challengeByte;
                clientkey = Crypt.RandomKey();
                byte[] dhexchangekey = Crypt.DHExchange(clientkey);
                string base64key = Crypt.Base64Encode(dhexchangekey) + '\n';
                byte[] message = Encoding.UTF8.GetBytes(base64key);
                WriteMessage(message);

                this.state = Enum_Login_Auth_State.GET_SECRET;
            }
        }

        void GetSecret(string socketstr)
        {
            byte[] lineByte = Crypt.Base64Decode(socketstr);
            if (lineByte.Length == 8)
            {
                ulong line = BitConverter.ToUInt64(lineByte, 0);
                secret = Crypt.DHSecret(lineByte, clientkey);
                byte[] hmac = Crypt.HMAC64(challenge, secret);
                string base64key = Crypt.Base64Encode(hmac) + '\n';
                byte[] message = Encoding.UTF8.GetBytes(base64key);
                WriteMessage(message);

                this.state = Enum_Login_Auth_State.SEND_LOGIN;
            }
        }

        void OnLogin()
        {
            string token = EncodeToken();
            byte[] etoken = Crypt.DesEncode(secret, Encoding.UTF8.GetBytes (token) );
            string base64token = Crypt.Base64Encode(etoken) + '\n';

            byte[] message = Encoding.UTF8.GetBytes(base64token);
            WriteMessage(message);

            this.state = Enum_Login_Auth_State.LOGIN_RESULT;
        }

        void OnLoginResult(string socketstr)
        {
            int code = int.Parse(socketstr.Substring(0, 3));
            byte[] subidByte = Crypt.Base64Decode(socketstr.Substring(4));
            string resultString = Encoding.UTF8.GetString(subidByte);
            ByteStream buffer = new ByteStream(UTF8Encoding.UTF8.GetBytes(200 + " " + resultString));
            if (code != 200)
            {
                Debug.LogError( "Login Result" + resultString);
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

            this.state = Enum_Login_Auth_State.LOGIN_FINISHED;
            this.cb(code);
        }

        void WriteMessage(byte[] message)
        {
            if (!Connected())
            {
                Close();
                return;
            }
            MemoryStream ms = null;
            using (ms = new MemoryStream())
            {
                ms.Position = 0;
                BinaryWriter writer = new BinaryWriter(ms);
                writer.Write(message);
                writer.Flush();
                if (client != null && client.Connected)
                {
                    byte[] payload = ms.ToArray();
                    Debug.Log("密匙写数据：" + payload.Length);
                    out_stream.BeginWrite(payload, 0, payload.Length, new AsyncCallback(OnWrite), null);
                }
                else
                {
                    Debug.LogError("---------client. is Closed-----");
                }
            }
        }

        protected void OnWrite(IAsyncResult r)
        {
            if (connected == false)
            {
                return;
            }

            try
            {
                out_stream.EndWrite(r);
            }
            catch (Exception ex)
            {
                Debug.LogError("OnWrite--->>>" + ex.Message);
            }
        }


        void OnDisconnected(string desc, string msg)
        {
            Close();   //关掉客户端链接
            if (this.state != Enum_Login_Auth_State.LOGIN_FINISHED)
            {
                Debug.LogError("链接关闭,但loginState != LoginStateEnum.LOGIN_FINISHED,loginState:" + this.state);
               AuthFailed();
            }
            Debug.Log("LoginAuth: Connection but closed by the server:>" + msg + " Desc:>" + desc);
        }

        void AuthFailed()
        {
            Debug.LogError("++++++++AuthFailed++++++++");
        }

        string EncodeToken()
        {
            return string.Format("{0}@{1}@{2}@{3}@{4}", Crypt.Base64Decode(this.auth_package.openId), 
                        Crypt.Base64Decode(this.auth_package.sdk),
                       Crypt.Base64Decode(this.auth_package.serverId),
                       Crypt.Base64Decode(this.auth_package.pf),
                       Crypt.Base64Decode(this.auth_package.userData));
        }

    }


}