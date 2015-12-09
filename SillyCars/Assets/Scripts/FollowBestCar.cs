using UnityEngine;
using System.Collections;

public class FollowBestCar : MonoBehaviour {

    public float snapSpeed = 3;

    Driver[] drivers;

	// Use this for initialization
	void Start () {
        drivers = FindObjectsOfType<Driver>();

	}
	
	// Update is called once per frame
	void Update () {

        float maxfit = 0;
        GameObject tracked = drivers[0].gameObject;
        foreach (Driver d in drivers)
        {
            if (d.transform.position.x > maxfit)
            {
                maxfit = d.transform.position.x;
                tracked = d.gameObject;
            }
        }

        Vector3 newPos = new Vector3(0, 0, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos + tracked.transform.position, Time.deltaTime * snapSpeed);
	}
}
