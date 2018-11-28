using System.Collections.Generic;
using Newtonsoft.Json;

public static class DebugHelper 
{
    public static string DumpJson<T>(this T x)
    {
        return JsonConvert.SerializeObject(x, Formatting.Indented);
    }

    public static string Dump(this object obj, char splitchar = '\n')
    {
        string str = "";
        if (obj == null)
        {
            str = "Object is null" + '\n';
            return str;
        }

        str += "Type: " + obj.GetType().ToString() + splitchar;
        str += "Hash: " + obj.GetHashCode().ToString() + splitchar;
        str += "-------------------------" + splitchar;

        var props = GetProperties(obj);
        foreach (var prop in props)
        {
            str += prop.Key.ToString();
            str += ": ";
            str += prop.Value.ToString();
            str += splitchar;
        }
        return str;
    }

    private static Dictionary<string, string> GetProperties(object obj)
    {
        var props = new Dictionary<string, string>();
        if (obj == null)
            return props;

        var type = obj.GetType();
        foreach (var prop in type.GetProperties())
        {
            var val = prop.GetValue(obj, new object[] { });
            var valStr = val == null ? "" : val.ToString();
            props.Add(prop.Name, valStr);
        }

        return props;
    }
}
