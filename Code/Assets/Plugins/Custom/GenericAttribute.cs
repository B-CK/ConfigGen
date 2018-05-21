using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using System.Linq;

namespace Sirenix.OdinInspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GenericAttribute : Attribute
    {
        public Type[] types;
        public GenericAttribute(Type[] types)
        {
            this.types = types;
        }
    }
}

//[OdinDrawer]
//public class GenericAttributeDrawer : OdinAttributeDrawer<GenericAttribute, Type[]>
//{
//    protected override void DrawPropertyLayout(InspectorProperty property, GenericAttribute attribute, GUIContent label)
//    {
//        // Check all values for null, and if any are null, create an instance
//        // Only do this in repaint; as a rule, only change reference type values in repaint
//        for (int i = 0; i < property.Children.Count; i++)
//        {
//            property.Children[i].Draw();
//        }

//        // Call the next drawer in line, and let the lists be drawn normally
//        this.CallNextDrawer(property, null);
//    }
//}
