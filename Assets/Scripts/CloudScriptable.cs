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

    public Quaternion SetRotation(float posX, float posZ)
    {
        float z = posZ / zRotationMulitplier;
        float x = posX / xRotationMulitplier;
        if(z > maxTiltY) z = maxTiltY;
        else if(z < -maxTiltY) z = -maxTiltY;
        if(x > maxTiltX) x = maxTiltX;
        else if(x < -maxTiltX) x = -maxTiltX;
        
        return Quaternion.Euler(z, 0, x * -1);
    }
}
