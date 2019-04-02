using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;

namespace Xml
{
	publicabstract class abstract
	{
				public abstract void Write(TextWriter os);
				public abstract void Read(XmlNode os);


		public  string ReadAttribute(XmlNode node, string attribute)
		{
						try
			{
								if (node != null && node.Attributes != null && node.Attributes[attribute] != null)
									return node.Attributes[attribute].Value;
			}
						catch (Exception ex)
			{
								throw new Exception(string.Format("attribute:{0} not exist", attribute), ex);
			}
						return "";
		}
		
		public  XmlNode GetOnlyChild(XmlNode parent, string name)
		{
						XmlNode child = null;
						foreach (XmlNode sub in parent.ChildNodes)
			{
								if (sub.NodeType == XmlNodeType.Element && sub.Name == name)
				{
										if (child != null)
											throw new Exception(string.Format("child:{0} duplicate", name));
										child = sub;
				}
			}
						return child;
		}
		
		public  List<XmlNode> GetChilds(XmlNode parent)
		{
						var childs = new List<XmlNode>();
						if (parent != null)
							childs.AddRange(parent.ChildNodes.Cast<XmlNode>().Where(sub => sub.NodeType == XmlNodeType.Element));
						return childs;
		}
		
		public  bool ReadBool(XmlNode node)
		{
						string str = node.InnerText.ToLower();
						if (str == "true")
							return true;
						else if (str == "false")
							return false;
						throw new Exception(string.Format("'{0}' is not valid bool", str));
		}
		
		public  int ReadInt(XmlNode node)
		{
						return int.Parse(node.InnerText);
		}
		
		public  long ReadLong(XmlNode node)
		{
						return long.Parse(node.InnerText);
		}
		
		public  float ReadFloat(XmlNode node)
		{
						return float.Parse(node.InnerText);
		}
		
		public  string ReadString(XmlNode node)
		{
						return node.InnerText;
		}
		
		public  T ReadObject<T>(XmlNode node, string fullTypeName) where T :XmlObject
		{
						var obj = (T)Create(node, fullTypeName);
						obj.Read(node);
						return obj;
		}
		
		public  T ReadDynamicObject<T>(XmlNode node, string ns) where T :XmlObject
		{
						var fullTypeName = ns + "." + ReadAttribute(node, "Type");
						return ReadObject<T>(node, fullTypeName);
		}
		
		public  object Create(XmlNode node, string type)
		{
						try
			{
								var t = Type.GetType(type);
								return Activator.CreateInstance(t);
			}
						catch (Exception e)
			{
								throw new Exception(string.Format("type:{0} create fail!", type), e);
			}
		}
		
		public  void Write(TextWriter os, string name, bool x)
		{
						os.WriteLine("<{0}>{1}</{0}>", name, x);
		}
		
		public  void Write(TextWriter os, string name, int x)
		{
						os.WriteLine("<{0}>{1}</{0}>", name, x);
		}
		
		public  void Write(TextWriter os, string name, long x)
		{
						os.WriteLine("<{0}>{1}</{0}>", name, x);
		}
		
		public  void Write(TextWriter os, string name, float x)
		{
						os.WriteLine("<{0}>{1}</{0}>", name, x);
		}
		
		public  void Write(TextWriter os, string name, string x)
		{
						os.WriteLine("<{0}>{1}</{0}>", name, x);
		}
		
		public  void Write(TextWriter os, string name, XmlObject x)
		{
						os.WriteLine("<{0} Type =\"{1}\">", name, x.GetType().Name);
						x.Write(os);
						os.WriteLine("</{0}>", name);
		}
		
		public  void Write<V>(TextWriter os, string name, List<V> x)
		{
						os.WriteLine("<{0}>", name);
						x.ForEach(v => Write(os, "Item", v));
						os.WriteLine("</{0}>", name);
		}
		
		public  void Write<K, V>(TextWriter os, string name, Dictionary<K, V> x)
		{
						os.WriteLine("<{0}>", name);
						foreach (var e in x)
			{
								os.WriteLine("<Pair>");
								Write(os, "Key", e.Key);
								Write(os, "Value", e.Value);
								os.WriteLine("</Pair>");
			}
						os.WriteLine("</{0}>", name);
		}
		
		public  void Write(TextWriter os, string name, object x)
		{
						if (x is bool)
							Write(os, name, (bool)x);
						else if (x is int)
							Write(os, name, (int)x);
						else if (x is long)
							Write(os, name, (long)x);
						else if (x is float)
							Write(os, name, (float)x);
						else if (x is string)
							Write(os, name, (string)x);
						else if (x is XmlObject)
							Write(os, name, (XmlObject)x);
						else
							throw new Exception("unknown Lson type; " + x.GetType());
		}
		
		public void LoadAConfig(string file)
		{
						var doc = new XmlDocument();
						doc.Load(file);
						Read(doc.DocumentElement);
		}
		
		public void SaveAConfig(string file)
		{
						var os = new StringWriter();
						Write(os, "Root", this);
						File.WriteAllText(file, os.ToString());
		}
		
		public  void LoadConfig<T>(List<T> x, string file) where T :XmlObject
		{
						var doc = new XmlDocument();
						doc.Load(file);
						x.AddRange(GetChilds(doc.DocumentElement).Select(_ => ReadDynamicObject<T>(_, typeof(T).Namespace)));
		}
		
		public  void SaveConfig<T>(List<T> x, string file) where T :XmlObject
		{
						var os = new StringWriter();
						Write(os, "Root", x);
						File.WriteAllText(file, os.ToString());
		}
		
	}
}
