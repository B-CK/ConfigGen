using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wrap
{
    public class GroupWrap
    {
        private static HashSet<string> _groups;
        private GroupWrap() { }

        public static void LoadGroup(string g)
        {
            _groups = new HashSet<string>(Util.SplitArgs(g.ToLowerExt()));
            if (!_groups.Contains(Setting.DefualtGroup))
                _groups.Add(Setting.DefualtGroup);
        }
        public static bool IsGroup(string group)
        {
            return _groups.Contains(group.ToLower());
        }
    }
}
