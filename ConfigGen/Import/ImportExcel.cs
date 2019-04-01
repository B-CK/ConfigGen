using System;
using ConfigGen.Config;
using ConfigGen.TypeInfo;
using System.Collections.Generic;
using System.Data;

namespace ConfigGen.Import
{
    /// <summary>
    /// Excel 行列读取器
    /// </summary>
    public class ImportExcel : Import
    {
        private List<List<object>> _lines;
        private int _ri;//行
        private int _ci;//列
        private string _relPath;

        public ImportExcel(string relPath)
        {
            _relPath = relPath;
            _lines = new List<List<object>>();
            _ri = 0;
            _ci = -1;

            string absPath = Util.GetAbsPath(relPath);
            var tables = Util.ReadXlsxFile(absPath).Tables;
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
                    if (column.StartsWith(Consts.RowEndFlag))
                    {
                        ++_ri;
                        _ci = -1;
                    }
                    else if (!column.IsEmpty())
                    {
                        if (column.StartsWith(Consts.SetEndFlag))
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
        protected override void Error(string msg)
        {
            string error = string.Format("错误:{0} \n位置:{1}[{2}{3}]", msg, _relPath, GetColumnName(_ci + 1), _ri + 1);
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
                    if (column.StartsWith(Consts.RowEndFlag))
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
                Error(v + "非long类型");
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
        public override void GetClass(FClass data, ClassInfo info)
        {
            if (info.IsDynamic())
            {
                if (!info.Inherit.IsEmpty())
                {
                    ClassInfo parent = ClassInfo.Get(info.Inherit);
                    GetClass(data, parent);
                }

                string type = GetString();
                ClassInfo dynamic = ClassInfo.Get(type);
                if (dynamic == null)
                    Error("多态类型" + type + "未知");
                if (!info.HasChild(type))
                    Error(string.Format("数据类型{0}非{1}子类", type, info.FullName));
                data.SetCurrentType(type);
                info = dynamic;
            }

            var fields = info.Fields;
            for (int i = 0; i < fields.Count; i++)
                data.Values.Add(Data.Create(data, fields[i], this));
        }
        public override void GetList(FList data, FieldInfo define)
        {
            FieldInfo item = define.GetItemDefine();
            while (!IsSectionEnd())
                data.Values.Add(Data.Create(data.Host, item, this));
        }
        public override void GetDict(FDict data, FieldInfo define)
        {
            FieldInfo key = define.GetKeyDefine();
            FieldInfo value = define.GetValueDefine();
            while (!IsSectionEnd())
            {
                var dk = Data.Create(data.Host, key, this);
                var dv = Data.Create(data.Host, value, this);
                data.Values.Add(dk, dv);
            }
        }
    }
}
