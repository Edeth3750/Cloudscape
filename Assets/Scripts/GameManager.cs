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
    
    private bool gameState = true;
    public bool GameState
    {
        get
        {
            return gameState;
        }
    }

    [SerializeField] private float timer = 0f; 
    [Header("Events")]
    [SerializeField] private List<EventTimeline> Events = new List<EventTimeline>(); 

    [SerializeField] private List<CloudSpawnerSettings> SettingsChanges = new List<CloudSpawnerSettings>();
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
        for(int x=0; x<SettingsChanges.Count; x++)
        {
            if(SettingsChanges[x].Timer < timer)
            {
                CloudSpawner.cloudSpawner.CloudSettings(SettingsChanges[x]);
                SettingsChanges.Remove(SettingsChanges[x]);
                x--;
            }
        }
        timer += Time.deltaTime; 
    }
}
