using Description.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    //public interface MemberEditor
    //{
    //    /// <summary>
    //    /// 数据唯一名称
    //    /// </summary>
    //    string ID { get; }
    //    /// <summary>
    //    /// 用于ListBox显示名称
    //    /// </summary>
    //    string DisplayName { get; }
    //    /// <summary>
    //    /// true:新建;false:移除
    //    /// </summary>
    //    bool IsNew { get; set; }
    //    void Show();
    //    void Hide();
    //    void Save();
    //    void Clear();
    //}

    public partial class EditorDock : DockContent
    {
        static Dictionary<string, EditorDock> _open = new Dictionary<string, EditorDock>();
        /// <summary>
        /// 关闭界面且按需保存
        /// </summary>
        public static void ClearAll()
        {
            var values = new List<EditorDock>(_open.Values);
            for (int i = 0; i < values.Count; i++)
                values[i].ScriptDoClose();
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
        /// <summary>
        /// 取消且关闭
        /// </summary>
        public static void CancleAll()
        {
            var values = new List<EditorDock>(_open.Values);
            for (int i = 0; i < values.Count; i++)
            {
                values[i]._isDirty = false;
                values[i].ScriptDoClose();
            }
            _open.Clear();
        }
        private static void AddOpen(string name, EditorDock dock)
        {
            if (!_open.ContainsKey(name))
                _open.Add(name, dock);
        }
        private static void RemoveOpen(string name)
        {
            _open.Remove(name);
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
        public static void CloseDock(string fullName)
        {
            if (_open.ContainsKey(fullName))
                _open[fullName].Close();
        }
        /// <summary>
        /// Dock 唯一ID
        /// </summary>
        public string ID { get { return _wrap != null ? _wrap.FullName : "?"; } }
        public Action<BaseWrap> OnWrapPropertiesModified;

        public bool IsInit { get { return _isInit; } }

        protected bool _isInit = false;
        protected bool _isDirty = false;
        protected bool _isSilent = false;

        protected BaseWrap _wrap;
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
            NamespaceWrap.HasModifyNamespace = true;
        }

        /// <summary>
        /// 初始化完毕,必须设置
        /// </summary>
        protected virtual void OnInit(BaseWrap wrap)
        {
            _nameId = 0;
            _isDirty = false;
            _isSilent = false;
            _wrap = wrap;
            _memberDict.Clear();
            if (!IsContain(ID))
                AddOpen(ID, this);
        }
        protected virtual void OnSave() { }
        protected virtual void ValidateData() { }
        protected virtual void Clear()
        {
            foreach (var item in _memberDict)
                item.Value.Clear();
        }
        protected virtual void ScriptDoClose()
        {
            _isSilent = true;
            Close();
        }
        protected virtual void UpdateTreeNode(string oldName)
        {
            var typeWrap = _wrap as TypeWrap;
            string oldFullName = Util.Format("{0}.{1}", typeWrap.Namespace.FullName, oldName);
            FindNamespaceDock.Ins.UpdateNode(oldFullName, typeWrap);
            RemoveOpen(oldFullName);
            AddOpen(typeWrap.FullName, this);
        }
        /// <summary>
        /// 必须在NamespaceWrap重写制定类型Add/Remove("TypeName")函数
        /// </summary>
        /// <typeparam name="T">TypeWrap子类型</typeparam>
        protected virtual void SetNamespace<T>(string src, string dst) where T : TypeWrap
        {
            T wrap = _wrap as T;
            if (src != dst)
            {
                //界面层,修改显示状态
                FindNamespaceDock.Ins.SwapNamespace(wrap, src, dst);
                string dstName = wrap.FullName.Replace(src, dst);
                RemoveOpen(wrap.FullName);
                AddOpen(dstName, this);
                var dstNsw = NamespaceWrap.GetNamespace(dst);
                if (OnWrapPropertiesModified != null)
                {
                    OnWrapPropertiesModified(wrap.Namespace);
                    OnWrapPropertiesModified(dstNsw);
                }

                //数据层,命名空间数据更新
                string method = typeof(T).Name;
                Type type = typeof(NamespaceWrap);
                var add = type.GetMethod(Util.Format("Add{0}", method));
                var remove = type.GetMethod(Util.Format("Remove{0}", method));
                add.Invoke(dstNsw, new object[] { wrap, true });
                remove.Invoke(wrap.Namespace, new object[] { wrap, true });
                wrap.Namespace = dstNsw;
            }
        }
        protected virtual void AddMember(MemberEditor mem)
        {
            if (_memberDict.ContainsKey(mem.ID))
                Util.MsgWarning("重复定义字段{0}", mem.ID);
            else
                _memberDict.Add(mem.ID, mem);
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


        protected EditorDock()
        {
            InitializeComponent();
        }
        protected void Init(BaseWrap wrap)
        {
            _isInit = false;
            OnInit(wrap);
            _isInit = true;
        }
        private void Save()
        {
            OnSave();
            Text = _wrap.Name;
            ValidateData();
            if (OnWrapPropertiesModified != null)
                OnWrapPropertiesModified(_wrap);
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
                if (_open.ContainsKey(ID))
                    _open.Remove(ID);
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
