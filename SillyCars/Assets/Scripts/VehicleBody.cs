using UnityEngine;
using System.Collections;

public enum ComponentType {  PISTON, LEG, WHEEL };

public class VehicleBody : MonoBehaviour {

    // sockets for the vehicle components
    public GameObject FrontSocket;
    public GameObject BackSocket;

    public GameObject piston;
    public GameObject leg;
    public GameObject wheel;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        AttachPiston(FrontSocket);
        
        AttachPiston(BackSocket);
	}

    void InitComponents()
    {
        // make front component
        

        // make back component


    }


    void AttachWheel(GameObject socket)
    {

    }

    void AttachLeg(GameObject socket)
    {
        // make WheelJoint2D on Vehicle body
        

    }

    void AttachPiston(GameObject socket)
    {
        GameObject p = Instantiate(piston) as GameObject;
        p.transform.parent = transform;
        p.transform.localPosition = socket.transform.localPosition;
        
        SliderJoint2D slider = p.GetComponent<SliderJoint2D>();
        slider.connectedBody = rb;
        slider.connectedAnchor = socket.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, FrontSocket.transform.position);
        Debug.DrawLine(transform.position, BackSocket.transform.position);
	}
}
