using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace Xml {
    public class Loader {
        static Dictionary<string, NamespaceXml> AllNs = new Dictionary<string, NamespaceXml> ();
        private static HashSet<string> _groups;
        private static void LoadGroup (string g) {
            _groups = new HashSet<string> (Split (g));
            if (!_groups.Contains (Setting.DefualtGroup))
                _groups.Add (Setting.DefualtGroup);
        }
        private static void LogErrorFormat (string format, params object[] errorString) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine (format, errorString);
        }

        public static object Deserialize (string filePath, Type type) {
            object result = null;
            if (File.Exists (filePath)) {
                using (StreamReader streamReader = new StreamReader (filePath)) {
                    result = new XmlSerializer (type).Deserialize (streamReader);
                }
            }
            return result;
        }
        public static bool IsGroup (string group) {
            return _groups.Contains (group);
        }
        public static void LoadDefine (string modulePath) {
            string path = "无法解析Xml.NamespaceXml";
            try {
                var moduleXml = Deserialize (modulePath, typeof (ModuleXml)) as ModuleXml;
                if (moduleXml.Name.IsEmpty ())
                    throw new Exception ("数据结构导出时必须指定命名空间根节点<Module Name =\"**\">");
                LoadGroup (moduleXml.Groups);
                string moduleDir = Path.GetDirectoryName (Setting.Module);
                List<string> imports = moduleXml.Imports;
                for (int i = 0; i < imports.Count; i++) {
                    path = Path.Combine (moduleDir, imports[i]);
                    var nsx = Deserialize (path, typeof (NamespaceXml)) as NamespaceXml;
                    AllNs.Add (nsx.Name, nsx);
                }
            } catch (Exception e) {
                LogErrorFormat ("路径:{0} 错误:{1}\n{2}", path, e.Message, e.StackTrace);
                return;
            }
        }

    }
}