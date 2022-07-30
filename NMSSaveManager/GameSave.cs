﻿using libNOM.map;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static NMSSaveManager.SaveCompression;

namespace NMSSaveManager
{
    public class GameSave
    {
        private dynamic _json;

        public static JObject DecryptSave(string hgFilePath, string jsonOutputPath)
        {
            //string rawSave = @".\temp_rawsave.json";
            DecompressSave(hgFilePath, out string rawjson);

            //string rawjson = File.ReadAllText(rawSave);
            string ufjson = JsonConvert.SerializeObject(JObject.Parse(rawjson), Formatting.Indented).TrimEnd('\0');

            // Sets json after modifying original values to key names                
            JObject jObject = JsonConvert.DeserializeObject(ufjson) as JObject;
            Mapping.Deobfuscate(jObject);

            // Writes new json string to jsonOutputPath
            File.WriteAllText(jsonOutputPath, jObject.ToString());

            //if (File.Exists(rawSave))
            //    File.Delete(rawSave);

            return jObject;
        }
        public static void EncryptSave(uint saveslot, string hgFilePath, string jsonInputPath)
        {
            string injson = File.ReadAllText(jsonInputPath);
            GameSaveManager _gsm = new GameSaveManager(Path.GetDirectoryName(hgFilePath));

            // Sets json from input and reverses all key names back to original
            JObject jObject = JsonConvert.DeserializeObject(injson) as JObject;
            Mapping.Obfuscate(jObject);
            injson = jObject.ToString();

            // Sets new save and checks if compressed
            GameSave newsave = new GameSave(injson);
            newsave.SetSaveToLatest(hgFilePath);
            bool FrontiersCheck = IsFrontiers(hgFilePath);

            // Writes the new save file
            _gsm.WriteSaveFile(newsave, saveslot);

            if (FrontiersCheck)
                CompressSave(hgFilePath);
        }
        public GameSave(string jsonStr)
        {
            _json = JObject.Parse(jsonStr);
        }
        public void SetSaveToLatest(string hgFilePath)
        {
            // Gets paths to modify
            string mf_hgFilePath = hgFilePath;
            mf_hgFilePath = String.Format("{0}{1}{2}{3}", Path.GetDirectoryName(mf_hgFilePath) + @"\", "mf_", Path.GetFileNameWithoutExtension(mf_hgFilePath), Path.GetExtension(mf_hgFilePath));

            //Sets the save to be the last modified
            File.SetLastWriteTime(mf_hgFilePath, DateTime.Now);
            File.SetLastWriteTime(hgFilePath, DateTime.Now);
        }
        public string ToFormattedJsonString()
        {
            string json = JsonConvert.SerializeObject(_json, Formatting.Indented);
            return json;
        }
        public string ToUnformattedJsonString()
        {
            string json = JsonConvert.SerializeObject(_json, Formatting.None);
            return json;
        }
    }
}