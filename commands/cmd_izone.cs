using System;
using System.Collections.Generic;

using UnityEngine;

using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Player;

using interception.enums;
using interception.zones;
using interception.extensions;
using interception.serialization.types;
using interception.serialization.types.zones;

using interception.plugins.zones.extensions;

namespace interception.plugins.zones.commands {
    internal class cmd_izone : IRocketCommand {
        public void Execute(IRocketPlayer caller, string[] args) {
            UnturnedPlayer p = (UnturnedPlayer)caller;
            if (args.Length < 1) {
                p.Player.say_to(Syntax, Color.red);
                return;
            }
            if (args[0].ToLower() == "create") {
                if (!p.HasPermission("interception.zones.izone.create")) {
                    p.Player.say_to(R.Translate("command_no_permission"), Color.red);
                    return;
                }
                if (args.Length < 3) {
                    p.Player.say_to(Syntax_Create, Color.red);
                    return;
                }
                if (main.cfg.zones.is_zone_exist(args[1])) {
                    p.Player.say_to(main.instance.Translate("zone_already_exist", args[1]), Color.red);
                    return;
                }
                if (zone_manager.is_zone_exist(args[1])) {
                    p.Player.say_to(main.instance.Translate("zone_name_is_occupied", args[1]), Color.red);
                    return;
                }

                if (args[2].ToLower() == "sphere") {
                    if (args.Length < 4) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    if (!float.TryParse(args[3], out float rad)) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    zone_manager.create_sphere_zone(args[1].ToLower(), p.Position, rad);
                    main.cfg.zones.Add(new s_sphere_zone(e_zone_type.sphere, args[1], p.Position, rad));
                    main.instance.Configuration.Save();
                    p.Player.say_to(main.instance.Translate("zone_created", e_zone_type.sphere, args[1]), Color.green);
                }
                else if (args[2].ToLower() == "distance_fast") {
                    if (args.Length < 4) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    if (!float.TryParse(args[3], out float rad)) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    zone_manager.create_distance_fast_zone(args[1].ToLower(), p.Position, rad);
                    main.cfg.zones.Add(new s_distance_fast_zone(e_zone_type.distance_fast, args[1], p.Position, rad));
                    main.instance.Configuration.Save();
                    p.Player.say_to(main.instance.Translate("zone_created", e_zone_type.distance_fast, args[1]), Color.green);
                }
                else if (args[2].ToLower() == "distance_slow") {
                    if (args.Length < 4) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    if (!float.TryParse(args[3], out float rad)) {
                        p.Player.say_to(Syntax_Create_Sphere_Distance, Color.red);
                        return;
                    }
                    zone_manager.create_distance_slow_zone(args[1].ToLower(), p.Position, rad);
                    main.cfg.zones.Add(new s_distance_slow_zone(e_zone_type.distance_slow, args[1], p.Position, rad));
                    main.instance.Configuration.Save();
                    p.Player.say_to(main.instance.Translate("zone_created", e_zone_type.distance_slow, args[1]), Color.green);
                }
                else if (args[2].ToLower() == "box") {
                    if (args.Length < 6) {
                        p.Player.say_to(Syntax_Create_Box, Color.red);
                        return;
                    }
                    float x,y,z;
                    if (!float.TryParse(args[3], out x) || !float.TryParse(args[4], out y) || !float.TryParse(args[5], out z)) {
                        p.Player.say_to(Syntax_Create_Box, Color.red);
                        return;
                    }
                    zone_manager.create_box_zone(args[1].ToLower(), p.Position, new Vector3(x, y, z));
                    main.cfg.zones.Add(new s_box_zone(e_zone_type.box, args[1], p.Position, p.Player.transform.forward, new s_vector3(x, y, z)));
                    main.instance.Configuration.Save();
                    p.Player.say_to(main.instance.Translate("zone_created", e_zone_type.box, args[1]), Color.green);
                }
                else if (args[2].ToLower() == "mesh") {
                    if (args.Length < 4) {
                        p.Player.say_to(Syntax_Create_Mesh, Color.red);
                        return;
                    }
                    if (!float.TryParse(args[3], out float h)) {
                        p.Player.say_to(Syntax_Create_Mesh, Color.red);
                        return;
                    }
                    zone_manager.create_mesh_zone(args[1].ToLower(), p.Position, h, null);
                    main.cfg.zones.Add(new s_mesh_zone(e_zone_type.mesh, args[1], p.Position, h, null));
                    main.instance.Configuration.Save();
                    p.Player.say_to(main.instance.Translate("zone_created", e_zone_type.mesh, args[1]), Color.green);
                }
            }
            else if (args[0].ToLower() == "remove") {
                if (!p.HasPermission("interception.zones.izone.remove")) {
                    p.Player.say_to(R.Translate("command_no_permission"), Color.red);
                    return;
                }
                if (args.Length < 2) {
                    p.Player.say_to(Syntax_Remove, Color.red);
                    return;
                }
                if (!main.cfg.zones.try_find_zone_index(args[1], out int index)) {
                    p.Player.say_to(main.instance.Translate("zone_not_exist", args[1]), Color.red);
                    return;
                }
                zone_manager.remove_zone(args[1].ToLower());
                main.cfg.zones.RemoveAt(index);
                main.instance.Configuration.Save();
                p.Player.say_to(main.instance.Translate("zone_removed", args[1]), Color.yellow);
            }
            else if (args[0].ToLower() == "debug") {
                if (!p.HasPermission("interception.zones.izone.debug")) {
                    p.Player.say_to(R.Translate("command_no_permission"), Color.red);
                    return;
                }
                zone_manager.toggle_debug_mode();
            }
            else if (args[0].ToLower() == "count") {
                if (!p.HasPermission("interception.zones.izone.count")) {
                    p.Player.say_to(R.Translate("command_no_permission"), Color.red);
                    return;
                }
                p.Player.say_to(zone_manager.get_zones_count().ToString(), Color.yellow);
            }
            else {
                p.Player.say_to(Syntax, Color.red);
                return;
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "izone";
        public string Syntax => "/izone [create/remove/debug/count] [args*]";
        public string Syntax_Create => "/izone create [name] [type] [args]";
        public string Syntax_Create_Sphere_Distance => "/izone create [name] [sphere/distance_fast/distance_slow] [radius]";
        public string Syntax_Create_Box => "/izone create [name] [box] [x] [y] [z]";
        public string Syntax_Create_Mesh => "/izone create [name] [mesh] [raycast_height]";
        public string Syntax_Remove => "/izone remove [name]";
        public string Help => "null";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>() { "interception.zones.izone" };
    }
}
