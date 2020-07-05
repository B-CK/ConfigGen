using Tool;
using Wrap;
using System;
using DataSet;
using System.Collections.Generic;


namespace Import
{
    /// <summary>
    /// Excel 行列读取器
    /// 注:遇到##符号则忽略当前行
    /// </summary>
    public class ImportExcel : Import
    {
        private List<List<object>> _lines;
        private int _ri;//行
        private int _ci;//列
        private string _path;

        private static readonly Dictionary<string, string> BOOLS = new Dictionary<string, string> { { "真", "true" }, { "假", "false" } };

        public ImportExcel(string path)
        {
            _path = path;
            _lines = new List<List<object>>();
            _ri = 0;
            _ci = -1;

            var tables = Util.ReadXlsxFile(path).Tables;
            for (int i = 0; i < tables.Count; i++)
            {
                var rows = tables[i].Rows;
                for (int j = 0; j < rows.Count; j++)
                {
                    _lines.Add(new List<object>(rows[j].ItemArray));
                }
            }
        }
        private bool IsSectionEnd()
        {
            while (_ri < _lines.Count)
            {
                ++_ci;
                List<object> row = _lines[_ri];
                if (_ci >= row.Count)
                {
                    ++_ri;
                    _ci = -1;
                }
                else
                {
                    string column = row[_ci].ToString();
                    if (column.StartsWith(Setting.RowEndFlag))
                    {
                        ++_ri;
                        _ci = -1;
                    }
                    else if (!column.IsEmpty())
                    {
                        if (column.StartsWith(Setting.SetEndFlag))
                        {
                            return true;
                        }

                        --_ci;
                        return false;
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// 将Excel中的列编号转为列名称（第1列为A，第28列为AB）
        /// </summary>
        private string GetColumnName(int columnNumber)
        {
            string result = string.Empty;
            int temp = columnNumber;
            int quotient;
            int remainder;
            do
            {
                quotient = temp / 26;
                remainder = temp % 26;
                if (remainder == 0)
                {
                    remainder = 26;
                    --quotient;
                }

                result = (char)(remainder - 1 + 'A') + result;
                temp = quotient;
            }
            while (quotient > 0);

            return result;
        }
        public override void Error(string msg)
        {
            string error = string.Format("错误:{0} \n位置:{1}[{2}{3}]", msg, _path, GetColumnName(_ci + 1), _ri + 1);
            throw new Exception(error);
        }
        private string GetNext()
        {
            while (_ri < _lines.Count)
            {
                ++_ci;
                List<object> row = _lines[_ri];
                if (_ci >= row.Count)
                {
                    ++_ri;
                    _ci = -1;
                }
                else
                {
                    string column = row[_ci].ToString();
                    if (column.StartsWith(Setting.RowEndFlag))
                    {
                        ++_ri;
                        _ci = -1;
                    }
                    else if (!column.IsEmpty())
                    {
                        return column;
                    }
                }
            }
            return null;
        }
        private string GetNextAndCheckNotEmpty()
        {
            string value = GetNext();
            if (value.IsEmpty())
                Error("数据无法正常读取(漏填,集合读一半等)");
            return value;
        }
        public override bool GetBool()
        {
            string v = GetNextAndCheckNotEmpty();
            if (BOOLS.ContainsKey(v))
                v = BOOLS[v];
            if (v.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                return true;
            else if (v.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                return false;
            else
            {
                Error(v + "非bool类型");
                return false;
            }
        }
        public override int GetInt()
        {
            string v = GetNextAndCheckNotEmpty();
            int r;
            if (!int.TryParse(v, out r))
            {
                Error(v + "非int类型");
                return -1;
            }
            return r;
        }
        public override long GetLong()
        {
            string v = GetNextAndCheckNotEmpty();
            long r;
            if (!long.TryParse(v, out r))
            {
                Error(v + "非long类型");
                return -1L;
            }
            return r;
        }
        public override float GetFloat()
        {
            string v = GetNextAndCheckNotEmpty();
            float r;
            if (!float.TryParse(v, out r))
            {
                Error(v + "非float类型");
                return 0f;
            }
            return r;
        }
        public override string GetString()
        {
            return GetNextAndCheckNotEmpty();
        }
        public override string GetEnum()
        {
            return GetNextAndCheckNotEmpty();
        }
        public override void GetClass(FClass data, ClassWrap info)
        {
            if (!info.Inherit.IsEmpty())
            {
                ClassWrap parent = ClassWrap.Get(info.Inherit);
                GetClass(data, parent);
            }

            var fields = info.Fields;
            ConfigWrap cfg = ConfigWrap.Get(info.FullType);
            for (int i = 0; i < fields.Count; i++)
            {
                var d = DataSet.Data.Create(data, fields[i], this);
                data.Values.Add(d);
                if (data.Host == null && fields[i] == cfg.Index)
                    FList.AddIndex(cfg.Index, d);
            }
        }
        public override void GetList(FList data, FieldWrap define)
        {
            FieldWrap item = define.GetItemDefine();
            while (!IsSectionEnd())
            {
                var d = DataSet.Data.Create(data.Host, item, this);
                if (data.Host == null)
                    Program.RecordLastData(d);
                data.Values.Add(d);
            }
        }
        public override void GetDict(FDict data, FieldWrap define)
        {
            FieldWrap key = define.GetKeyDefine();
            FieldWrap value = define.GetValueDefine();
            while (!IsSectionEnd())
            {
                var dk = DataSet.Data.Create(data.Host, key, this);
                var dv = DataSet.Data.Create(data.Host, value, this);
                data.Values.Add(dk, dv);
            }
        }
    }
}
