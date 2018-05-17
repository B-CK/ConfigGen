using System;
using System.Text;
using System.Collections.Generic;

namespace Csv
{
	public class DataStream
	{
		public DataStream(string path, Encoding encoding)
		{
		}

		public int GetInt()
		{
			int result;
			int.TryParse(Next(), out result);
			return result;
		}
		public long GetLong()
		{
			long result;
			long.TryParse(Next(), out result);
			return result;
		}
		public float GetFloat()
		{
			float result;
			float.TryParse(Next(), out result);
			return result;
		}
		public bool GetBool()
		{
			return !Next().Equals("0");
		}
		public string GetString()
		{
			return Next();
		}


		private int _rIndex;
		private int _cIndex;
		private int _maxRow;
		private int _maxcolumn;
		private string[] _rows;
		private string[] _columns;

		private void NextRow()
		{
		}

		private string Next()
		{
		}

	}
}
