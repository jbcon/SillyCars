using UnityEngine;
using System.IO;
using System.Collections;

public class Bookie : MonoBehaviour {

    public static Bookie current;
    //FileStream outputFile;
    StreamWriter output;

    public float currentBestFitness = 0;
    public long currentBestPattern;

	// Use this for initialization
	void Start () {
        if (!current)
        {
            current = this;
        }
        output = new StreamWriter("Assets/stats.csv");
	}

    public void UpdateStats(float fitness, long pattern)
    {
        currentBestFitness = fitness;
        currentBestPattern = pattern;
    }

    public void WriteStats()
    {
        output.WriteLine(System.Convert.ToString(currentBestFitness) + ',' + System.Convert.ToString(currentBestPattern, 16).PadLeft(16, '0'));
    }

    public void WriteStatsToFile()
    {
        output.Close();
        output = new StreamWriter("Assets/stats.csv", true);
    }
}
