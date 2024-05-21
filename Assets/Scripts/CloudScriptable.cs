using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cloud", menuName = "Scriptables/CloudScriptable", order = 1)]
public class CloudScriptable : ScriptableObject
{
    public float maxTiltX = 30f;
    public float maxTiltY = 30f;
    public float xRotationMulitplier = 10f;
    public float zRotationMulitplier = 10f;
    public float heightMultiplier = 5f;

    public Quaternion SetRotation(float posX, float posZ)
    {
        float x = posX / xRotationMulitplier;
        float z = posZ / zRotationMulitplier;
        if(x > maxTiltX) x = maxTiltX;
        else if(x < -maxTiltX) x = -maxTiltX;
        if(z > maxTiltY) z = maxTiltY;
        else if(z < -maxTiltY) z = -maxTiltY;
        
        return Quaternion.Euler(z, 0, x * -1);
    }

    public float AdjustHeight(float posX, float posY, float posZ)
    {
        //reduce height per unit of distance x:0 z:0
        float height = -heightMultiplier * Mathf.Max(Mathf.Abs(posX), Mathf.Abs(posZ)) / 2;
        return height;
    }
}
