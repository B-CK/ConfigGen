using Description.Wrap;
using System;
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
        public static TypeEditorDock Create(ClassWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<TypeEditorDock>();
            if (dock == null) dock = new TypeEditorDock();
            dock.Text = wrap.FullName;
            dock._typeGroupBox.Text = "类型[Class]";

            var info = ClassInfoDock.Create(wrap);
            info.Parent = dock._typeGroupBox;
            return dock;
        }
        public static void Create(EnumWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<TypeEditorDock>();
            if (dock == null) dock = new TypeEditorDock();
            dock.Text = wrap.FullName;
            dock._typeGroupBox.Text = "类型[Enum]";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            for (int i = 0; i < Controls.Count; i++)
            {
                var ctrl = Controls[i] as IUserOperation;
                ctrl.Close();
            }
            PoolManager.Ins.Push(this);
        }
        public TypeEditorDock()
        {
            InitializeComponent();
            Show(MainWindow.Ins._dock, DockState.Document);
            
        }

        private void Dock_OnMouseDown(object sender, MouseEventArgs e)
        {

        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
    }
}
