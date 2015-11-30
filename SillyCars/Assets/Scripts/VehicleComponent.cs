using UnityEngine;
using System.Collections;

public class VehicleComponent : MonoBehaviour {

    public float pulseFrequency = 2f;   // frequency of pulse, divide the choice frequency by this

    private bool isSustained = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Pulse()
    {

        isSustained = false;
    }

    void Sustain()
    {
        isSustained = true;
    }
}
