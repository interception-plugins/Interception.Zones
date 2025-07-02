using System;
using System.Collections.Generic;

using SDG.Unturned;

using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.API.Collections;

using interception.serialization.types.zones;

namespace interception.plugins.zones {   
    public class config : IRocketPluginConfiguration, IDefaultable {
        public List<s_zone> zones;
       
        public void LoadDefaults() {
            zones = new List<s_zone>();
        }
    }

    public class main : RocketPlugin<config> {
        internal static main instance;
        internal static config cfg;

        protected override void Load() {
            instance = this;
            cfg = instance.Configuration.Instance;
            Level.onPostLevelLoaded += events.on_post_level_loaded;
            GC.Collect();
        }

        protected override void Unload() {
            Level.onPostLevelLoaded -= events.on_post_level_loaded;
            cfg = null;
            instance = null;
            GC.Collect();
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "zone_already_exist", "Zone named \"{0}\" already exist" },
            { "zone_name_is_occupied", "Zone named \"{0}\" was already created by some other plugin" },
            { "zone_created", "Zone with type \"{0}\" and name \"{1}\" successfully created" },
            { "zone_not_exist", "Zone named \"{0}\" does not exist" },
            { "zone_removed", "Zone named \"{0}\" successfully removed" },
            //{ "zone_destroyed", "Zone named \"{0}\" successfully destroyed" },
        };
    }
}

