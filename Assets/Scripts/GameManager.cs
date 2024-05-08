using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager gameManager;
    static public GameManager instance
    {
        get
        {
            if (gameManager == null)
            {
                Debug.LogError("There is no EventManager in the scene.");
            }
            return gameManager;
        }
    }

    void Awake()
    {
        if (gameManager != null)
        {
            // destroy duplicates
            Destroy(gameObject);
        }
        else
        {
            gameManager = this;
        }
    }
    
    [SerializeField] private float timer; 
    [Header("Events")]
    [SerializeField] private List<EventTimeline> Events = new List<EventTimeline>(); 

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        for(int x=0; x<Events.Count; x++)
        {
            if(Events[x].isTime(timer))
            {
                Events[x].StartEvent();
                Events.Remove(Events[x]);
                x--;
            }
        }
        timer += Time.deltaTime; 
    }

    public void TestEvent1()
    {
        Debug.Log("test event 1");
    }

    public void TestEvent2()
    {
        Debug.Log("test event 2");
    }

    public void TestEvent3()
    {
        Debug.Log("test event 3");
    }
}
