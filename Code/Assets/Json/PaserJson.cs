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
//using Json.Test;
using Lson.AllType;
using Lson.Card;
using Sirenix.Serialization;
using Newtonsoft.Json.Linq;

public class PaserJson : SerializedMonoBehaviour
{
    public string RelPath = @"\Json\Nsj.json";
    public TextAsset asset;
    //public Data data;
    //[OdinSerialize]
    //public AllClass datac;

    [Button(ButtonSizes.Medium)]
    void ReadJson()
    {
        string txt = File.ReadAllText(Application.dataPath + RelPath);
        //var d = JsonConvert.DeserializeObject<Dictionary<string, object>>(txt, new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.Auto
        //});
        ////datac = JsonConvert.DeserializeObject<AllClass>(txt, new JsonSerializerSettings
        ////{
        ////    TypeNameHandling = TypeNameHandling.Auto
        ////});
        //JArray array = d["VarListBase"] as JArray;
        //for (int i = 0; i < array.Count; i++)
        //{
        //    Debug.Log(array[i]);
        //}
        JObject js = JObject.Parse(txt);

        Debug.Log("");
    }
    [Button(ButtonSizes.Medium)]
    void WriteJson()
    {
        //string d = JsonConvert.SerializeObject(datac, Formatting.Indented, new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.Auto
        //});
        //File.WriteAllText(Application.dataPath + RelPath, d);
        //AssetDatabase.Refresh();
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
