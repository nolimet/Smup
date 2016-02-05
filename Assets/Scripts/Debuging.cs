using UnityEngine;
using UnityEngine.UI;
using util;
using System.Collections;

public class Debuging : MonoBehaviour {
    public Text text;
	// Use this for initialization
	void Start () {
        text.text = Serialization.SaveLocation(Serialization.fileTypes.binary);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
