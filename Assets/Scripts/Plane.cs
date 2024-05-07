using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>(); 
    private bool Launched = false;
    private int currentWaypoint = 0;
    private Vector3 nextPoint;
    private Vector3 direction;
    
    //Event start
    public void Launch()
    {
        Launched = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void Start()
    {
        nextPoint = waypoints[currentWaypoint];
        direction = (nextPoint - transform.position).normalized;
        transform.position = nextPoint;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(Launched)
        {
            //Calculate distance to waypoint
            float distanceToWaypoint = Vector3.Distance(nextPoint, transform.position);

            //Checks if waypoint is reached this update
            if (distanceToWaypoint <= Speed * Time.deltaTime)
            {
                NextWaypoint();
            }
            else
            {
                transform.Translate(direction * Speed * Time.deltaTime, Space.World);
            }
        }
    }

    private void NextWaypoint()
    {
        if(currentWaypoint+1 != waypoints.Count)
        {
            transform.position = nextPoint;
            currentWaypoint++;
            nextPoint = waypoints[currentWaypoint];
            direction = (nextPoint - transform.position).normalized;
            transform.LookAt(nextPoint);
        } else
        {
            transform.position = nextPoint;
            Launched = false;
            Destroy(gameObject);
        }
    }
}