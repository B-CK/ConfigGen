using Desc.Wrap;
using Desc.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desc
{
    public class ModuleMgr
    {
        public static ModuleMgr Ins
        {
            get
            {
                if(_ins == null)
                    _ins = new ModuleMgr();
                return _ins;
            }
        }
        static ModuleMgr _ins;

        //所有本地模块信息
        Dictionary<string, ModuleXml> _modules = new Dictionary<string, ModuleXml>();

        /// 所有信息
        public Dictionary<string, NamespaceWrap> Namespaces;
        public Dictionary<string, ClassWrap> Classes;
        public Dictionary<string, EnumWrap> Enums;

        /// <summary>
        /// 当前开启模块
        /// </summary>
        public ModuleWrap Current;

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

            //ModuleXml xml = _modules[Util.DefaultModule];
            //_default = new ModuleWrap();
            //_default.Init(xml);
            //MainWindow.Ins.Text = Util.Format("结构描述 - {0}", _default.FullName);
            //var allNsw = NamespaceWrap.Dict;
            //foreach (var item in allNsw)
            //    _default.AddImport(item.Value, true);
            //_default.Save(false);
            //ResetAllState();

            if (Util.LastRecord.IsEmpty() || !File.Exists(Util.LastRecord))
                Open(Util.DefaultModule);
            else
                Open(Util.LastRecord);
        }
        public ModuleWrap Open(string path)
        {
            return null;
        }
        public void Close()
        {
            //关闭当前模块:重置监听事件
            //#TODO
        }
    }
}
