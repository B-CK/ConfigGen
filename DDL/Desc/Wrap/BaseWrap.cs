using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desc.Wrap
{
    public abstract class BaseWrap : IDisposable
    {
        public Action<BaseWrap, string> OnNameChange;
        public Action<BaseWrap, NodeState> OnNodeStateChange;

        public virtual string Name
        {
            get { return _name; }
            set
            {
                if (_name.IsEmpty() || _name != value)
                {
                    string src = FullName;
                    _name = value;
                    OnNameChange?.Invoke(this, src);
                }
            }
        }
        protected string _name;

        public abstract string DisplayName { get; }

        /// <summary>
        /// 命名空间:Name
        /// Class/Enum:NamespaceName.Name
        /// </summary>
        public virtual string FullName { get { return _name ?? "_"; } }
        public virtual NodeState NodeState { get { return _nodestate; } }
        /// <summary>
        /// 数据集合,可作重复判断
        /// </summary>
        public HashSet<string> Hash => _hash;
        /// <summary>
        /// 子对象名称集合,用于保证子对象不重名
        /// </summary>
        private HashSet<string> _hash;
        private NodeState _nodestate = NodeState.Exclude;
        protected virtual void Init(string name)
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
        public virtual void Dispose()
        {
            OnNameChange = null;
            OnNodeStateChange = null;
            _nodestate = NodeState.Exclude;
            _hash.Clear();
        }
        /// <summary>
        /// 检查数据正确性
        /// </summary>
        /// <returns>true:数据正常;false:数据异常</returns>
        public virtual bool Check()
        {
            //bool r = Util.CheckIdentifier(_name);
            //if (r == false)
            //    Debug.LogErrorFormat("名称[{0}]不规范,请以'_',字母和数字命名且首字母只能为'_'和字母!", _name);
            //return r;
            return true;
        }
        /// <summary>
        /// 设置节点状态
        /// </summary>
        /// <param name="state">节点状态</param>
        /// <param name="isTriger">是否触发事件</param>
        public virtual void SetNodeState(NodeState state, bool isTriger = true)
        {
            if ((state & NodeState.Include) != 0)
                _nodestate &= ~NodeState.Exclude;
            else if ((state & NodeState.Exclude) != 0)
                _nodestate &= ~NodeState.Include;
            _nodestate = state;
            if (isTriger)
                OnNodeStateChange?.Invoke(this, state);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            if (FullName == null) return 0;
            return FullName.GetHashCode();
        }
        public override string ToString()
        {
            return FullName;
        }
    }
}
