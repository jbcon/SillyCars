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

    public void SetRefToJoint(WheelJoint2D joint)
    {
        wjoint = joint;
    }

    protected override void FixedUpdate()
    {
        wjoint.useMotor = isOn;
    }
}
