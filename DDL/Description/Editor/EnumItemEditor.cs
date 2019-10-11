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
    public partial class EnumItemEditor : MemberEditor
    {
        public static EnumItemEditor Create(EnumEditorDock dock, EnumItemWrap wrap)
        {
            EnumItemEditor editor = PoolManager.Ins.Pop<EnumItemEditor>();
            if (editor == null) editor = new EnumItemEditor();
            editor.Init(dock, wrap);
            return editor;
        }
        public static EnumItemEditor Create(EnumEditorDock dock, string name)
        {
            EnumItemEditor editor = PoolManager.Ins.Pop<EnumItemEditor>();
            if (editor == null) editor = new EnumItemEditor();
            EnumItemWrap wrap = PoolManager.Ins.Pop<EnumItemWrap>();
            if (wrap == null) wrap = EnumItemWrap.Create(name, dock.GetWrap<EnumWrap>());
            editor.Init(dock, wrap);
            return editor;
        }
        public static implicit operator EnumItemWrap(EnumItemEditor editor)
        {
            return editor._wrap as EnumItemWrap;
        }

        public override string DisplayName
        {
            get
            {
                string name = _nameTextBox.Text ?? "Name?";
                string display = _aliasTextBox.Text.IsEmpty() ?
                    Util.Format("{0} = {1}", name, _defaultValue.Value)
                    : Util.Format("{0}({1}) = {2}", name, _aliasTextBox.Text, _defaultValue.Value);
                return display;
            }
        }
        public EnumItemEditor()
        {
            InitializeComponent();
        }

        protected override void OnInit()
        {
            base.OnInit();

            var item = _wrap as EnumItemWrap;
            Location = Point.Empty;
            var panel = GetDock<EnumEditorDock>().MemberSplitContainer.Panel2;
            Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            Size = new Size(panel.Width - 95, panel.Height);
            _isNew = true;


            _nameTextBox.Text = item.Name;
            _defaultValue.Maximum = int.MaxValue;
            _defaultValue.Minimum = int.MinValue;
            _defaultValue.Value = item.Value;
            _aliasTextBox.Text = item.Alias;
            _groupComboBox.Items.AddRange(Util.Groups);
            _groupComboBox.Text = item.Group.IsEmpty() ? Util.Groups[0] : item.Group;
            _desTextBox.Text = item.Desc;
        }
        public override void Save()
        {
            base.Save();
            var item = _wrap as EnumItemWrap;
            if (item.Name != _nameTextBox.Text)
                GetDock<EnumEditorDock>().GetWrap<EnumWrap>().RemoveItem(item);
            item.Name = _nameTextBox.Text;
            item.Value = (int)_defaultValue.Value;
            item.Alias = _aliasTextBox.Text;
            item.Group = _groupComboBox.Text;
            item.Desc = _desTextBox.Text;
        }
        public override void Hide()
        {
            base.Hide();
            GetDock<EnumEditorDock>().MemberSplitContainer.Panel2.Controls.Remove(this);
        }
        public override void Show()
        {
            base.Show();
            GetDock<EnumEditorDock>().MemberSplitContainer.Panel2.Controls.Add(this);
        }

        private void OnItemTextChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            GetDock<EnumEditorDock>().OnValueChange();
        }
        private void OnItemValueChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            GetDock<EnumEditorDock>().OnValueChange();
            GetDock<EnumEditorDock>().RefreshMember(this);
        }
        private void OnItemNameChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            var dock = GetDock<EnumEditorDock>();
            dock.OnValueChange();
            var nameBox = sender as TextBox;
            if (!Util.CheckName(_wrap.Name))
                nameBox.Text = _wrap.Name;
            else if (dock.ContainMember(nameBox.Text))
            {
                Util.MsgWarning("枚举{0}中重复定义字段{1}!", dock.GetWrap<EnumWrap>().Name, nameBox.Text);
                nameBox.Text = _wrap.Name;
            }
            else
            {
                string oldName = Name;
                Name = nameBox.Text;
                dock.RefreshMember(this, oldName);
            }
        }
        private void OnItemAliasChanged(object sender, EventArgs e)
        {
            if (!_isInit) return;
            var dock = GetDock<EnumEditorDock>();
            dock.RefreshMember(this);
            dock.OnValueChange();
        }
    }
}
