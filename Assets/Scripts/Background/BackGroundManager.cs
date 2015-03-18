using UnityEngine;
using System.Collections;

public class BackGroundManager : MonoBehaviour
{

    [SerializeField]Layer[] layers;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Layer l in layers)
        {
            foreach (Transform t in l.parts)
            {
                if (t.localPosition.x < l.killPoint)
                    t.localPosition += new Vector3(l.startPoint*2f, 0);
                t.localPosition -= new Vector3(l.speed * Time.deltaTime, 0);
            }
        }
    }

    [System.Serializable]
    class Layer
    {
        public string name;
        public Transform[] parts;
        public float speed, startPoint, killPoint;
    }
}
