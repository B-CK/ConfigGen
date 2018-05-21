using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
using System;
using Sirenix.OdinInspector.Editor;

public class SEditor : SearchableEditorWindow
{
    [MenuItem("Tools/Open Custom")]
    static void Open()
    {
        var w = GetWindow<SEditor>("自定义", false);
        w.Show(true);

    }

    public override void OnEnable()
    {
        json = Editor.CreateEditor(new PaserJson());
        PropertyTree<PaserJson> propertyTree = new PropertyTree<PaserJson>(json.serializedObject);
        IEnumerable<InspectorProperty> propertys = propertyTree.EnumerateTree(true);
        foreach (var item in propertys)
        {
            Debug.Log(item.ToString());
        }
    }

    bool IsVisible;
    TB tB = new TB();
    Editor json;

    private void OnGUI()
    {
        SirenixEditorFields.PolymorphicObjectField(tB, Type.GetType("T"), false);
        SirenixEditorGUI.BeginBox();
        {
            SirenixEditorGUI.BeginBoxHeader();
            var content = GUIHelper.TempContent("StringMemberHelper");
            var rect = GUILayoutUtility.GetRect(content, SirenixGUIStyles.Label);
            IsVisible = SirenixEditorGUI.Foldout(rect, IsVisible, content);
            SirenixEditorGUI.EndBoxHeader();

            if (SirenixEditorGUI.BeginFadeGroup(tB, IsVisible))
            {

            }

            SirenixEditorGUI.EndFadeGroup();
        }
        SirenixEditorGUI.EndBox();
    }
}

//
public class T
{
    public string t;
    public bool tboll;
}

public class TA : T
{
    public string a;
}
[System.Serializable]
public class TB : T
{
    [Range(1, 2)]
    public int b;
}
public class TC : T
{
    public bool c;
}