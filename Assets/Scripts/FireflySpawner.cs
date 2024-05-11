using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireflyPrefab;
    [Header("Firefly Settings")]
    [SerializeField] private int numberOfFireflies = 40;
    [SerializeField] private float xAreaRadius = 40f;
    [SerializeField] private float zAreaRadius = 40f;
    [Range(0f, 100f)]
    [SerializeField] private float raycastHeight = 30f;

    RaycastHit hit;
    int layerMasking = 1 << 0;

    //Event start
    public void Launch()
    {
        // Reveal them
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Set itself to the origin while at a higher height
        this.transform.position = new Vector3(0f, raycastHeight, 0f);
        
        // For how many number of fireflies, distribute them randomly among the area radius and paste them to the ground below
        for (int i = 0; i < numberOfFireflies; i++)
        {
            GameObject firefly = Instantiate(fireflyPrefab, this.transform);
            firefly.transform.localPosition = new Vector3(Random.Range(-xAreaRadius, xAreaRadius), 0f, Random.Range(-zAreaRadius, zAreaRadius));
            if (Physics.Raycast(origin: firefly.transform.position, direction: Vector3.down, hitInfo: out hit, maxDistance: Mathf.Infinity, layerMask: layerMasking))
            {
                Debug.Log(hit.point.y);
                firefly.transform.position = new Vector3(firefly.transform.position.x, hit.point.y, firefly.transform.position.z);
            }
        }

        // Set them invisible for now
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
