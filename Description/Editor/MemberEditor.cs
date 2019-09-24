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
        /// 数据下标索引
        /// </summary>
        public virtual int Index { get; set; }
        /// <summary>
        /// 用于ListBox显示名称
        /// </summary>
        public virtual string DisplayName => Name;
        /// <summary>
        /// true:新建;false:移除
        /// </summary>
        public virtual bool IsNew
        {
            get => _isNew;
            set => _isNew = value;
        }

        protected bool _isNew;
        protected bool _isInit;
        protected BaseWrap _wrap;
        protected MemberEditor()
        {
            InitializeComponent();
        }
        public void Init(BaseWrap wrap)
        {
            _isInit = false;
            _wrap = wrap;
            Name = wrap.Name;
            OnInit();
            _isInit = true;
        }

        protected virtual void OnInit() { }
        public new virtual void Show() { }
        public new virtual void Hide() { }
        public virtual void Save() { }
        public virtual void Clear()
        {
            Hide();

            _isNew = false;
            PoolManager.Ins.Push(this);
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
