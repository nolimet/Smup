using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
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
		private static readonly BinaryFormatter _binaryFormatter = new();

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
				/*{
					FileTypes.Text, ".txt"
				},
				{
					FileTypes.SaveHead, ".sav"
				},
				{
					FileTypes.GameState, ".sav"
				},*/
				{
					FileTypes.Wave, ".wva"
				},
			},
			FileLocations = new()
			{
				{
					FileTypes.Binary, "Data"
				},
				/*{
					FileTypes.Text, "Data"
				},
				{
					FileTypes.SaveHead, "Saves\\" + "Head"
				},
				{
					FileTypes.GameState, "Saves\\" + "GameState"
				},*/
				{
					FileTypes.Wave, "Waves"
				},
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
			{
				saveLocation += "/..";
			}
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				saveLocation = Application.persistentDataPath;
			}

			saveLocation += "/" + SaveFolderName + "/" + FileLocations[fileType] + "/";
			if (!Directory.Exists(saveLocation))
			{
				Directory.CreateDirectory(saveLocation);
			}
			return saveLocation;
		}

		/// <summary>
		/// Returns file type with name attached
		/// </summary>
		/// <param name="fileName">The name of the file</param>
		/// <param name="fileType">The type of file</param>
		/// <returns>Name + Type </returns>
		private static string GetFileType(string fileName, FileTypes fileType) => fileName + FileExstentions[fileType];

		#region Binary Saving & Loading
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
			if (!Directory.Exists(targetDirectory))
			{
				Directory.CreateDirectory(targetDirectory);
			}

			var filePath = Path.Combine(targetDirectory, GetFileType(fileName, FileTypes.Binary));
			var bytes = instance.GetBytes();
			File.WriteAllBytes(filePath, bytes);

			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				SyncFiles();
			}
		}

		public static bool TryLoadBinary<T>(out T outputData) where T : IBinarySerializable, new()
		{
			outputData = new T();
			TryLoadBinaryInternal(outputData.FileName, ref outputData);
			return true;
		}

		public static bool TryLoadBinary<T>(string fileName, out T outputData) where T : IBinarySerializable, new()
		{
			outputData = new T();
			return TryLoadBinaryInternal(fileName, ref outputData);
		}

		private static bool TryLoadBinaryInternal<T>(string fileName, ref T outputData) where T : IBinarySerializable, new()
		{
			var filePath = Path.Combine(SaveLocation(FileTypes.Binary), GetFileType(fileName, FileTypes.Binary));
			if (!File.Exists(filePath))
			{
				outputData = default;
				return false;
			}

			outputData ??= new T();
			var bytes = File.ReadAllBytes(filePath);
			outputData.ApplyBytes(bytes);
			return true;
		}
		#endregion Binary Saving & Loading

		#region Waves Saving & Loading
		public static void SaveWave(string fileName, WaveClass wave)
		{
			throw new NotImplementedException("Currently not supported... rework wave editor");
		}

		public static bool TryLoadWave(string fileName, out WaveClass wave)
		{
			var filePath = Path.Combine(SaveLocation(FileTypes.Wave), GetFileType(fileName, FileTypes.Wave));
			if (!File.Exists(filePath))
			{
				wave = null;
				return false;
			}

			//TODO replace with a json or binary format
			using var stream = new FileStream(filePath, FileMode.Open);
			wave = (WaveClass) _binaryFormatter.Deserialize(stream);
			return true;
		}
		#endregion Waves Saving & Loading

		/// <summary>
		/// Used to generate an error when there is one while saving or loading
		/// </summary>
		/// <param name="message">The message that will be shown</param>
		private static void PlatformSafeMessage(string message)
		{
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				WindowAlert(message);
			}
			else
			{
				Debug.Log(message);
			}
		}
	}
}
