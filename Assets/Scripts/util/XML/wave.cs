using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class wave
{
    [XmlAttribute("name")]
    public string Name;
    [XmlAttribute("id")]
    public string id;

    [XmlArray("enemies"), XmlArrayItem("Enemy")]
    public Enemy[] enemies;

    [System.Serializable]
    public class Enemy
    {
        [XmlAttribute("tijd")]
        public string tijdAangewerkt;
        [XmlAttribute("type", typeof(int))]
        public int type;
    }
}
