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

    void Crossover(long a, long b, out long alpha, out long beta) {
        
        // single-point crossover for now

        // pick random bit to start at from 50 to 10

        // I hope I'm doing this right
        
        //int n = Random.Range(10, 50);
        int n = 32;
        long x = (((long)1 << n) - 1);
        long a1 = a & x;
        long a2 = a - a1;

        Debug.Assert(a1 + a2 == a);

        long b1 = b & x;
        long b2 = b - b1;

        alpha = a1 + b2;
        beta = b1 + a2;
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

    void DispatchDrivers(long pattern1, long pattern2)
    {
        // TODO: actual GA
        foreach (Driver d in allDrivers) {
            long a, b;
            Crossover(pattern1, pattern2, out a, out b);
            // give them a random child
            if (Random.value > 0.5f) {
                d.GenerateDrivingPattern(a);
            }
            else {
                d.GenerateDrivingPattern(b);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (driversCheckedIn == allDrivers.Count)
        {
            GatherFitness();
            driversCheckedIn = 0;
        }
	}

    void GatherFitness()
    {
        // get top two
        // TODO: get N best candidates
        Driver best = fitnessQueue.Pop();
        Driver secondBest = fitnessQueue.Pop();
        long bestPattern = best.startingPattern;
        long secondBestPattern = secondBest.startingPattern;

        Debug.Log("Best fitness in this iteration: \n" + best.Fitness + "," + secondBest.Fitness);
        Debug.Log("Best patterns in this iteration: \n" + System.Convert.ToString(bestPattern, 16).PadLeft(16, '0') + "," + System.Convert.ToString(secondBestPattern, 16).PadLeft(16, '0'));

        DispatchDrivers(bestPattern, secondBestPattern);

        fitnessQueue.Clear();
    }
}