using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace Util.Saving
{
    /// <summary>
    /// class that handels serialization and writing to disk
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// File types that are defined.
        /// </summary>
        public enum FileTypes
        {
            Binary,
            Wave
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
                {
                    FileTypes.Binary, ".bin"
                },
                {
                    FileTypes.Wave, ".wva"
                },
            },
            FileLocations = new()
            {
                {
                    FileTypes.Binary, "Data"
                },
                {
                    FileTypes.Wave, "Waves"
                },
            };

        /// <summary>
        /// Generates a string for where the file is located
        /// </summary>
        /// <param name="fileType">The type of file can matter for directory</param>
        /// <returns></returns>
        public static string SaveLocation(FileTypes fileType)
        {
            var saveLocation = Application.dataPath;
            if (!Application.isEditor) saveLocation += "/..";
            if (Application.platform == RuntimePlatform.WebGLPlayer) saveLocation = Application.persistentDataPath;

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
        private static string GetFileType(string fileName, FileTypes fileType) => fileName + FileExstentions[fileType];

        public static void SaveBinary<T>([NotNull] T instance) where T : IBinarySerializable
        {
            SaveBinaryInternal(instance.FileName, instance);
        }

        public static void SaveBinary<T>(string fileName, [NotNull] T instance) where T : IBinarySerializable
        {
            SaveBinaryInternal(fileName, instance);
        }

        public static void SaveBinaryInternal<T>(string fileName, [NotNull] T instance) where T : IBinarySerializable
        {
            var targetDirectory = SaveLocation(FileTypes.Binary);
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

            var filePath = Path.Combine(targetDirectory, GetFileType(fileName, FileTypes.Binary));
            var bytes = instance.GetBytes();
            File.WriteAllBytes(filePath, bytes);
        }

        public static bool TryLoadBinary<T>(out T data) where T : IBinarySerializable, new()
        {
            data = new T();
            return TryLoadBinaryInternal(data.FileName, ref data);
        }

        public static bool TryLoadBinary<T>(string fileName, out T data) where T : IBinarySerializable, new()
        {
            data = new T();
            return TryLoadBinaryInternal(fileName, ref data);
        }

        private static bool TryLoadBinaryInternal<T>(string fileName, ref T data) where T : IBinarySerializable, new()
        {
            var filePath = Path.Combine(SaveLocation(FileTypes.Binary), GetFileType(fileName, FileTypes.Binary));
            if (!File.Exists(filePath))
            {
                data = default;
                return false;
            }

            data ??= new T();
            var bytes = File.ReadAllBytes(filePath);
            data.ApplyBytes(bytes);
            return true;
        }
    }
}
