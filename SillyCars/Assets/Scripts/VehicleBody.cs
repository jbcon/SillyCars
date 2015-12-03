using UnityEngine;
using System.Collections;

public enum ComponentType {  PISTON, LEG, WHEEL };

public class VehicleBody : MonoBehaviour {

    // sockets for the vehicle components
    public GameObject FrontSocket;
    public GameObject BackSocket;

    public LocomotionComponent frontControl;
    public LocomotionComponent backControl;

    public GameObject piston;
    public GameObject leg;
    public GameObject wheel;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        InitComponents();
	}

    void InitComponents()
    {
        // make front component
        frontControl = AttachLeg(FrontSocket);
        // make back component
        backControl = AttachPiston(BackSocket);
    }


    void AttachWheel(GameObject socket)
    {

    }

    LocomotionComponent AttachLeg(GameObject socket)
    {
        // make WheelJoint2D on Vehicle body
        WheelJoint2D legJoint = gameObject.AddComponent<WheelJoint2D>();
        GameObject l = Instantiate(leg) as GameObject;

        l.transform.parent = transform;
        l.transform.localPosition = socket.transform.localPosition;
        legJoint.connectedBody = l.GetComponent<Rigidbody2D>();
        legJoint.connectedAnchor = l.transform.localPosition;
        legJoint.anchor = socket.transform.localPosition;
        legJoint.useMotor = true;
        JointSuspension2D sus = new JointSuspension2D();
        sus.frequency = 5;
        sus.dampingRatio = 1;
        legJoint.suspension = sus;
        JointMotor2D newMotor = new JointMotor2D();
        newMotor.motorSpeed = -300;
        newMotor.maxMotorTorque = 100;
        legJoint.motor = newMotor;

        SpinningLeg spinleg = l.GetComponent<SpinningLeg>();
        spinleg.SetRefToJoint(legJoint);
        return spinleg;
    }

    LocomotionComponent AttachPiston(GameObject socket)
    {
        GameObject p = Instantiate(piston) as GameObject;
        p.transform.parent = transform;
        p.transform.localPosition = socket.transform.localPosition;
        
        SliderJoint2D slider = p.GetComponent<SliderJoint2D>();
        slider.connectedBody = rb;
        slider.connectedAnchor = socket.transform.localPosition;

        return p.GetComponent<Piston>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
