using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CloudSpawnerSettings 
{
    [Header("Activation Time")]
    [Range(0,435)]
    public float Timer;
    [Header("Cloud Settings")]
    public int CloudSpeed = 5;
    public int CloudSize = 2;
    [Range(0,5)] 
    public int MinCloudDensity = 5;
    [Range(1,5)] 
    public int MaxCloudDensity = 5;
    public bool RandomRotation = false;
    [Range(0f,5f)]
    public float RotationUpdateTimer = 1f; 
    
    [Header("Cloud Spawn Settings")]
    [Range(1,30)]
    public float cloudSpawnTimer = 3f;
    public int spawnCount = 3;
    public float horizontalVariance = 10f;
    public float verticalVariance = 5f;
}