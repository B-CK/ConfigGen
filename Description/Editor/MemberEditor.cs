using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Description.Editor
{
    public partial class MemberEditor : UserControl
    {
        /// <summary>
        /// 数据唯一名称
        /// </summary>
        public virtual string ID => Name;
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
        public MemberEditor()
        {
            InitializeComponent();
        }
        public void Init()
        {
            _isInit = false;
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
