using System.Collections;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using System;
using Json.Test;

public class PaserJson : SerializedMonoBehaviour
{
    public string RelPath = @"\Json\Nsj.json";
    public TextAsset asset;
    public Data data;

    //public List<JsonObject> lstClass;

    [Button(ButtonSizes.Medium)]
    void ReadJson()
    {
        string txt = File.ReadAllText(Application.dataPath + RelPath);
        data = JsonConvert.DeserializeObject<Data>(txt, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(txt, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        Debug.Log("");
    }
    [Button(ButtonSizes.Medium)]
    void WriteJson()
    {
        string d = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(Application.dataPath + RelPath, d);
        AssetDatabase.Refresh();
    }
}
public enum EnumType
{
    C_C = 1,
    Coo,
    DFC,
}

public class Data
{
    [EnumToggleButtons]
    public EnumType enumType;

    public int _int;
    public long _long;

    [TableList]
    public List<Test> list_test;
}

namespace Json.Test
{
    [System.Serializable]
    public class Test
    {
        public int var1;
        public long var2;
    }
    [System.Serializable]
    public class TestA : Test
    {
        public float var3;
    }
    [System.Serializable]
    public class TestB : Test
    {
        public bool var4;
    }

    [System.Serializable]
    public class TestC : Test
    {
        public string var5;
    }
}

//"list_class": [
//  {
//    "Type": "Test",
//    "Fields": {
//      "var1": 11111,
//      "var2": 1
//    }
//  },
//  {
//    "Type": "Test2",
//    "Fields": {
//      "var1": 22222,
//      "var2": 2
//    }
//  }
//]

//"ID": 1,
//  "Level": 55,
//  "Hp": 77.5,
//  "IsDead": true,
//  "Name": "小兵",

//"list_clas": [
//    {
//      "var1": 1,
//      "var2": 12
//    },
//    {
//      "var1": 2,
//      "var2": 123333,
//      "name": "继承"
//    }
//  ]

//"ClassDes": {
//   "type": "Test",
//   "Fields": {
//     "var1": 11111,
//     "var2": 1
//   }
// }
// "list_int": [
//   "1",
//   "2",
//   "3"
// ],
// "list_clas": [
//   {
//     "type": "Test",
//     "Fields": {
//       "var1": 1,
//       "var2": 12
//     }
//   },
//   {
//     "type": "Test",
//     "Fields": {
//       "var1": 2,
//       "var2": 34
//     }
//   }
// ],


//"dict_int_float": [
//      {"Key": 1, "Value": 1.2},
//      {"Key": 2, "Value": 2.3}
//   ]

public class LsonManager
{
    public static List<int> li = new List<int>();
    //..
    //..


    public static T Deserialize<T>(string path)
    {
        string value = File.ReadAllText(path);
        T data = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        return data;
    }
    public static void Serialize(string path, object data)
    {
        string value = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(path, value);
    }
    public static List<T> Load<T>(string dirPath)
    {
        List<T> list = new List<T>();
        try
        {
            string[] fs = Directory.GetFiles(dirPath);
            foreach (var f in fs)
            {
                list.Add(Deserialize<T>(f));
            }
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("文件夹路径不存在{0}\n{1}", dirPath, e.StackTrace);
        }
        return list;
    }
    public static void LoadAll()
    {
        li = Load<int>("");
    }
    public static void Clear()
    {
        li.Clear();
    }
}