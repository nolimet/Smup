using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace util
{
    public static class Serialization
    {
        #region fileSaveSettings
        public enum fileTypes
        {
            wave,
            save,
            settings,
            indexFile,
            binary
        }

        public static string saveFolderName = "GameData";
        readonly public static Dictionary<fileTypes, string> fileExstentions = new Dictionary<fileTypes, string>
        {
            { fileTypes.save,       ".sav"  },
            { fileTypes.settings,   ".set"  },
            { fileTypes.wave,       ".wva"  },
            { fileTypes.indexFile,  ".idex" },
            { fileTypes.binary,     ".bin"  }
        },

            FileLocations = new Dictionary<fileTypes, string>
            {
            { fileTypes.save,       "Save"      },
            { fileTypes.settings,   "Settings"  },
            { fileTypes.wave,       "Waves"     },
            { fileTypes.binary,     "Data"      }
            };
        #endregion

        public static string SaveLocation(fileTypes fileType)
        {
            string saveLocation = "";
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.isEditor)
            {
                saveLocation = Application.dataPath;
                if (!Application.isEditor)
                    saveLocation += "/..";
            }
            else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
            {
                saveLocation = Application.persistentDataPath;
            }

            saveLocation += "/" + saveFolderName + "/" + FileLocations[fileType] + "/";
            if (!Directory.Exists(saveLocation))
            {
                Directory.CreateDirectory(saveLocation);
            }
            return saveLocation;
        }

        public static string GetFileType(string fileName, fileTypes fileType)
        {
            return fileName + fileExstentions[fileType];
        }

        public static void Save<T>(string fileName, fileTypes fileType, T token)
        {
            string saveFile = SaveLocation(fileType);
            saveFile += GetFileType(fileName, fileType);

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            formatter.Serialize(stream, token);
            stream.Close();

            Debug.Log("Saved file: " + saveFile);
        }

        public static bool Load<T>(string fileName, fileTypes fileType, ref T outputData)
        {
            string saveFile = SaveLocation(fileType);
            saveFile += GetFileType(fileName, fileType);
            bool returnval = false;


            if (!File.Exists(saveFile))
            {
                outputData = default(T);
                returnval = false;
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(saveFile, FileMode.Open);

                T data = (T)formatter.Deserialize(stream);
                outputData = data;
                returnval = true;
                stream.Close();
            }
            return returnval;
        }
    }
}