﻿using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

namespace NLog.Unity {
    [CustomEditor(typeof(SOSMaxSocketAppender))]
    public class SOSMaxSocketAppenderEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            SOSMaxSocketAppender client = (SOSMaxSocketAppender)target;

            if (GUILayout.Button("Send to local IP Address")) {
                client.sendToIpAddress = getLocalIPAddress();
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(client);
            }
        }

        string getLocalIPAddress() {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
    }
}