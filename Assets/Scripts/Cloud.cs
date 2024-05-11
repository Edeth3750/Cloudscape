using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    //Using magic num for now, ill make it adjustable later
    public void UpdateRotation()
    {
        float z = transform.position.z/10;
        float x = transform.position.x/10;
        if(z > 30) z = 30;
        if(z < -30) z = -30;
        if(x > 30) x = 30;
        if(x < -30) x = -30;
        transform.rotation = Quaternion.Euler(z, 0, x * -1);
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
