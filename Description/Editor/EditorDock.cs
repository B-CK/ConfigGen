using Description.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    public partial class EditorDock : DockContent
    {
        static Dictionary<string, EditorDock> _open = new Dictionary<string, EditorDock>();
        /// <summary>
        /// 关闭界面且按需保存
        /// </summary>
        public static void ClearAll()
        {
            foreach (var dock in _open)
                dock.Value.ScriptDoClose();
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
            foreach (var dock in _open)
            {
                dock.Value._isDirty = false;
                dock.Value.ScriptDoClose();
            }
            _open.Clear();
        }
        private static void AddOpen(EditorDock dock)
        {
            if (!_open.ContainsKey(dock._wrap.FullName))
                _open.Add(dock._wrap.FullName, dock);
        }
        private static void RemoveOpen(EditorDock dock)
        {
            _open.Remove(dock._wrap.FullName);
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
        public string ID { get { return _wrap != null ? _wrap.FullName : "*"; } }
        public Action<BaseWrap> OnWrapPropertiesModified;


        protected bool _isInit = false;
        protected bool _isDirty = false;
        protected bool _isSilent = false;
        protected BaseWrap _currentMember;

        protected BaseWrap _wrap;
        private HashSet<BaseWrap> _memberHash = new HashSet<BaseWrap>();
        private List<BaseWrap> _memberList = new List<BaseWrap>();
        private int _nameId = 0;

        public T GetWrap<T>() where T : BaseWrap { return _wrap as T; }
        public string UnqueName { get { return Util.Format("_{0}", _nameId++); } }
        /// <summary>
        /// 初始化完毕,必须设置
        /// </summary>
        protected virtual void Init(BaseWrap wrap)
        {
            _isInit = false;
            _isDirty = false;
            _isSilent = false;
            _wrap = wrap;
            if (!IsContain(ID))
                AddOpen(this);

            InitMember();
        }
        protected virtual void Save()
        {
            Text = _wrap.Name;
            _isDirty = false;
            if (OnWrapPropertiesModified != null)
                OnWrapPropertiesModified(_wrap);
        }
        protected virtual void Clear() { }
        protected virtual void ScriptDoClose()
        {
            _isSilent = true;
            Close();
        }
        protected virtual void InitMember() { }
        protected virtual void ShowMember(BaseWrap member)
        {
            if (_currentMember != null)
            {
                if (_memberHash.Count != _memberList.Count)
                {
                    Util.MsgWarning("字段{0}命名重复!", _currentMember.Name);
                    return;
                }
                SaveMember(_currentMember);
            }
            _currentMember = member;
        }
        protected virtual void SaveMember(BaseWrap member)
        {

        }
        protected virtual void HideMember(BaseWrap member) { }
        protected virtual void AddMember(BaseWrap member)
        {
            _memberHash.Add(member);
            _memberList.Add(member);
            ShowMember(member);
            OnValueChange();
        }
        protected virtual void RemoveMember(BaseWrap member)
        {
            if (_memberHash.Contains(member))
            {
                _memberHash.Remove(member);
                _memberList.Remove(member);
                OnValueChange();
            }
            HideMember(member);
        }
        protected virtual void RemoveMember(int index)
        {
            BaseWrap member = _memberList[index];
            if (member != null)
                RemoveMember(member);
            else
                Util.MsgError("成员列表数据混乱,无法索引!", member.FullName);
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
                FindNamespaceDock.Ins.SwapNamespace(wrap, src, dst);
                var dstNsw = NamespaceWrap.GetNamespace(dst);
                if (OnWrapPropertiesModified != null)
                {
                    OnWrapPropertiesModified(wrap.Namespace);
                    OnWrapPropertiesModified(dstNsw);
                }

                string method = typeof(T).Name;
                Type type = typeof(NamespaceWrap);
                var add = type.GetMethod(Util.Format("Add{0}", method));
                var remove = type.GetMethod(Util.Format("Remove{0}", method));
                add.Invoke(dstNsw, new object[] { wrap, true });
                remove.Invoke(wrap.Namespace, new object[] { wrap, true });
                wrap.Namespace = dstNsw;
            }
            else
            {
                if (OnWrapPropertiesModified != null)
                    OnWrapPropertiesModified(wrap.Namespace);
                wrap.Namespace.SetDirty();
            }
        }
        protected virtual void FindMember(string name) { }
        protected bool CheckMemeberIndex(int index) { return index < 0 || index >= _memberList.Count; }
        protected EditorDock()
        {
            InitializeComponent();
        }
        protected void OnValueChange()
        {
            if (!_isInit) return;
            Text = "*" + _wrap.Name;
            _isDirty = true;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }

        private void EditorDock_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveOpen(this);
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
