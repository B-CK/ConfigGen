using Description;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;

namespace Description
{

    public partial class ConsoleDock : DockContent
    {
        public static ConsoleDock Ins { get { return _ins; } }
        static ConsoleDock _ins;
        public static void Inspect()
        {
            if (_ins == null)
            {
                _ins = new ConsoleDock();
                _ins.Show(MainWindow.Ins._dockPanel, DockState.DockBottomAutoHide);
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
        private ConsoleDock()
        {
            InitializeComponent();

            _checks = new CheckBox[] { _logCheckBox, _logWarnCheckBox, _logErrorCheckBox };
            _logListView.ListViewItemSorter = new ListViewItemComparer();
            ResetLogCount();
            var logs = Debug.GetLogs();
            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                var check = _checks[(int)log.MsgType];
                int num = 0;
                if (int.TryParse(check.Text, out num))
                {
                    if (num >= 99)
                        check.Text = string.Format("{0}+", num).ToString();
                    else
                        check.Text = string.Format("{0}", num + 1).ToString();
                }
                if (check.Checked)
                    _logListView.Items.Add(log);
                _logListView.Refresh();
            }
        }
        class ListViewItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Log a = ((ListViewItem)x).Tag as Log;
                Log b = ((ListViewItem)y).Tag as Log;
                return a.Line - b.Line;
            }
        }


        CheckBox[] _checks;
        bool _isEnter = false;

        public void ShowMessage(Log log)
        {
            if (log.MsgType == LogType.Error)
                Show();

            CheckBox check = _checks[(int)log.MsgType];
            int num = 0;
            if (int.TryParse(check.Text, out num))
            {
                if (num >= 99)
                    check.Text = string.Format("{0}+", num).ToString();
                else
                    check.Text = string.Format("{0}", num + 1).ToString();
            }
            if (check.Checked)
                _logListView.Items.Add(log);
            _logListView.Refresh();
        }

        private void ResetLogCount()
        {
            _logCheckBox.Text = "0";
            _logWarnCheckBox.Text = "0";
            _logErrorCheckBox.Text = "0";
        }
        private void LogCheckStateChanged(object sender, EventArgs e)
        {
            _logListView.BeginUpdate();
            CheckBox check = sender as CheckBox;
            var logs = Debug.GetLogs();
            for (int i = 0; i < logs.Count; i++)
            {
                Log log = logs[i];
                if ((int)log.MsgType != check.TabIndex) continue;

                if (check.Checked)
                {
                    if (log.Line < _logListView.Items.Count)
                        _logListView.Items.Insert(log.Line, log);
                    else
                        _logListView.Items.Add(log);
                }
                else
                    _logListView.Items.Remove(log);
            }
            _logListView.EndUpdate();
        }
        private void ClearLogs(object sender, EventArgs e)
        {
            ResetLogCount();
            _logListView.Items.Clear();
            Debug.Clear();
        }
        private void CopyLogs(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            var items = _logListView.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Selected)
                    builder.AppendLine(items[i].Text);
            }
            string copy = builder.ToString();
            if (!copy.IsEmpty())
                Clipboard.SetText(copy);
        }
        //-------------测试
        private void ConsoleDock_DoubleClick(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start
            //for (int i = 0; i < 111; i++)
            //{
            //SendMessage(LogType.Info, " -  Info|askjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdh");
            //SendMessage(LogType.Warn, " -  Warn|askjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdh");
            //SendMessage(LogType.Error, " -  Error|askjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdhaskjhkjdshkjsdh");
            //}
        }

        private void ConsoleDock_SizeChanged(object sender, EventArgs e)
        {
            int width = _logListView.Width - 30;
            int height = _logListView.TileSize.Height;
            if (width <= 0)
                width = _logListView.TileSize.Width;
            _logListView.TileSize = new Size(width, height);
            _logListView.Refresh();
        }

        /// <summary>
        /// Ctrl+A:全选;Esc:取消全选
        /// </summary>
        private void LogListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isEnter) return;
            if (e.Control && e.KeyCode == Keys.A)
            {
                var items = _logListView.Items;
                for (int i = 0; i < items.Count; i++)
                    items[i].Selected = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                var items = _logListView.Items;
                for (int i = 0; i < items.Count; i++)
                    items[i].Selected = false;
            }
        }

        private void LogListView_Enter(object sender, EventArgs e)
        {
            _isEnter = true;
        }

        private void LogListView_Leave(object sender, EventArgs e)
        {
            _isEnter = false;
        }
    }
}
