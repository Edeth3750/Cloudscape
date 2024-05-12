using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Movement")]
    [SerializeField] private int CloudSpeed = 5;
    [Range(0,360)]
    [SerializeField] private int windDirection = 270;

    [Header("Cloud Settings")]
    [SerializeField] private int CloudSize = 2;

    [Range(0,5)]
    [SerializeField] private int MinCloudDensity = 5;
    [Range(1,5)]
    [SerializeField] private int MaxCloudDensity = 5;
    [SerializeField] private bool RandomRotation = false;
    [Range(0f,5f)]
    [SerializeField] private float RotationUpdateTimer = 1f; 
    
    [Header("Spawn Settings")]
    [SerializeField] private bool InitialCloudFill = false;
    [Range(1,30)]
    [SerializeField] private float cloudSpawnTimer = 3f;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float horizontalVariance = 10f;
    [SerializeField] private float verticalVariance = 5f;

    [Header("Boundary Settings")]
    [SerializeField] private int cloudHeight = 50;
    [SerializeField] private int boundaryWidth = 100;
    [SerializeField] private int boundaryLength = 100;

    [Header("Fade Settings")]
    [SerializeField] private float IncrementDelay = 0.2f;
    [Range(0,1)]
    [SerializeField] private float FadeIncrement = 0.05f;

    private GameObject CloudLayer;
    private float timer = 0f;
    private int sortOrder = 0;
    private float updateTimer = 0f;
    private List<Cloud> CloudList = new List<Cloud>();

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
    
    private GameObject[] cloudPrefabs;

    void Start()
    {
        transform.position = new Vector3(0,0,0);
        CloudLayer = new GameObject();
        CloudLayer.transform.parent = transform;
        CloudLayer.name = "CloudLayer";
        transform.eulerAngles = new Vector3(0,windDirection,0);
        cloudPrefabs = Resources.LoadAll<GameObject>("Clouds");
        if(InitialCloudFill) InitialCloudSpawn();
    }

    void Update()
    {
        CloudLayer.transform.Translate(Vector3.forward * CloudSpeed * Time.deltaTime); 
        if(timer > cloudSpawnTimer)
        {
            spawnClouds(-boundaryLength/2, true);
            cullClouds();
            timer = 0;
        } else 
        {
            timer += Time.deltaTime;
        }
        updateTimer += Time.deltaTime;
        if(RotationUpdateTimer > 0f && updateTimer > RotationUpdateTimer)
        {
            updateTimer = 0f;
            foreach(Cloud c in CloudList)
            {
                c.UpdateRotation(); 
            }
        }
    }

    private void spawnClouds(float zLoc, bool FadeIn)
    {
        for(int i=0; i < spawnCount; i++)
        {
            float extraHeight = Random.Range(0,verticalVariance);
            GameObject clouds = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)], transform);
            clouds.transform.localPosition = new Vector3(Random.Range(-boundaryWidth/2, boundaryWidth/2), 
                                                        cloudHeight + extraHeight, 
                                                        zLoc - Random.Range(0,horizontalVariance));
            if(RandomRotation)
            {
                int rand = Random.Range(0, 360);
                for(int x = 0; x < clouds.transform.childCount; x++)
                {
                    clouds.transform.GetChild(x).transform.localRotation = Quaternion.Euler(-90, rand, 0);
                }
            }
            clouds.transform.localScale = new Vector3(CloudSize, 1, CloudSize);
            clouds.transform.parent = CloudLayer.transform;

            Cloud c = clouds.GetComponent<Cloud>();
            CloudList.Add(c);
            c.SetLayer(CalculateSortLayer(extraHeight), sortOrder);
            sortOrder++;
            if(RotationUpdateTimer > 0f) c.UpdateRotation(); 
            c.SetDensity(Random.Range(MinCloudDensity, MaxCloudDensity));
            if(FadeIn) StartCoroutine(Fade(clouds, false)); 
        }
    }

    private void InitialCloudSpawn()
    {
        for(float y = -boundaryLength/2; y < boundaryLength/2; y = y + cloudSpawnTimer * cloudSpawnTimer)
        {
            spawnClouds(y, false);
        }
    }

    private string CalculateSortLayer(float h)
    {
        float percentile = h / horizontalVariance;
        if(percentile < 0.2f) return "CloudLayer1";
        if(percentile < 0.4f) return "CloudLayer2";
        if(percentile < 0.6f) return "CloudLayer3";
        if(percentile < 0.8f) return "CloudLayer4";
        return "CloudLayer5";
    }

    IEnumerator Fade(GameObject clouds, bool isFading) 
    {
        for (float ft = 0f; ft <= 1; ft += FadeIncrement) 
        {
            for(int x = 0; x < clouds.transform.childCount; x++)
            {
                Color c = clouds.transform.GetChild(x).GetComponent<Renderer>().material.color; 
                c.a = ft;
                if(isFading) c.a = 1 - ft;
                clouds.transform.GetChild(x).GetComponent<Renderer>().material.color = c;
            }
            yield return new WaitForSeconds(IncrementDelay);
        }
        if(!isFading)
        {
            //setting transparency to default after fade in
            for(int x = 0; x < clouds.transform.childCount; x++)
            {
                Color c = clouds.transform.GetChild(x).GetComponent<Renderer>().material.color; 
                c.a = 1f;
                clouds.transform.GetChild(x).GetComponent<Renderer>().material.color = c;
            }
        } else 
        {
            //deleting transparent Clouds
            CloudList.Remove(clouds.GetComponent<Cloud>());
            Destroy(clouds);
        }
    }

    //Culls clouds that are too far, includes horizontalVariance to avoid culling newly spawned clouds
    private void cullClouds()
    {
        for(int i=0; i<CloudLayer.transform.childCount; i++)
        {
            GameObject cloud = CloudLayer.transform.GetChild(i).gameObject;
            if(cloud.transform.position.x > boundaryWidth/2+horizontalVariance ||
            cloud.transform.position.x < -boundaryWidth/2-horizontalVariance ||
            cloud.transform.position.z > boundaryLength/2+horizontalVariance ||
            cloud.transform.position.z < -boundaryLength/2-horizontalVariance ) 
            {
                if(cloud.transform.GetChild(0).GetComponent<Renderer>().material.color.a == 1f)
                {
                    StartCoroutine(Fade(cloud, true)); 
                } 
            }
        }
    }

    public void CloudSettings(CloudSpawnerSettings other)
    {
        CloudSpeed = other.CloudSpeed;
        CloudSize = other.CloudSize;
        MinCloudDensity = other.MinCloudDensity;
        MaxCloudDensity = other.MaxCloudDensity;
        if(MinCloudDensity > MaxCloudDensity) MaxCloudDensity = MinCloudDensity;
        RandomRotation = other.RandomRotation;
        cloudSpawnTimer = other.cloudSpawnTimer;
        if(cloudSpawnTimer < 1f) cloudSpawnTimer = 1f;
        spawnCount = other.spawnCount;
        horizontalVariance = other.horizontalVariance;
        verticalVariance = other.verticalVariance;
    }
}