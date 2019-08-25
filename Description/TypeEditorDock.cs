using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Description
{
    public partial class TypeEditorDock : DockContent
    {
        public static TypeEditorDock Ins { get { return _ins; } }
        static TypeEditorDock _ins;
        public static void Inspect()
        {
            if (_ins == null)
            {
                _ins = new TypeEditorDock();
                _ins.Show(MainWindow.Ins._dock, DockState.Document);
            }
            else
            {
                _ins.Show();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            _ins = null;
            base.OnClosed(e);
        }
        public TypeEditorDock()
        {
            InitializeComponent();
        }

        public void Create()
        {

        }
    }
}
