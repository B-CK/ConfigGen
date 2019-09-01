using System.Data;
using System.Linq;
using System.Windows.Forms;
using Description.Wrap;

namespace Description
{
    public partial class EnumInfoDock : UserControl, IUserOperation
    {
        public static void Create(EnumWrap wrap)
        {
            EnumInfoDock dock = PoolManager.Ins.Pop<EnumInfoDock>();
            if (dock == null) dock = new EnumInfoDock(wrap);

            dock._nameTextBox.Text = wrap.Name;
            int index = dock._namespaceComboBox.FindStringExact(wrap.Namespace);
            dock._namespaceComboBox.Select(index, 1);
            dock._desTextBox.Text = wrap.Desc;
        }

        private EnumWrap _wrap;
        public EnumInfoDock(EnumWrap wrap)
        {
            InitializeComponent();
            _wrap = wrap;
        }
        public void Close()
        {
            _wrap.Dispose();
            PoolManager.Ins.Push(this);
        }

        public void Save()
        {
             
        }
    }
}
