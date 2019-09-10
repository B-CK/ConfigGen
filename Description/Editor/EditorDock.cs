using Description.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    public partial class EditorDock : DockContent
    {
        public static EditorDock LastDock { get { return _lastDock; } }
        static EditorDock _lastDock;//Ctrl+S保存类型数据
        static Dictionary<string, EditorDock> _open = new Dictionary<string, EditorDock>();
        /// <summary>
        /// 关闭界面且按需保存
        /// </summary>
        public static void ClearAll()
        {
            foreach (var dock in _open)
                dock.Value.DoClose();
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
                dock.Value.DoClose();
            }
            _open.Clear();
        }

        /// <summary>
        /// Dock 唯一ID
        /// </summary>
        public string ID { get { return _wrap != null ? _wrap.FullName : "*"; } }



        private BaseWrap _wrap;
        private HashSet<BaseWrap> _memberHash = new HashSet<BaseWrap>();
        private List<BaseWrap> _memberList = new List<BaseWrap>();
        protected bool _isDirty = false;
        protected bool _isSilent = false;
        private int _nameId = 0;
        protected BaseWrap _currentMember;
        public T GetWrap<T>() where T : BaseWrap { return _wrap as T; }
        public string UnqueName { get { return Util.Format("_{0}", _nameId++); } }
        protected virtual void Init(BaseWrap wrap)
        {
            _isDirty = false;
            _isSilent = false;
            _wrap = wrap;
            if (!_open.ContainsKey(ID))
                _open.Add(ID, this);

            InitMember();
        }
        protected virtual void Save() { _isDirty = false; }
        protected virtual void Clear() { }
        protected virtual void DoClose()
        {
            _isSilent = true;
            Close();
        }
        protected virtual void InitMember() { }
        protected virtual void ShowMember(BaseWrap member)
        {
            _currentMember = member;
        }
        protected virtual void AddMember(BaseWrap member)
        {
            _memberHash.Add(member);
            _memberList.Add(member);
            ShowMember(member);
            Util.MsgError("成员定义", "成员命名{0}重复!", member.FullName);
        }
        protected virtual void RemoveMember(BaseWrap member)
        {
            if (_memberHash.Contains(member))
            {
                _memberHash.Remove(member);
                _memberList.Remove(member);
            }
        }
        protected virtual void RemoveMember(int index)
        {
            BaseWrap member = _memberList[index];
            if (member != null)
                RemoveMember(member);
            else
                Util.MsgError("成员定义", "成员列表数据混乱,无法索引!", member.FullName);
        }
        protected virtual void FindMember(string name) { }
        public EditorDock()
        {
            InitializeComponent();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (_open.ContainsKey(ID))
                _open.Remove(ID);
            if (!_isSilent)
            {
                string content = Util.Format("[{0}]{1}已经修改,是否保存?", _wrap.GetType().Name.Replace("Wrap", ""), _wrap.FullName);
                var result = MessageBox.Show(content, "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
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

            Clear();
            _wrap.Dispose();
            PoolManager.Ins.Push(this);
            base.OnClosed(e);
        }
        protected virtual void OnValueChange()
        {
            Text = "*" + _wrap.Name;
            _isDirty = true;
        }
    }
}
