using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Dispatcher : MonoBehaviour {

    // singleton reference to self
    public static Dispatcher current = null;
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.04f;

    [HideInInspector]
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

    void SinglePointCrossover(long a, long b, out long alpha, out long beta) {
        
        // single-point crossover for now

        // pick random bit to start at from 50 to 10

        // I hope I'm doing this right
        
        int n = Random.Range(0, 63);
        //int n = 32;
        long x = (((long)1U << n) - 1);
        long a1 = a & x;
        long a2 = a - a1;

        Debug.Assert(a1 + a2 == a);

        long b1 = b & x;
        long b2 = b - b1;

        alpha = a1 + b2;
        beta = b1 + a2;
    }

    void MultiPointCrossover(long a, long b, out long alpha, out long beta)
    {
        PerformMultiPoint(a, b, out alpha, out beta, 0, 16);
        //PerformMultiPoint(a, b, out alpha, out beta, 16, 32);
        PerformMultiPoint(a, b, out alpha, out beta, 32, 48);
        //PerformMultiPoint(a, b, out alpha, out beta, 48, 64);
    }


    // helper for the above, where start and end are between 0 and 63
    void PerformMultiPoint(long a, long b, out long alpha, out long beta, int start, int end)
    {
		//0001111000
		long mask = ((1 << (end - start)) - 1) << start;
		long temp = a & mask;

		//a & ~mask = 1110000111
		alpha = (a & ~mask) | (b & mask);
		beta = (b & ~mask) | (temp);
    }

    long GenerateRandomPattern(System.Random rand)
    {
        long pattern = rand.Next();
        pattern |= ((long)rand.Next() << 32);
        return pattern;
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

    long Mutate(long pattern)
    {
        if (Random.value < mutationRate)
        {
            long p = pattern;
            int bitNum = Random.Range(0, 63);
            long x = (long)1 << bitNum;
            p ^= x;
            Debug.Log("MMMUUUUTTTTTAAAAATTTTIIIOOONNNN");
            return p;
        }
        return pattern;
    }

    void DispatchDrivers(long pattern1, long pattern2)
    {
        // TODO: actual GA
        System.Random rand = new System.Random();
        for (int i = 0; i < allDrivers.Count; i++) {
            long a, b;
            MultiPointCrossover(pattern1, pattern2, out a, out b);
            // give them a random child
            if (i == 0) {
                allDrivers[i].GenerateDrivingPattern(Mutate(a));
            }
            else if (i == 1)
            {
                allDrivers[i].GenerateDrivingPattern(Mutate(b));
            }
            else {
                long p = GenerateRandomPattern(rand);
                allDrivers[i].GenerateDrivingPattern(Mutate(p));
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

        if (best.Fitness > Bookie.current.currentBestFitness)
        {
            Bookie.current.UpdateStats(best.Fitness, bestPattern);
        }

        Bookie.current.WriteStats();

        Debug.Log("Best fitness in this iteration: \n" + best.Fitness + "," + secondBest.Fitness);
        Debug.Log("Best patterns in this iteration: \n" + System.Convert.ToString(bestPattern, 16).PadLeft(16, '0') + "," + System.Convert.ToString(secondBestPattern, 16).PadLeft(16, '0'));

        DispatchDrivers(bestPattern, secondBestPattern);

        fitnessQueue.Clear();
    }
}