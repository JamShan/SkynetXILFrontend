using System;
//using System.Reflection;


namespace XIL.AI.Behavior3Sharp
{
    public class B3Functions
    {
        private static string hex_digits = "0123456789abcdef"; // { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static readonly int m = 36;

        public static string CreateUUID()
        {
            char[] s = new char[m];
            for (int i = 0; i < m; i++)
            {
                s[i] = hex_digits[Random.range(0, hex_digits.Length - 1)];
            }

            s[8] = s[13] = s[18] = s[23] = '-';

            return s.ToString();
        }

        public static BehaviorTree BuildBehavior3TreeFromConfig(string path)
        {
            Behavior3TreeCfg cfg = Behavior3Cfg.LoadBehavior3TreeCfg(path);
            var tree = new BehaviorTree();
            tree.Initialize();
            tree.Load(cfg);
            return tree;
        }

        public static BaseNode CreateBehavior3Instance<T>(string classname)
        {
            return CreateInstance<BaseNode>("Behavior3Sharp", "XIL.AI.Behavior3Sharp", classname);
        }

        private static T CreateInstance<T>(string asmname, string spacename, string classname)
        {
            try
            {
                string fullname = spacename + "." + classname;
                string path = fullname + "," + asmname;
                Type o = Type.GetType(path);
                Object obj = Activator.CreateInstance(o, true);
                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }


    }

}
