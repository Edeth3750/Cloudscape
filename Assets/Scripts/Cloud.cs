using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private CloudScriptable config; 
    public void UpdateRotation()
    {
        transform.rotation = config.SetRotation(transform.position.x, transform.position.z);
    }

    public void SetDensity(int num)
    {
        for(int x = 0; x < transform.childCount - num; x++)
        {
            transform.GetChild(x).gameObject.SetActive(false);
        }
    }

    public void SetLayer(string str, int SortOrder)
    {
        for(int x = 0; x < transform.childCount; x++)
        {
            Renderer r = transform.GetChild(x).GetComponent<Renderer>();
            r.sortingLayerID = SortingLayer.NameToID(str);
            r.sortingOrder = SortOrder;
        }
    }
}
