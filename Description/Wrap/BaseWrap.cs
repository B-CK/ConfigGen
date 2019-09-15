using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    public abstract class BaseWrap : IDisposable
    {
        public string Name { get { return _name; } set { _name = value; } }
        protected string _name;

        public abstract string DisplayName { get; }
        public virtual string FullName { get { return _name ?? "_"; } }
        public virtual NodeState NodeState { get { return _nodestate; } }
        /// <summary>
        /// 子对象名称集合,用于保证子对象不重名
        /// </summary>
        private HashSet<string> _hash;
        private NodeState _nodestate = NodeState.Include;
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
        public virtual bool Valide()
        {
            return true;
        }
        public virtual void Dispose()
        {
            _nodestate = NodeState.Include;
            _hash.Clear();
        }
        public void SetNodeState(NodeState state)
        {
            if (state == NodeState.Include)
                _nodestate &= ~NodeState.Exclude;
            else if (state == NodeState.Exclude)
                _nodestate &= ~NodeState.Include;
            _nodestate |= state;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
