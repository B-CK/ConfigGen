using Description.Wrap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description
{
    interface IUserOperation
    {
        void Save();
        void Close();
    }

    public partial class TypeEditorDock : DockContent
    {
        public static TypeEditorDock LastDock { get { return _lastDock; } }
        static TypeEditorDock _lastDock;//Ctrl+S保存类型数据
        static Dictionary<string, TypeEditorDock> _open = new Dictionary<string, TypeEditorDock>();
        public static TypeEditorDock Create(ClassWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<TypeEditorDock>();
            if (dock == null) dock = new TypeEditorDock();
            dock.Text = wrap.Name;
            dock._typeGroupBox.Text = "类型[Class]";
            dock._wrap = wrap;
            var info = ClassInfoDock.Create(wrap);
            info.Parent = dock._typeGroupBox;
            if (!_open.ContainsKey(wrap.FullName))
                _open.Add(wrap.FullName, dock);
            return dock;
        }
        public static TypeEditorDock Create(EnumWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<TypeEditorDock>();
            if (dock == null) dock = new TypeEditorDock();
            dock.Text = wrap.Name;
            dock._typeGroupBox.Text = "类型[Enum]";
            dock._wrap = wrap;
            if (!_open.ContainsKey(wrap.FullName))
                _open.Add(wrap.FullName, dock);
            return dock;
        }
        public static void Clear()
        {
            _open.Clear();
        }


        private BaseWrap _wrap;
        public TypeEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dock, DockState.Document);
        }       
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //for (int i = 0; i < Controls.Count; i++)
            //{
            //    var ctrl = Controls[i] as IUserOperation;
            //    ctrl.Close();
            //}
            PoolManager.Ins.Push(this);
            _open.Remove(_wrap.FullName);
        }
    }
}
