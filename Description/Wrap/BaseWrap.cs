using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    public class BaseWrap : IDisposable
    {
        public string Name { get { return _name; } set { _name = value; } }
        public virtual string FullName { get { return _name ?? "_"; } }

        protected string _name;
        private HashSet<string> _hash;
        protected BaseWrap(string name)
        {
            _name = name;
            _hash = new HashSet<string>();
        }
        public virtual bool Contains(string name)
        {
            return _hash.Contains(name);
        }
        protected virtual void Add(string name)
        {
            _hash.Add(name);
        }
        protected virtual void Remove(string name)
        {
            _hash.Remove(name);
        }
        public bool CheckName()
        {
            if (this is NamespaceWrap && _name == Util.EmptyNamespace)
                return true;
            if (!Util.MatchIdentifier(_name))
            {
                Util.MsgError("验证", "名称{0}不规范,请以'_',字母和数字命名且首字母只能为'_'和字母!");
                return false;
            }
            return true;
        }
        public virtual bool Valide()
        {
            return true;
        }
        public virtual void Dispose()
        {
            _hash.Clear();
            PoolManager.Ins.Pop<BaseWrap>();
        }
    }
}
