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
        transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
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
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward);
        } else
        {
            transform.position = nextPoint;
            Launched = false;
            StartCoroutine(FadeOut());
        }
    }
    
    [SerializeField] private float IncrementDelay = 0.2f;
    [SerializeField] private float FadeIncrement = 0.05f;
    IEnumerator FadeOut() 
    {
        Transform mainBody = transform.GetChild(0).transform;
        for (float ft = 1f; ft >= 0; ft -= FadeIncrement) 
        {
            for(int x = 0; x < mainBody.childCount; x++)
            {
                Color c = mainBody.GetChild(x).GetComponent<Renderer>().material.color; 
                c.a = ft;
                mainBody.GetChild(x).GetComponent<Renderer>().material.color = c;
            }
            yield return new WaitForSeconds(IncrementDelay);
        }
        Destroy(gameObject);
    }
}