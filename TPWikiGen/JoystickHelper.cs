﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TPWikiGen
{
    public class JoystickHelper
    {

        /// <summary>
        /// Serializes GameProfile class to a GameProfile.xml file.
        /// </summary>
        /// <param name="profile"></param>
        public static void SerializeGameProfile(GameProfile profile, string filename = "")
        {
            var serializer = new XmlSerializer(profile.GetType());
            using (var writer = XmlWriter.Create(filename == "" ? Path.Combine("UserProfiles", Path.GetFileName(profile.FileName)) : filename, new XmlWriterSettings { Indent = true }))
            {
                serializer.Serialize(writer, profile);
            }
        }

        /// <summary>
        /// Deserializes GameProfile.xml to the class.
        /// </summary>
        /// <returns>Read Gameprofile class.</returns>
        public static GameProfile DeSerializeGameProfile(string fileName, bool userProfile)
        {
            if (!File.Exists(fileName)) return null;
            try
            {
                var serializer = new XmlSerializer(typeof(GameProfile));
                GameProfile profile;
                using (var reader = XmlReader.Create(fileName))
                {
                    profile = (GameProfile)serializer.Deserialize(reader);
                }

#if !DEBUG
                if (profile.DevOnly)
                {
                    Debug.WriteLine($"Skipping loading dev profile {fileName}");
                    return null;
                }
#endif

                // migrate stuff in case names get changed, only for UserProfiles
                if (userProfile)
                {
                    if (profile.EmulationProfile == EmulationProfile.FNFDrift)
                    {
                        profile.EmulationProfile = EmulationProfile.RawThrillsFNF;
                        SerializeGameProfile(profile, fileName);
                    }
                }
                return profile;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static Description DeSerializeDescription(string fileName, string dir)
        {
            var descriptionPath = Path.Combine(dir, Path.GetFileNameWithoutExtension(fileName) + ".json");
            if (File.Exists(descriptionPath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<Description>(File.ReadAllText(descriptionPath));
                }
                catch
                {
                    Debug.WriteLine($"Error loading description file {descriptionPath}!");
                }
            }
            else
            {
                Debug.WriteLine($"Description file {descriptionPath} missing!");
            }
            return null;
        }
    }
}
