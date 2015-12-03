using UnityEngine;
using System.Collections;

public class VehicleComponent : MonoBehaviour {

    public float pulseFrequency = 2f;   // frequency of pulse, divide the choice frequency by this

    private bool isSustained = false;

	// Use this for initialization
	protected virtual void Start () {
	    
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    
	}

    protected virtual void On()
    {

    }

    protected virtual void Off()
    {

    }

    protected void Pulse()
    {

        isSustained = false;
    }

    protected void Sustain()
    {
        isSustained = true;
    }
}
