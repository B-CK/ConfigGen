using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desc
{
    public enum LogType
    {
        Info = 0,
        Warn,
        Error,
    }
    public class Log
    {
        private int _line;
        private LogType _type;
        private string _msg;
        private ListViewItem _item;
        private string _dateTime;
        public Log(int line, LogType type, string msg)
        {
            _item = new ListViewItem();
            _item.Tag = this;
            Reset(line, type, msg);
        }
        public void Reset(int line, LogType type, string msg)
        {
            _line = line;
            _type = type;
            _msg = msg;
        }

        public int Line { get { return _line; } }
        public LogType MsgType { get { return _type; } }
        public string TimeMsg
        {
            get
            {

                if (_dateTime.IsEmpty())
                    _dateTime = DateTime.Now.ToString("MM-dd HH:mm:ss");
                return Util.Format("[{0}]{1}", _dateTime, _msg);
            }
        }
        public static implicit operator ListViewItem(Log log)
        {
            var item = log._item;
            switch (log._type)
            {
                case LogType.Info:
                    item.ForeColor = Color.LightGray;
                    break;
                case LogType.Warn:
                    item.ForeColor = Color.Yellow;
                    break;
                case LogType.Error:
                    item.ForeColor = Color.Crimson;
                    break;
                    //case LogType.None:
                    //    return item;
            }
            item.Text = log.TimeMsg;
            return item;
        }
    }
}
