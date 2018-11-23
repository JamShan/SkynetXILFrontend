using System;
using System.Collections.Generic;
using Newtonsoft.Json;



namespace XIL.AI.Behavior3Sharp
{
    public class Behavior3NodeCfg
    {
        public string id;
        public string name;
        public string title;
        public string description;
        public List<string> children = new List<string>();
        public string child = "";
        public Dictionary<string, object> parameters = new Dictionary<string, object>();
        public Dictionary<string, object> properties = new Dictionary<string, object>();

        public T GetValue<T>(string key, T defaultValue)
        {
            if (this.properties.ContainsKey(key) == false)
            {
                return defaultValue;
            }
            return (T)this.properties[key];
        }

    }

    public class Behavior3TreeCfg
    {
        public string root;
        public string title;
        public string description;
        public Dictionary<string, object> properties = new Dictionary<string, object>();
        public Dictionary<string, Behavior3NodeCfg> nodes = new Dictionary<string, Behavior3NodeCfg>();

        public T GetValue<T>(string key, T defaultValue)
        {
            if (this.properties.ContainsKey(key) == false)
            {
                return defaultValue;
            }
            return (T) this.properties[key];
        }
    }

    public class Behavior3Cfg
    {
       public static Behavior3TreeCfg LoadBehavior3TreeCfg(string path)
        {
            // TODO:
            return new Behavior3TreeCfg();
        }
    }
}
