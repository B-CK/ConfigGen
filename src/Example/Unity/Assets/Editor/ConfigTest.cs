using Example;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using XmlEditor;

public class ConfigTest
{
    [MenuItem("Data/Read CsvData", false, 1)]
    static void ReadCsvData()
    {
        var config = new ConfigComponent();
        config.Load();
        StringBuilder builder = new StringBuilder();
        foreach (var item in config.BaseTypes)
        {
            var v = item.Value;
            builder.AppendLine($"{v.int_var}");
            builder.AppendLine($"{v.long_var}");
            builder.AppendLine($"{v.float_var}");
            builder.AppendLine($"{v.bool_var}");
            builder.AppendLine(v.string_var);
        }
        Debug.Log(builder.ToString());
    }

    [MenuItem("Data/Write XmlData", false, 2)]
    static void WriteXmlData()
    {
        string dir = $"{Application.dataPath}/../../Excel/Xml/";
        var config = new ConfigComponent();
        config.Load();
        var hash = new HashSet<Type>();
        var ls = new List<Editor.CustomTypes.Character>();
        foreach (var item in config.Characters)
        {
            var c = item.Value;
            if (hash.Contains(c.Custom.GetType())) continue;

            var character = new Editor.CustomTypes.Character();
            character.ID = c.ID;
            switch (c.Custom)
            {
                case Example.CustomTypes.Monster monster:
                    character.Custom = new Editor.CustomTypes.Monster()
                    {
                        Attack = monster.Attack,
                        Level = monster.Level,
                        Name = monster.Name,
                    };
                    break;
                case Example.CustomTypes.NPC npc:
                    character.Custom = new Editor.CustomTypes.NPC()
                    {
                        Alias = npc.Alias,
                        Level = npc.Level,
                        Name = npc.Name,
                    };
                    break;
                case Example.CustomTypes.Partner partner:
                    character.Custom = new Editor.CustomTypes.Partner()
                    {
                        Level = partner.Level,
                        Name = partner.Name,
                        Alias = partner.Alias,
                        Buff = (Editor.CustomTypes.BuffType)partner.Buff,
                    };
                    break;
            }
            hash.Add(c.Custom.GetType());
            ls.Add(character);
            character.SaveAConfig($"{dir}{character.Custom.GetType()}.xml");
        }

        XmlObject.SaveConfig(ls, $"{dir}../charactor.xml");
        Debug.Log("写Xml文件Ok~");
    }
    [MenuItem("Data/Read XmlData", false, 3)]
    static void ReadXmlData()
    {
        string dir = $"{Application.dataPath}/../../Excel/Xml/";
        string[] fs = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
        foreach (var item in fs)
        {
            var c = new Editor.CustomTypes.Character();
            c.LoadAConfig(item);
            OutputXml("Single:", c);
        }

        var ls = new List<Editor.CustomTypes.Character>();
        XmlObject.LoadConfig(ls, $"{dir}../charactor.xml");
        foreach (var item in ls)
        {
            OutputXml("Multi:", item);
        }
        Debug.Log("读Xml文件Ok~");
    }
    static void OutputXml(string pre, Editor.CustomTypes.Character character)
    {
        switch (character.Custom)
        {
            case Editor.CustomTypes.Monster monster:
                Debug.Log($"{pre}\t{character.ID}\t{monster.Name}\t{monster.Level}\t{monster.Attack}");
                break;
            case Editor.CustomTypes.NPC npc:
                Debug.Log($"{pre}\t{character.ID}\t{npc.Name}\t{npc.Level}\t{npc.Alias}");
                break;
            case Editor.CustomTypes.Partner partner:
                Debug.Log($"{pre}\t{character.ID}\t{partner.Name}\t{partner.Level}\t{partner.Alias}\t{partner.Buff}");
                break;
            default:
                Debug.LogError($"类型匹配异常:{character.Custom.GetType()}");
                break;
        }
    }

}
