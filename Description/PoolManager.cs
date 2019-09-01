using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description
{
    public class PoolManager
    {
        public static PoolManager Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new PoolManager();
                return _ins;
            }
        }
        private static PoolManager _ins;

        Dictionary<string, Queue<object>> _cache = new Dictionary<string, Queue<object>>();

        protected PoolManager() { }

        public void Push<T>(T obj) where T : class
        {
            string name = typeof(T).Name;
            if (_cache.ContainsKey(name))
            {
                _cache[name].Enqueue(obj);
            }
            else
            {
                Queue<object> queue = new Queue<object>();
                _cache.Add(name, queue);
            }
        }
        public T Pop<T>() where T : class
        {
            T obj = null;
            string name = typeof(T).Name;
            if (_cache.ContainsKey(name))
            {
                if (_cache[name].Count == 0)
                    obj = null;
                else
                    obj = _cache[name].Dequeue() as T;
            }

            return obj;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
