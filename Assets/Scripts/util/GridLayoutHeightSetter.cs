using UnityEngine;
using System.Collections;

public class GridLayoutHeightSetter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, GetComponent<UnityEngine.UI.GridLayoutGroup>().preferredHeight);
	}
}
