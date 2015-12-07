using UnityEngine;
using System;
using System.Collections;

public class Driver : MonoBehaviour, System.IComparable<Driver>, System.IEquatable<Driver>
{

	//a pattern of on/off for the front and back sockets.
	//each couplet of bits represents the front and back sockets respectively
	//there is one couplet provided per update interval in order (little endian)
	//For example, 011100 means the following:
	// - 00: both back/front are off
	// - 11: both back/front are on
	// - 01: front is on, back is off
	private long drivingStates;

	private int stateIndex;
	private int numHoldFrames;

	private bool isDone, isWaiting;
	private Vector3 initialPos;
	private Quaternion initialRot;

	//number of frames to hold a single driving state for
	public int UpdateInterval = 50;

	//maximum of 32 because of limitation in drivingStates (long = 64 bits = 32 couplets)
	public int NumberOfStates = 32;

	public LocomotionComponent frontSwitch, backSwitch;

	public float Fitness = 0;

	public bool IsDone { get { return isDone; } }

	// Use this for initialization
	void Awake()
	{
		//Sanitize inputs
		if (NumberOfStates > 32)
			NumberOfStates = 32;
		if (NumberOfStates < 0)
			NumberOfStates = 0;

		if (UpdateInterval < 0)
			UpdateInterval = 0;

		initialPos = transform.position;
		initialRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void FixedUpdate()
	{
		if (frontSwitch == null || backSwitch == null)
			return;
        if (!isWaiting)
        {
            if (isDone)
            {
                //HACK don't restart here, do it somewhere else
                //     where the fitness is recorded along with
                //     the states

                ReportResults();

                return;
            }

            if (stateIndex >= NumberOfStates)
            {
                frontSwitch.Off();
                backSwitch.Off();
                isDone = true;
                Fitness = transform.position.x - initialPos.x;
                Debug.Log("Fitness: " + Fitness);
                return;
            }

            numHoldFrames++;
            if (numHoldFrames >= UpdateInterval)
            {
                numHoldFrames = 0;

                bool frontState = ((drivingStates >> (stateIndex * 2 + 0)) & 0x1) == 0x1;
                bool backState = ((drivingStates >> (stateIndex * 2 + 1)) & 0x1) == 0x1;

                //Debug.Log("Update " + stateIndex + ": " + frontState + ", " + backState);
                if (frontState)
                    frontSwitch.On();
                else
                    frontSwitch.Off();

                if (backState)
                    backSwitch.On();
                else
                    backSwitch.Off();

                stateIndex++;
            }
        }
	}

	void ReportResults()
	{
        Dispatcher.current.ReportCompletion(this);
        isWaiting = true;
	}

    void Reset()
    {
        Fitness = 0;
        isDone = false;
        numHoldFrames = 0;
        stateIndex = 0;

        transform.position = initialPos;
        transform.rotation = initialRot;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.velocity = Vector2.zero;
        //rb.angularVelocity = 0;
        rb.Sleep();
    }

	public void GenerateDrivingPattern(long pattern)
	{
        Reset();
        drivingStates = pattern;
        //Debug.Log(Convert.ToString(drivingStates, 2).PadLeft(64, '0'));
        isWaiting = false;
	}

    public int CompareTo(Driver other){
        if (this.Fitness < other.Fitness) return -1;
        if (this.Fitness == other.Fitness) return 0;
        return 1;
    }

    public bool Equals(Driver other)
    {
        return this.Fitness == other.Fitness;
    }
}
