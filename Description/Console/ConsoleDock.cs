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
                _ins.Show(MainWindow.Ins._dock, DockState.DockBottomAutoHide);
            }
            else
            {
                _ins.Show();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            stream.Close();
            stream.Dispose();
            stream = null;
            writer = null;
            _ins = null;

            base.OnClosed(e);
        }
        private ConsoleDock()
        {
            InitializeComponent();

            _checks = new CheckBox[] { _logCheckBox, _logWarnCheckBox, _logErrorCheckBox };
            ResetLogCount();
            _logListView.ListViewItemSorter = new ListViewItemComparer();

            string[] lines = File.ReadAllLines(Util.LogErrorFile);
            if (lines.Length >= 1000)
            {
                string[] cut = new string[500];
                Array.Copy(lines, 500, cut, 0, 500);
                File.WriteAllLines(Util.LogErrorFile, cut);
            }
            stream = File.Open(Util.LogErrorFile, FileMode.Append, FileAccess.Write);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
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


        Stream stream;
        StreamWriter writer;

        StringBuilder _builder = new StringBuilder();
        Queue<Log> _cache = new Queue<Log>();
        List<Log> _logs = new List<Log>();
        CheckBox[] _checks;
        bool _isEnter = false;

        public void Log(object msg) { Log(msg.ToString()); }
        public void LogWarning(object msg) { LogWarning(msg.ToString()); }
        public void LogError(object msg) { LogError(msg.ToString()); }
        public void Log(string msg)
        {
            SendMessage(LogType.Info, msg);
        }
        public void LogWarning(string msg)
        {
            SendMessage(LogType.Warn, msg);
        }
        public void LogError(string msg)
        {
            SendMessage(LogType.Error, msg);
        }
        public void LogFormat(string format, params object[] logString)
        {
            Log(Util.Format(format, logString));
        }
        public void LogWarningFormat(string format, params object[] warningString)
        {
            LogWarning(Util.Format(format, warningString));
        }
        public void LogErrorFormat(string format, params object[] errorString)
        {
            LogError(Util.Format(format, errorString));
        }



        private void SendMessage(LogType type, string msg = "")
        {
            int line = _logs.Count;
            Log log = null;
            if (_cache.Count == 0)
            {
                log = new Log(line, type, msg);
            }
            else
            {
                log = _cache.Dequeue();
                log.Reset(line, type, msg);
            }

            _builder.Clear();
            _logs.Add(log);
            CheckBox check = _checks[(int)log.MsgType];
            int num = 0;
            if (int.TryParse(check.Text, out num))
            {
                if (num >= 99)
                    check.Text = _builder.AppendFormat("{0}+", num).ToString();
                else
                    check.Text = _builder.AppendFormat("{0}", num + 1).ToString();
            }
            if (check.Checked)
                _logListView.Items.Add(log);
            _logListView.Refresh();
            if (writer != null)
                writer.WriteLine(log.TimeMsg);
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
            for (int i = 0; i < _logs.Count; i++)
            {
                Log log = _logs[i];
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
            _logListView.Items.Clear();
            for (int i = 0; i < _logs.Count; i++)
                _cache.Enqueue(_logs[i]);
            _logs.Clear();
            ResetLogCount();
        }
        private void CopyLogs(object sender, EventArgs e)
        {
            _builder.Clear();
            var items = _logListView.SelectedItems;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Selected)
                    _builder.AppendLine(items[i].Text);
            }
            string copy = _builder.ToString();
            if (!copy.IsEmpty())
                Clipboard.SetText(_builder.ToString());
        }
        //-------------测试
        private void ConsoleDock_DoubleClick(object sender, EventArgs e)
        {
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
