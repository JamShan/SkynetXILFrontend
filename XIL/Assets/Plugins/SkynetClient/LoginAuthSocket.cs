
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
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
        GET_CHALLENGE,
        GET_SECRET,
        SEND_LOGIN,
        LOGIN_RESULT,
        LOGIN_FINISHED,
    }

    public delegate void LoginAuthResultFun(int code);


    public class LoginAuthSocket
    {
        private TcpClient client = null;
        private Thread thread = null;

        private string host;
        private int port;
        private int overtime = 5;

        private Enum_Login_Auth_State state = Enum_Login_Auth_State.GET_CHALLENGE;
        private AuthPackage auth_package;
        private LoginAuthResultFun cb = null;


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

            try
            {
                thread = new Thread(new ThreadStart(RecvForData));
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }
       
        private void RecvForData()
        {
            try
            {
                client = new TcpClient(this.host, this.port);
                //Byte[] bytes = new Byte[1024];
                while (true)
                {
                    // Get a stream object for reading 				
                    using (NetworkStream stream = client.GetStream())
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            while(!streamReader.EndOfStream)
                            {
                                string serverMessage = streamReader.ReadLine();
                                OnReceivedMessage(serverMessage);
                            }
                        }
                        //    int length;
                        //// Read incomming stream into byte arrary. 					
                        //while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        //{
                        //    var incommingData = new byte[length];
                        //    Array.Copy(bytes, 0, incommingData, 0, length);
                        //    // Convert byte array to string message. 					
                        //    string serverMessage = Encoding.ASCII.GetString(incommingData);
                        //    OnReceivedMessage(serverMessage);
                        //    Debug.Log("server message received as: " + serverMessage);
                        //}
                    }
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        private void SendMessage(string message)
        {
            Debug.Log("+++++SendMessage++++ state:" + state + " message: " + message);
            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = client.GetStream();
                if (stream.CanWrite)
                {
                    // Convert string message to byte array.        Encoding.UTF8.GetBytes(message);         
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    Debug.Log("Client sent his message - should be received by server");
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        public bool Connected()
        {
            return client != null && client.Client != null && client.Client.Connected;
        }

        public void Close()
        {
            if (client != null)
            {
                if (client.Connected)
                {
                    client.Close();
                }
                if(this.thread.IsAlive)
                {
                    this.thread.Abort();
                }
                client = null;
            }
        }

        void OnReceivedMessage(string socketline)
        {
            Debug.LogError("OnReceivedMessage state:" + this.state);
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

                this.state = Enum_Login_Auth_State.GET_SECRET;
                SendMessage(base64key);
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

                this.state = Enum_Login_Auth_State.SEND_LOGIN;
                SendMessage(base64key);
            }
        }

        void OnLogin()
        {
            Debug.Log("+++++++++OnLogin+++++++++");
            string token = EncodeToken();
            byte[] etoken = Crypt.DesEncode(secret, Encoding.ASCII.GetBytes (token) );
            string base64token = Crypt.Base64Encode(etoken) + '\n';

            this.state = Enum_Login_Auth_State.LOGIN_RESULT;
            SendMessage(base64token);
        }

        void OnLoginResult(string socketstr)
        {
            Debug.Log("+++++++++OnLoginResult+++++++++");
            int code = int.Parse(socketstr.Substring(0, 3));
            byte[] subidByte = Crypt.Base64Decode(socketstr.Substring(4));
            string resultString = Encoding.ASCII.GetString(subidByte);
            ByteStream buffer = new ByteStream(UTF8Encoding.ASCII.GetBytes(200 + " " + resultString));
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

        string EncodeToken()
        {
            return string.Format("{0}@{1}@{2}@{3}@{4}", Crypt.Base64Decode(this.auth_package.openId), 
                        Crypt.Base64Decode(this.auth_package.sdk),
                       Crypt.Base64Decode(this.auth_package.serverId),
                       Crypt.Base64Decode(this.auth_package.pf),
                       Crypt.Base64Decode(this.auth_package.userData));
        }

        public void DumpBytes(byte[] byte_buffer)
        {
            string returnStr = string.Empty;
            for (int i = 0; i < byte_buffer.Length; i++)
            {
                returnStr += byte_buffer[i].ToString("X2");
            }
            Debug.Log(returnStr);
        }


    }


}