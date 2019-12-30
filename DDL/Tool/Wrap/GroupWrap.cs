using Description;
using System.Collections.Generic;

namespace Wrap
{
    public class GroupWrap
    {
        private static HashSet<string> _groups;
        private GroupWrap() { }

        public static void LoadGroup(string g)
        {
            _groups = new HashSet<string>(Util.Split(g == null ? "" : g.ToLower()));
            if (!_groups.Contains(Setting.DefualtGroup))
                _groups.Add(Setting.DefualtGroup);
        }
        public static bool IsGroup(string group)
        {
            return _groups.Contains(group);
        }
    }
}
