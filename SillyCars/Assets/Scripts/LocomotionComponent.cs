using UnityEngine;
using System.Collections;

public class LocomotionComponent : MonoBehaviour {


    protected bool isOn = false;

	// Use this for initialization
	protected virtual void Start () {
	    
	}

    void Update()
    {

    }

    public void On()
    {
        isOn = true;
    }

    public void Off()
    {
        isOn = false;
    }

    protected virtual void FixedUpdate()
    {
        if (isOn)
        {
            Move();
        }
    }

    protected virtual void Move()
    {

    }

}
