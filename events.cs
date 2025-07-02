using System;

using interception.enums;
using interception.zones;
using interception.serialization.types.zones;

namespace interception.plugins.zones {
    internal static class events {
        public static void on_post_level_loaded(int level) {
            var len = main.cfg.zones.Count;
            for (int i = 0; i < len; i++) {
                if (main.cfg.zones[i].type == e_zone_type.sphere) {
                    var zone = (s_sphere_zone)main.cfg.zones[i];
                    zone_manager.create_sphere_zone(zone.name, zone.position, zone.radius);
                    continue;
                }
                if (main.cfg.zones[i].type == e_zone_type.distance_fast) {
                    var zone = (s_distance_fast_zone)main.cfg.zones[i];
                    zone_manager.create_distance_fast_zone(zone.name, zone.position, zone.radius);
                    continue;
                }
                if (main.cfg.zones[i].type == e_zone_type.distance_slow) {
                    var zone = (s_distance_slow_zone)main.cfg.zones[i];
                    zone_manager.create_distance_slow_zone(zone.name, zone.position, zone.radius);
                    continue;
                }
                if (main.cfg.zones[i].type == e_zone_type.box) {
                    var zone = (s_box_zone)main.cfg.zones[i];
                    zone_manager.create_box_zone(zone.name, zone.position, zone.forward, zone.size);
                    continue;
                }
                if (main.cfg.zones[i].type == e_zone_type.mesh) {
                    var zone = (s_mesh_zone)main.cfg.zones[i];
                    zone_manager.create_mesh_zone(zone.name, zone.position, zone.height, zone.mask);
                    continue;
                }
            }
        }
    }
}
