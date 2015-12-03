using UnityEngine;
using System.Collections;

public class Piston : LocomotionComponent {

    SliderJoint2D joint;

    // Use this for initialization
	protected override void Start () {
        base.Start();
        joint = GetComponent<SliderJoint2D>();
	}
	
	// Update is called once per frame
    protected override void FixedUpdate()
    {
        JointMotor2D mot = joint.motor;
        if (isOn)
        {
            mot.motorSpeed = Mathf.Abs(mot.motorSpeed);
        }

        else
        {
            // reverse
            mot.motorSpeed = Mathf.Abs(mot.motorSpeed) * -1;
        }
        joint.motor = mot;
	}

    protected override void Move()
    {
        
    }
}
