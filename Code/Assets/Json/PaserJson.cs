using Sirenix.OdinInspector;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
//using Json.Test;
using Lson.AllType;
using Sirenix.Serialization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

public class PaserJson : SerializedMonoBehaviour
{
    public string CsvDir = "";
    public string RelPath = @"\Json\Nsj.json";
    public TextAsset asset;
    [OdinSerialize]
    public LsonAllClass datac;

    public int c = 1;
    Stopwatch a = new Stopwatch();
    Stopwatch b = new Stopwatch();
    void Awake()
    {
        txt = c.ToString();
        Screen.SetResolution(1280, 720, true);
        a.Start();
        for (int i = 0; i < c; i++)
        {
            var l = new Lson.AllType.LInherit1();
        }
        a.Stop();
        UnityEngine.Debug.LogFormat("Normal : {0:N3}", a.ElapsedMilliseconds / 1000f);


        b.Start();
        Type type = Type.GetType("Lson.AllType.LInherit1");
        for (int i = 0; i < c; i++)
        {
            var l = Activator.CreateInstance(type);
        }
        b.Stop();
        UnityEngine.Debug.LogFormat("Reflect : {0:N3}", b.ElapsedMilliseconds / 1000f);
    }



    string txt = "";
    private void OnGUI()
    {
        Rect rect = new Rect(Vector2.zero, new Vector2(300, 30));
        txt = GUI.TextField(rect, txt);
        int r = 0;
        if (int.TryParse(txt, out r))
            c = r;

        rect.position += new Vector2(0, rect.size.y);
        rect.size = new Vector2(300, 100);
        if (GUI.Button(rect, "Run"))
        {
            a.Reset();
            b.Reset();
            Awake();
        }

        rect.position = new Vector2(0, rect.size.y);
        rect.size = new Vector2(300, 30);
        GUI.TextField(rect, string.Format("Normal : {0:N3}", a.ElapsedMilliseconds / 1000f));
        rect.position += new Vector2(0, rect.size.y);
        rect.size = new Vector2(300, 30);
        GUI.TextField(rect, string.Format("Reflect : {0:N3}", b.ElapsedMilliseconds / 1000f));
    }







    [Button(ButtonSizes.Medium)]
    void ReadJson()
    {
        string txt = File.ReadAllText(Application.dataPath + RelPath);
        datac = JsonConvert.DeserializeObject<LsonAllClass>(txt, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
    }
    [Button(ButtonSizes.Medium)]
    void WriteJson()
    {
        string d = JsonConvert.SerializeObject(datac, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        File.WriteAllText(Application.dataPath + RelPath, d);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }


    [Button(ButtonSizes.Medium)]
    void ReadCsv()
    {
        Csv.CfgManager.ConfigDir = CsvDir;
        Csv.CfgManager.LoadAll();
        UnityEngine.Debug.Log("Ok");
    }
    [Button(ButtonSizes.Medium)]
    void OutputCsv()
    {

    }
    [Button(ButtonSizes.Medium)]
    void ClearCsv()
    {
        Csv.CfgManager.Clear();
    }
}

//public enum EnumType
//{
//    C_C = 1,
//    Coo,
//    DFC,
//}

//public class Data
//{
//    [EnumToggleButtons]
//    public EnumType enumType;

//    public int _int;
//    public long _long;

//    [TableList]
//    public List<Test> list_test;
//}

//namespace Json.Test
//{
//    public class Test
//    {
//        public int var1;
//        public long var2;
//    }
//    public class TestA : Test
//    {
//        public float var3;
//    }
//    public class TestB : Test
//    {
//        public bool var4;
//    }

//    public class TestC : Test
//    {
//        public string var5;
//    }
//}
//---------------------------------------------
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
