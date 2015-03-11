using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    Bar Energy;
    public float currentEnergy = 0;
    public float maxEnergy = 200;

    void Start()
    {
        Energy.init(maxEnergy, 2f);
    }
	
	// Update is called once per frame
	void Update () 
    {
        currentEnergy += 16f * Time.deltaTime;
        Energy.UpdateSize(currentEnergy);
	}
}
