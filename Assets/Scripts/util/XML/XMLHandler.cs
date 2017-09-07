using UnityEngine;
using System.Collections;
using System.IO;

namespace Util.XML
{
    public class XMLHandler : MonoBehaviour
    {
        public DataContainer data;
        public string fileName;
        public string path;
        public void Save()
        {
            data.Save(Application.dataPath + path, fileName);
        }

        public void Load()
        {
            data = DataContainer.Load(path, fileName);
        }
    }
}