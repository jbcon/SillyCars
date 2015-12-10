using UnityEngine;
using System.IO;
using System.Collections;

public class Bookie : MonoBehaviour {

    public static Bookie current;
    //FileStream outputFile;

    public float currentBestFitness = 0;
    public long currentBestPattern;

	// Use this for initialization
	void Start () {
        if (!current)
        {
            current = this;
        }
	}

    public void UpdateStats(float fitness, long pattern)
    {
        currentBestFitness = fitness;
        currentBestPattern = pattern;
    }

    public void WriteStats(float fit)
    {
        StreamWriter output = new StreamWriter("Assets/stats.csv", true);
        output.WriteLine(fit + ',' + System.Convert.ToString(currentBestFitness) + ',' + System.Convert.ToString(currentBestPattern, 16).PadLeft(16, '0'));
        output.Close();
    }

    public void WriteStatsToFile()
    {

    }
}
