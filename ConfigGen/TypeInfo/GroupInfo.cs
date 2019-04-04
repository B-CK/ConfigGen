using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGen.TypeInfo
{
    public class GroupInfo
    {
        private static HashSet<string> _groups;
        private GroupInfo() { }

        public static void LoadGroup(string g)
        {
            _groups = new HashSet<string>(Util.Split(g));
            if (!_groups.Contains(Setting.DefualtGroup))
                _groups.Add(Setting.DefualtGroup);
        }
        public static bool IsGroup(string group)
        {
            return _groups.Contains(group);
        }
    }
}
