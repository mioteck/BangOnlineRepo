using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

public static class ISerialize
{
    public static byte[] Serialize(object obj)
    {
        if(obj == null)
        {
            return null;
        }
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    public static byte[] Serialize(string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }

    public static object Deserialize(byte[] b)
    {
        if(b == null || b.Length == 0)
        {
            return null;
        }
        MemoryStream ms = new MemoryStream();
        ms.Write(b, 0, b.Length);
        ms.Seek(0, SeekOrigin.Begin);

        return Deserialize(ms);
    }

    public static object Deserialize(Stream stream)
    {
        if(stream == null)
        {
            return null;
        }
        IFormatter formatter = new BinaryFormatter();
        stream.Seek(0, SeekOrigin.Begin);
        object o = formatter.Deserialize(stream);
        return o;
    }

    public static T Deserialize<T>(string path) where T : class
    {
        if(string.IsNullOrEmpty(path))
        {
            return null;
        }
        return JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(path));
    }

    public static string DeserializeString(byte[] b)
    {
        return ResizeString(Encoding.UTF8.GetString(b));
    }

    static string ResizeString(string s)
    {
        int index = s.IndexOf('\0');
        int count = s.Length - index - 1;
        s = s.Remove(index, count);
        return s;
    }
}

