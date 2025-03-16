using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Util.XML
{
    [XmlRoot("WaveData")]
    [Serializable]
    public class DataContainer
    {
        //[XmlArray("waves"), XmlArrayItem("wave")]
        //public wave[] Test;

        public void Save(string path, string fileName)
        {
            var pathpoint = Path.Combine(Application.dataPath, path);
            try
            {
                var serializer = new XmlSerializer(typeof(DataContainer));

                if (!Directory.Exists(pathpoint))
                    Directory.CreateDirectory(Path.Combine(Application.dataPath, path));

                using (var stream = new FileStream(Path.Combine(Path.Combine(Application.dataPath, path), fileName), FileMode.Create))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch (IOException e)
            {
                Debug.LogException(e);
            }
        }

        public static DataContainer Load(string path, string fileName)
        {
            var serializer = new XmlSerializer(typeof(DataContainer));
            Debug.Log(Application.dataPath);
            Debug.Log(Path.Combine(Application.dataPath + path, fileName));
            using (var stream = new FileStream(Path.Combine(Application.dataPath + path, fileName), FileMode.Open))
            {
                return serializer.Deserialize(stream) as DataContainer;
            }
        }

        //Loads the xml directly from the given string. Useful in combination with www.text.
        public static DataContainer LoadFromText(string text)
        {
            var serializer = new XmlSerializer(typeof(DataContainer));
            return serializer.Deserialize(new StringReader(text)) as DataContainer;
        }
    }
}
