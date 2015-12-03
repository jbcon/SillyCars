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

	//number of frames to hold a single driving state for
	public int UpdateInterval = 50;

	//maximum of 32 because of limitation in drivingStates (long = 64 bits = 32 couplets)
	public int NumberOfStates = 32;

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
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void FixedUpdate()
	{
		if (stateIndex >= NumberOfStates)
			return;

		numHoldFrames++;
		if (numHoldFrames >= UpdateInterval)
		{
			numHoldFrames = 0;

			bool frontState = ((drivingStates >> (stateIndex * 2 + 0)) & 0x1) == 0x1;
			bool backState =  ((drivingStates >> (stateIndex * 2 + 1)) & 0x1) == 0x1;

			Debug.Log("Update " + stateIndex.ToString() + ": " + frontState.ToString() + ", " + backState.ToString());

			stateIndex++;
		}
	}

	void GenerateDrivingPattern()
	{
		System.Random rand = new System.Random();
		drivingStates = rand.Next();
		drivingStates |= ((long)rand.Next() << 32);
		Debug.Log(Convert.ToString(drivingStates, 2));
	}
}
