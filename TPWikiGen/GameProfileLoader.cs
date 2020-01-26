using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace TPWikiGen
{
    public static class GameProfileLoader
    {
        public static List<GameProfile> GameProfiles { get; set; }

        public static void LoadProfiles(string profileFolder, string descFolder)
        {
            var profiles = Directory.GetFiles(profileFolder, "*.xml");

            List<GameProfile> profileList = new List<GameProfile>();
            List<GameProfile> userprofileList = new List<GameProfile>();
            foreach (var file in profiles)
                {
                    var gameProfile = JoystickHelper.DeSerializeGameProfile(file, false);
                    if (gameProfile == null) continue;
                    gameProfile.GameInfo = JoystickHelper.DeSerializeDescription(file, descFolder);
                    profileList.Add(gameProfile);
                    continue;
                }
                GameProfiles = profileList.OrderBy(x => x.GameName).ToList();
        }

        static GameProfileLoader()
        {
        }
    }
}
