using Desc.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Desc.Editor
{
    // ---- 可优化命名空间错误颜色更新逻辑
    public partial class EditorDock : DockContent
    {
        static Dictionary<string, EditorDock> _open = new Dictionary<string, EditorDock>();
        /// <summary>
        /// 检查是否存在已修改而未保存数据
        /// </summary>
        public static bool CheckOpenDock()
        {
            foreach (var item in _open)
            {
                if (item.Value._isDirty)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 关闭界面且不保存
        /// </summary>
        public static void CloseAll()
        {
            var values = new List<EditorDock>(_open.Values);
            for (int i = 0; i < values.Count; i++)
            {
                values[i]._isDirty = false;
                values[i]._isSilent = true;
                values[i].Close();
            }
            _open.Clear();
        }
        /// <summary>
        /// 直接全部保存,但不关闭
        /// </summary>
        public static bool SaveAll()
        {
            bool result = false;
            foreach (var dock in _open)
            {
                if (dock.Value._isDirty)
                {
                    dock.Value.Save();
                    result = true;
                    break;
                }
            }
            return result;
        }
        private static void AddOpen(string name, EditorDock dock)
        {
            if (!_open.ContainsKey(name))
                _open.Add(name, dock);
        }
        private static void RemoveOpen(string fullName)
        {
            _open.Remove(fullName);
        }
        public static bool IsContain(string fullName)
        {
            return _open.ContainsKey(fullName);
        }
        public static void FocusDock(string fullName)
        {
            _open[fullName].Focus();
        }
        public static void SaveDock(EditorDock dock)
        {
            if (!dock._isDirty) return;
            dock.Save();
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="fullName">窗口名称</param>
        /// <param name="isDirty">数据是否标记为脏</param>
        public static void CloseDock(string fullName, bool isDirty = false)
        {
            if (_open.ContainsKey(fullName))
            {
                var dock = _open[fullName];
                dock._isDirty = isDirty;
                dock.Close();
            }
        }
        /// <summary>
        /// Dock 唯一ID
        /// </summary>
        public string ID { get { return _wrap != null ? _wrap.FullName : "?"; } }
        //public Action<BaseWrap> OnWrapPropertiesModified;

        public bool IsInit { get { return _isInit; } }

        protected bool _isInit;
        protected bool _isDirty;
        protected bool _isSilent;

        protected TypeWrap _wrap;
        /// <summary>
        /// 在多态环境中,不包含父类成员
        /// </summary>
        protected Dictionary<string, MemberEditor> _memberDict = new Dictionary<string, MemberEditor>();
        private int _nameId = 0;

        public T GetWrap<T>() where T : BaseWrap { return _wrap as T; }
        public string UnqueName
        {
            get
            {
                string name = Util.Format("_{0}", _nameId++);
                while (_memberDict.ContainsKey(name))
                    name = Util.Format("_{0}", _nameId++);
                return name;
            }
        }
        public bool ContainMember(string name)
        {
            return _memberDict.ContainsKey(name);
        }
        public void OnValueChange()
        {
            if (!_isInit) return;
            Text = "*" + _wrap.Name;
            _isDirty = true;
        }

        /// <summary>
        /// 初始化完毕,必须设置
        /// </summary>
        protected virtual void OnInit(TypeWrap wrap)
        {
            _nameId = 0;
            _isSilent = false;
            _wrap = wrap;
            _memberDict.Clear();
            if (!IsContain(ID))
                AddOpen(ID, this);
        }
        protected virtual void OnSave() { }
        /// <summary>
        /// 不保存数据时的一些重置操作
        /// </summary>
        protected virtual void OnDiscard() { }
        protected virtual void Clear()
        {
            foreach (var item in _memberDict)
                item.Value.Clear();
        }
        protected virtual void AddMember(MemberEditor mem)
        {
            if (_memberDict.ContainsKey(mem.Name))
                Util.MsgWarning("重复定义字段{0}", mem.Name);
            else
                _memberDict.Add(mem.Name, mem);
        }
        protected virtual void RemoveMember(string name)
        {
            if (_memberDict.ContainsKey(name))
                _memberDict.Remove(name);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }
        /// <summary>
        /// 更新界面Dock名称
        /// </summary>
        protected virtual void UpdateDock(string srcFullName)
        {
            var wrap = _wrap as TypeWrap;
            RemoveOpen(srcFullName);
            AddOpen(wrap.FullName, this);
        }
        protected void SwapNamespace(NamespaceWrap src, NamespaceWrap dst)
        {
            if (src != dst)
            {
                string srcFullName = _wrap.FullName;
                TypeWrap wrap = _wrap as TypeWrap;
                src.RemoveTypeWrap(wrap);
                dst.AddTypeWrap(wrap);

                UpdateDock(srcFullName);
            }
        }

        protected EditorDock()
        {
            InitializeComponent();
        }
        protected void Init(TypeWrap wrap)
        {
            _isInit = false;
            OnInit(wrap);
            _isInit = true;
        }
        private void Save()
        {
            OnSave();
            if (_isDirty)
                _wrap.Namespace.SetDirty();
            //ModuleWrap.Current.Check();
            Text = _wrap.Name;
            _isDirty = false;
        }
        private void EditorDock_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveOpen(ID);
            Clear();
            PoolManager.Ins.Push(this);
        }
        private void EditorDock_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_isDirty) return;
                if (e.CloseReason == CloseReason.UserClosing && !_isSilent)
                {
                    string content = Util.Format("[{0}]{1}已经修改,是否保存?", _wrap.GetType().Name.Replace("Wrap", ""), _wrap.FullName);
                    var result = MessageBox.Show(content, "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                        case DialogResult.Yes:
                            Save();
                            break;
                        case DialogResult.No:
                            OnDiscard();
                            break;
                    }
                }
                else if (_isDirty)
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Util.MsgError("{0}\n{1}\n", ex.Message, ex.StackTrace);
            }
        }
    }
}
