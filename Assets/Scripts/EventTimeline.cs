using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventTimeline 
{
    [Range(0,435)]
    public float TriggerTime;
    public UnityEvent MyEvent;
    private bool Activated = false; 

    public bool isTime(float Time)
    {
        if(Time >= TriggerTime)
        {
            return true;
        } 
        return false;
    }

    public void StartEvent()
    {
        MyEvent.Invoke();
    }
}