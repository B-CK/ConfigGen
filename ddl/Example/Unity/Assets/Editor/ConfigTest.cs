using Example;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

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

    [MenuItem("Data/Gen XmlData", false, 2)]
    static void GenXmlData()
    {
        var config = new ConfigComponent();
        config.Load();
        
    }
}
