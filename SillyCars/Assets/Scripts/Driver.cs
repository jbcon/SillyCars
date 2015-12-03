using UnityEngine;
using System;
using System.Collections;

public class Driver : MonoBehaviour
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

	private bool isDone;
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
	void Start()
	{
		GenerateDrivingPattern();

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

		if (isDone)
		{
			//HACK don't restart here, do it somewhere else
			//     where the fitness is recorded along with
			//     the states
			Restart();
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
			bool backState =  ((drivingStates >> (stateIndex * 2 + 1)) & 0x1) == 0x1;

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

	void Restart()
	{
		Fitness = 0;
		isDone = false;
		numHoldFrames = 0;
		stateIndex = 0;

		transform.position = initialPos;
		transform.rotation = initialRot;

		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0;
		rb.Sleep();

		GenerateDrivingPattern();

	}

	void GenerateDrivingPattern()
	{
		System.Random rand = new System.Random();
		drivingStates = rand.Next();
		drivingStates |= ((long)rand.Next() << 32);
		Debug.Log(Convert.ToString(drivingStates, 2).PadLeft(64, '0'));
	}
}
