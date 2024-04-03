using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Movement")]
    [SerializeField] private int CloudSpeed = 5;
    [Range(0,360)]
    [SerializeField] private int windDirection = 270;
    
    [Header("Spawn Settings")]
    [Range(1,30)]
    [SerializeField] private float cloudSpawnTimer = 3f;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float spawnVariance = 10f;
    [SerializeField] private GameObject CloudPrefabs;

    [Header("Boundary Settings")]
    [SerializeField] private int cloudHeight = 50;
    [SerializeField] private int boundaryWidth = 100;
    [SerializeField] private int boundaryLength = 100;
    
    private GameObject CloudLayer;
    private float timer = 0f;

    static private CloudSpawner _cloudSpawner;
    static public CloudSpawner cloudSpawner
    {
        get
        {
            if (_cloudSpawner == null)
            {
                Debug.LogError("There is no cloudSpawner in the scene.");
            }
            return _cloudSpawner;
        }
    }

    void Awake()
    {
        if (_cloudSpawner != null)
        {
            // destroy duplicates
            Destroy(gameObject);
        }
        else
        {
            _cloudSpawner = this;
        }
    }
    
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        CloudLayer = new GameObject();
        CloudLayer.transform.parent = transform;
        CloudLayer.name = "CloudLayer";
        transform.eulerAngles = new Vector3(0,windDirection,0);
    }

    void Update()
    {
        CloudLayer.transform.Translate(Vector3.forward * CloudSpeed * Time.deltaTime); 
        if(timer > cloudSpawnTimer)
        {
            spawnClouds();
            cullClouds();
            timer = 0;
        } else 
        {
            timer += Time.deltaTime;
        }
    }

    private void spawnClouds()
    {
        for(int i=0; i < spawnCount; i++)
        {
            GameObject clouds = Instantiate(CloudPrefabs, transform);
            clouds.transform.localPosition = new Vector3(Random.Range(-boundaryWidth/2, boundaryWidth/2), 
                                                        cloudHeight, 
                                                        -boundaryWidth/2-Random.Range(0,spawnVariance));
            clouds.transform.parent = CloudLayer.transform;
        }
    }

    //Culls clouds that are too far, includes spawnVariance to avoid culling newly spawned clouds
    private void cullClouds()
    {
        for(int i=0; i<CloudLayer.transform.childCount; i++)
        {
            GameObject cloud = CloudLayer.transform.GetChild(i).gameObject;
            if(cloud.transform.position.x > boundaryWidth/2+spawnVariance ||
            cloud.transform.position.x < -boundaryWidth/2-spawnVariance ||
            cloud.transform.position.z > boundaryLength/2+spawnVariance ||
            cloud.transform.position.z < -boundaryLength/2-spawnVariance ) 
            {
                Destroy(cloud);
            }
        }
    }
}
