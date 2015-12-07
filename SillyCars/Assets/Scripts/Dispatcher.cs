using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Dispatcher : MonoBehaviour {

    // singleton reference to self
    public static Dispatcher current = null;
    public List<Driver> allDrivers;

    PriorityQueue<Driver> fitnessQueue;
    int iteration = 0, maxIterations = 100;
    int totalDrivers;
    int driversCheckedIn;
    

	// Use this for initialization
	void Start () {
        if (current == null)
        {
            current = this;
        }
        allDrivers = new List<Driver>(FindObjectsOfType<Driver>());
        fitnessQueue = new PriorityQueue<Driver>();
        driversCheckedIn = totalDrivers;
        InitialDispatch();
	}


    public void ReportCompletion(Driver driver)
    {
        fitnessQueue.Push(driver);
        driversCheckedIn++;
    }

    void InitialDispatch()
    {
        System.Random rand = new System.Random();
        foreach (Driver d in allDrivers)
        {
            long pattern = rand.Next();
            pattern |= ((long)rand.Next() << 32);
            d.GenerateDrivingPattern(pattern);
        }
    }

    void DispatchDrivers()
    {
        // TODO: actual GA
        InitialDispatch();
    }

	// Update is called once per frame
	void Update () {
        if (driversCheckedIn == allDrivers.Count)
        {
            GatherFitness();
            driversCheckedIn = 0;
            DispatchDrivers();
        }
	}

    void GatherFitness()
    {
        // get top two
        Driver best = fitnessQueue.Pop();
        Driver secondBest = fitnessQueue.Pop();

        Debug.Log("Best fitness in this iteration: \n" + best.Fitness + "\n" + secondBest.Fitness);

        fitnessQueue.Clear();
    }
}