using Desc.Editor;
using Desc.Wrap;
using Desc.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desc
{
    public class WrapManager
    {
        public static WrapManager Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new WrapManager();
                return _ins;
            }
        }
        static WrapManager _ins;

        //所有本地模块信息
        Dictionary<string, ModuleXml> _modules = new Dictionary<string, ModuleXml>();


        /// <summary>
        /// 当前开启模块
        /// </summary>
        public ModuleWrap Current => _current;
        private ModuleWrap _current;

        public void Init()
        {
            _modules.Clear();
            var ms = Directory.GetFiles(Util.ModuleDir, "*.xml");
            for (int i = 0; i < ms.Length; i++)
            {
                string path = ms[i];
                if (!_modules.ContainsKey(path))
                {
                    var module = Util.Deserialize<ModuleXml>(path);
                    _modules.Add(path, module);
                }
            }


            ModuleXml xml = _modules[Util.DefaultModule];
            if (xml == null)
            {
                xml = new ModuleXml()
                {
                    Name = Path.GetFileNameWithoutExtension(Util.DefaultModule),
                    Imports = new List<string>(),
                };
            }
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", "Default");
            var allNsw = NamespaceWrap.Dict;
            foreach (var item in allNsw)
            {
                var ns = item.Value;
                xml.Imports.Add(Util.GetRelativePath(ns.FilePath, Util.ModuleDir));
            }

            if (Util.LastRecord.IsEmpty() || !File.Exists(Util.LastRecord))
                Open(Util.DefaultModule);
            else
                Open(Util.LastRecord);
        }
        public ModuleWrap Open(string path)
        {
            if (_current != null && path == _current.FullName)
                return _current;

            Util.LastRecord = path;
            if (File.Exists(path))
            {
                ModuleWrap wrap = PoolManager.Ins.Pop<ModuleWrap>();
                if (wrap == null)
                    wrap = new ModuleWrap();

                if (_current != null)
                {
                    TrySave();
                    _current.Close();
                }
                _current = wrap;
            }
            else
            {
                Debug.LogWarningFormat("[Module]模块{0}不存在,开启默认模块.", path);
                return _current;
            }

            _current.Init(_modules[path]);
            MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _current.FullName);
            Debug.LogFormat("[Module]打开模板{0}", path);
            GC.Collect();
            return _current;
        }
        public DialogResult TrySave()
        {
            if (EditorDock.CheckOpenDock() || _current.NeedSaveNum != 0)
            {
                var result = MessageBox.Show("当前模块未保存,是否保存?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel://取消操作,对数据不做任何处理
                        return DialogResult.Cancel;
                    case DialogResult.Yes://保存所有修改,且关闭模块
                        EditorDock.SaveAll();
                        _current.Save();
                        EditorDock.CloseAll();
                        return DialogResult.Yes;
                    case DialogResult.No://放弃所有修改,且关闭模块
                        EditorDock.CloseAll();
                        return DialogResult.No;
                }
            }
            else
            {
                SilientSave();
            }
            return DialogResult.None;
        }
        public void SilientSave(bool saveNamespace = false)
        {
            _current.Save(saveNamespace);
            EditorDock.CloseAll();
        }
        public void AddModuleXml(string path, ModuleXml xml)
        {
            _modules.Add(path, xml);
        }
        public void RemoveModuleXml(string path)
        {
            if (_modules.ContainsKey(path))
                _modules.Remove(path);
        }
        public ModuleXml GetModuleXml(string path)
        {
            if (_modules.ContainsKey(path))
                return _modules[path];
            return null;
        }

        public void Close()
        {
            //关闭当前模块:重置监听事件
            //#TODO
        }
    }
}
