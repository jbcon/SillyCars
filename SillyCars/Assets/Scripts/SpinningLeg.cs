using UnityEngine;
using System.Collections;

public class SpinningLeg : LocomotionComponent {

    // actually connected to the vehicle
    WheelJoint2D wjoint;

	// Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetRefToJoint(WheelJoint2D joint)
    {
        wjoint = joint;
    }
}
