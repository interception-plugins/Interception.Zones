using System;
using System.Collections.Generic;

using interception.serialization.types.zones;

namespace interception.plugins.zones.extensions {
    public static class cfg_ex {
        public static bool is_zone_exist(this List<s_zone> l, string name) {
            var len = l.Count;
            for (int i = 0; i < len; i++) {
                if (l[i].name.ToLower() == name.ToLower()) {
                    return true;
                }
            }
            return false;
        }

        public static bool try_find_zone_index(this List<s_zone> l, string name, out int index) {
            var len = l.Count;
            for (int i = 0; i < len; i++) {
                if (l[i].name.ToLower() == name.ToLower()) {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }
    }
}
