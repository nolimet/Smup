using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Util.Saving
{
    /// <summary>
    /// class that handels serialization and writing to disk
    /// </summary>
    public static class Serialization
    {
        [DllImport("__Internal")]
        private static extern void SyncFiles();

        [DllImport("__Internal")]
        private static extern void WindowAlert(string message);

        #region fileSaveSettings

        /// <summary>
        /// File types that are defined.
        /// </summary>
        public enum FileTypes
        {
            Binary,
            Text,
            SaveHead,
            GameState,
            Wave = 4
        }

        /// <summary>
        /// Location of the save data
        /// </summary>
        public static string SaveFolderName = "GameData";

        /// <summary>
        /// A dictonary contain information related to a filetype
        /// </summary>
        public static readonly Dictionary<FileTypes, string> FileExstentions = new()
            {
                { FileTypes.Binary, ".bin" },
                { FileTypes.Text, ".txt" },
                { FileTypes.SaveHead, ".sav" },
                { FileTypes.GameState, ".sav" },
                { FileTypes.Wave, ".wva" },
            },
            FileLocations = new()
            {
                { FileTypes.Binary, "Data" },
                { FileTypes.Text, "Data" },
                { FileTypes.SaveHead, "Saves\\" + "Head" },
                { FileTypes.GameState, "Saves\\" + "GameState" },
                { FileTypes.Wave, "Waves" },
            };

        #endregion

        /// <summary>
        /// Generates a string for where the file is located
        /// </summary>
        /// <param name="fileType">The type of file can matter for directory</param>
        /// <returns></returns>
        public static string SaveLocation(FileTypes fileType)
        {
            var saveLocation = Application.dataPath;
            if (!Application.isEditor)
                saveLocation += "/..";
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                saveLocation = Application.persistentDataPath;

            saveLocation += "/" + SaveFolderName + "/" + FileLocations[fileType] + "/";
            if (!Directory.Exists(saveLocation)) Directory.CreateDirectory(saveLocation);
            return saveLocation;
        }

        /// <summary>
        /// Returns file type with name attached
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="fileType">The type of file</param>
        /// <returns>Name + Type </returns>
        private static string GetFileType(string fileName, FileTypes fileType)
        {
            return fileName + FileExstentions[fileType];
        }

        /// <summary>
        /// Save file to disk
        /// </summary>
        /// <typeparam name="T">Type of the file</typeparam>
        /// <param name="fileName">File name with out exstentions</param>
        /// <param name="fileType">The type of file</param>
        /// <param name="data">The actual data fo the file</param>
        public static void Save<T>(string fileName, FileTypes fileType, T data)
        {
            fileName = fileName.Replace('/', '#').Replace("\\\"", "#").Replace(':', '#')
                .Replace('?', '#').Replace('"', '#').Replace('|', '#').Replace('*', '#').Replace('>', '#')
                .Replace('<', '#');

            var saveFile = SaveLocation(fileType);
            saveFile += GetFileType(fileName, fileType);

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                formatter.Serialize(stream, data);
                stream.Close();
            }
            catch (Exception e)
            {
                PlatformSafeMessage("Failed to Save: " + e.Message);
            }

            if (Application.platform == RuntimePlatform.WebGLPlayer)
                SyncFiles();

            Debug.Log(DateTime.Now + " Saved file: " + saveFile);
        }

        /// <summary>
        /// Loads a file from disk
        /// </summary>
        /// <typeparam name="T">Type of the file</typeparam>
        /// <param name="fileName"> Name of the file</param>
        /// <param name="fileType">The file exstention Type</param>
        /// <param name="outputData">A ref for the file that will be loaded</param>
        /// <returns>if the loading was succesfull. Needed because a save file can be non existant</returns>
        public static bool Load<T>(string fileName, FileTypes fileType, ref T outputData)
        {
            var saveFile = SaveLocation(fileType);
            saveFile += GetFileType(fileName, fileType);
            try
            {
                if (!File.Exists(saveFile))
                {
                    outputData = default;
                    return false;
                }

                IFormatter formatter = new BinaryFormatter();
                var stream = new FileStream(saveFile, FileMode.Open);

                var data = (T)formatter.Deserialize(stream);
                outputData = data;
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                Debug.LogError($"Failed to load config at {SaveLocation(fileType)}");
                throw;
            }
        }

        public static T Load<T>(string fileName, FileTypes fileType = 0, bool fileNameHasPointer = false)
        {
            string saveFile;
            if (!fileNameHasPointer)
            {
                saveFile = SaveLocation(fileType);
                saveFile += GetFileType(fileName, fileType);
            }
            else
            {
                saveFile = fileName;
            }

            T outputData;

            if (!File.Exists(saveFile))
            {
                Debug.Log("failed to find File");
                Debug.Log(saveFile);
                outputData = default;
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                var stream = new FileStream(saveFile, FileMode.Open);
                var data = (T)formatter.Deserialize(stream);
                outputData = data;

                stream.Close();
            }

            return outputData;
        }

        /// <summary>
        /// Used to generate an error when there is one while saving or loading
        /// </summary>
        /// <param name="message">The message that will be shown</param>
        private static void PlatformSafeMessage(string message)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
                WindowAlert(message);
            else
                Debug.Log(message);
        }
    }
}
