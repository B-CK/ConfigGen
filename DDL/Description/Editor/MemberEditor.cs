using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Description.Wrap;

namespace Description.Editor
{
    public partial class MemberEditor : UserControl
    {
        /// <summary>
        /// 用于ListBox显示名称
        /// </summary>
        public virtual string DisplayName => Name;
        /// <summary>
        /// true:新建;false:移除
        /// </summary>
        protected string ParentName { get { return _dock.ID; } }
        public bool IsDelete => _isDelete;

        protected bool _isDelete;
        protected bool _isInit;
        protected BaseWrap _wrap;
        private EditorDock _dock;
        protected MemberEditor()
        {
            InitializeComponent();
        }
        public void Init(EditorDock dock, BaseWrap wrap)
        {
            _isDelete = false;
            _isInit = false;
            _wrap = wrap;
            _dock = dock;
            Name = wrap.Name;
            OnInit();
            _isInit = true;
        }
        public T GetDock<T>() where T : EditorDock { return _dock as T; }
        protected virtual void OnInit() { }
        public new virtual void Show() { }
        public new virtual void Hide() { }
        public virtual void Save() { }
        public virtual void Clear()
        {
            Hide();
            _isDelete = true;
            PoolManager.Ins.Push(this);
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
