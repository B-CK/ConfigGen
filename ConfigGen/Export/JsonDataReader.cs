using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.Export
{
    //class JsonDataReader
    //{
    //    JsonReader _reader;

    //    public JsonDataReader(string path)
    //    {
    //        string json = File.ReadAllText(path);
    //        _reader = new JsonReader(json);
    //    }

    //    private object Value 
    //    {
    //        get
    //        {
    //            if (_reader.Read())
    //                return _reader.Value;
    //            return null;
    //        }
    //    }

    //    public int ReadInt()
    //    {
    //        if (_reader.Token == JsonToken.Int)
    //        {
    //            return Convert.ToInt32(Value);
    //        }
    //        else
    //        {
    //            throw new Exception("Json 读取数据类型不匹配,Int != " + _reader.Token);
    //        }
    //    }
    //    public long ReadLong()
    //    {
    //        if (_reader.Token == JsonToken.Int || _reader.Token == JsonToken.Long)
    //        {
    //            return Convert.ToInt64(Value);
    //        }
    //        else
    //        {
    //            throw new Exception("Json 读取数据类型不匹配,Long != " + _reader.Token);
    //        }
    //    }
    //    public float ReadFloat()
    //    {
    //        if (_reader.Token == JsonToken.Double)
    //        {
    //            return Convert.ToSingle(Value);
    //        }
    //        else
    //        {
    //            throw new Exception("Json 读取数据类型不匹配,Double != " + _reader.Token);
    //        }
    //    }
    //    public bool ReadBool()
    //    {
    //        if (_reader.Token == JsonToken.Boolean)
    //        {
    //            return Convert.ToBoolean(Value);
    //        }
    //        else
    //        {
    //            throw new Exception("Json 读取数据类型不匹配,Boolean != " + _reader.Token);
    //        }
    //    }
    //    public string ReadString()
    //    {
    //        if (_reader.Token == JsonToken.String)
    //        {
    //            return Convert.ToString(Value);
    //        }
    //        else
    //        {
    //            throw new Exception("Json 读取数据类型不匹配,String != " + _reader.Token);
    //        }
    //    }   
    //}
}
