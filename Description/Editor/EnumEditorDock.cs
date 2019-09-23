using Description.Wrap;
using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description.Editor
{
    public partial class EnumEditorDock : EditorDock
    {
        public static EnumEditorDock Create(EnumWrap wrap)
        {
            var dock = PoolManager.Ins.Pop<EnumEditorDock>();
            if (dock == null) dock = new EnumEditorDock();
            dock.Init(wrap);
            dock.Text = wrap.Name;
            dock._typeGroupBox.Text = "枚举[Enum]";
            return dock;
        }

        private EnumEditorDock() 
        {
            InitializeComponent();
            Show(MainWindow.Ins._dockPanel, DockState.Document);
        }

        protected override void OnInit(BaseWrap arg)
        {
            base.OnInit(arg);

            var wrap = GetWrap<EnumWrap>();

            _nameTextBox.Text = wrap.Name;
            _namespaceComboBox.Text = wrap.Namespace.FullName;
            _inhertComboBox.Text = wrap.Inhert;
            _descTextBox.Text = wrap.Desc;
        }
        protected override void OnSave()
        {
            base.OnSave();
            var wrap = GetWrap<EnumWrap>();
            wrap.Name = _nameTextBox.Text;
            wrap.Inhert = _inhertComboBox.Text;
            wrap.Namespace.Name = _namespaceComboBox.Text;
            wrap.Desc = _descTextBox.Text;

            //修改命名空间
            var nsw = NamespaceWrap.GetNamespace(_namespaceComboBox.Text);
            if (nsw.FullName != wrap.Namespace.FullName)
            {
                nsw.AddEnumWrap(wrap);
                wrap.Namespace.RemoveEnumWrap(wrap);
                wrap.Namespace = nsw;
            }
            nsw.SetDirty();
        }
        protected override void Clear()
        {
            base.Clear();
            _nameTextBox.Text = "";
            _namespaceComboBox.Text = "";
            _inhertComboBox.Text = "";
            _descTextBox.Text = "";
        }
        //protected override void InitMember()
        //{
        //    base.InitMember();

        //    var wrap = GetWrap<EnumWrap>();
        //    var items = wrap.Items;
        //    for (int i = 0; i < items.Count; i++)
        //        AddMember(items[i]);
        //}
    }
}
