using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.LocalInfo
{
    [XmlRoot("FindInfo")]
    public class FindInfo : BaseInfo
    {
        public List<FindState> FindStates { get; set; }

        private const int MaxValue = 20;
        private HashSet<string> _findStates = new HashSet<string>();
        public void Init()
        {
            if (FindStates == null || FindStates.Count == 0)
            {
                FindStates = new List<FindState>();
                return;
            }
            for (int i = 0; i < FindStates.Count; i++)
            {
                if (!_findStates.Contains(FindStates[i].Content))
                    _findStates.Add(FindStates[i].Content);
            }
        }
        public void Add(object info)
        {
            if (FindStates.Count == MaxValue)
                FindStates.RemoveAt(MaxValue - 1);
            FindState findState = info as FindState;
            if (!_findStates.Contains(findState.Content))
            {
                _findStates.Add(findState.Content);
                FindStates.Insert(0, findState);
            }
            else
            {
                FindState state = FindStates.Find(f => f.Content == findState.Content);
                state.Files.AddRange(findState.Files);
            }
        }
        public void Remove(object info) { }
        public void UpdateList()
        {
             
        }

        public void Save()
        {
            UpdateList();
            string path = LocalInfoManager.GetInfoPath(LocalInfoType.FindInfo);
            Util.Serialize(path, this);
        }
    }
    [XmlInclude(typeof(FindState))]
    public class FindState
    {
        private List<string> _files;
        public string Content { get; set; }
        /// <summary>
        /// 文件路径列表[相对路径]
        /// </summary>
        public List<string> Files
        {
            get
            {
                if (_files == null)
                    _files = new List<string>();
                Update();
                return _files;
            }
            set
            {
                _files = value;
                Update();
            }
        }

        private void Update()
        {
            List<string> ls = new List<string>();
            foreach (var f in _files)
            {
                string path = Util.GetConfigAbsPath(f);
                if (File.Exists(path))
                    ls.Add(f);
            }
            _files = ls;
        }
    }
}
