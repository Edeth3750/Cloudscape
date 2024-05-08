using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotairBalloon : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] private float HorizontalSpeed = 5f;
    [SerializeField] private float AscendingSpeed = 5f;
    [SerializeField] private float MaxfallSpeed = 2f;
    [SerializeField] private float Acceleration = 0.5f;
    [SerializeField] private float Gravity = 0.5f;
    [SerializeField] private float BoundDist = 2f;
    private float VerticalMovement = 0f;
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>(); 

    private bool Launched = false;
    private int currentWaypoint = 0;
    private Vector3 nextPoint;
    private bool Ascending = true; 
    
    //Event start
    public void Launch()
    {
        Launched = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void Start()
    {
        nextPoint = waypoints[currentWaypoint];
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        if(Launched)
        {
            //Calculate distance to waypoint
            float distanceToWaypoint =  Mathf.Abs(nextPoint.x - transform.position.x) +
                                        Mathf.Abs(nextPoint.z - transform.position.z);

            //Checks if waypoint is reached this update
            if (distanceToWaypoint <= HorizontalSpeed * Time.deltaTime)
            {
                NextWaypoint();
            }
            else
            {
                //horizontal movement
                Vector3 direction = new Vector3(
                                        nextPoint.x - transform.position.x,
                                        0,
                                        nextPoint.z - transform.position.z);
                direction = direction.normalized;
                transform.Translate(direction * HorizontalSpeed * Time.deltaTime);
                
                //vertical movement
                if(!Ascending && VerticalMovement > -MaxfallSpeed)
                {
                    VerticalMovement -= Gravity * Time.deltaTime;
                } else if (Ascending && VerticalMovement < AscendingSpeed)
                {
                    VerticalMovement += Acceleration * Time.deltaTime;
                }
                transform.Translate(Vector3.up * VerticalMovement * Time.deltaTime);

                if(transform.position.y - nextPoint.y < -BoundDist)
                {
                    Ascending = true;
                } else if (transform.position.y > nextPoint.y)
                {
                    Ascending = false;
                }
            }
        }
    }

    private void NextWaypoint()
    {
        if(currentWaypoint+1 != waypoints.Count)
        {
            transform.position = new Vector3(nextPoint.x, transform.position.y, nextPoint.z);
            currentWaypoint++;
            nextPoint = waypoints[currentWaypoint];
        } else
        {
            transform.position = new Vector3(nextPoint.x, transform.position.y, nextPoint.z);
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
            Color c = mainBody.GetComponent<Renderer>().material.color; 
            c.a = ft;
            for(int x = 0; x < mainBody.childCount; x++)
            {
                c = mainBody.GetChild(x).GetComponent<Renderer>().material.color; 
                c.a = ft;
                mainBody.GetChild(x).GetComponent<Renderer>().material.color = c;
            }
            yield return new WaitForSeconds(IncrementDelay);
        }
        Destroy(gameObject);
    }
}