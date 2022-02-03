using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;
using UnityEngine.Events;

public class AudioEventTimer : MonoBehaviour
{
    [SerializeField] AudioSource source;

    //Treated as a stack.
    [SerializeField] List<TimedEvent> events;
    //Not tied to any functionality, only to be able to see in the inspector how long until the next event.
    [SerializeField] long untilNext;


    void Start()
    {
        TimedEvent last = null;
        foreach (TimedEvent ev in events) {
            //        \\       //        \\   <-- support beams
            if (last != null && ev.time <= last.time) {
                Debug.LogWarning("Invalid timed event ordering.");
            }
            last = ev;
        }
    }


    void Update()
    {
        if (events.Count > 0) {
            if (source.timeSamples >= events[0].time) {
                //          ||          \\
                events[0].trigger.Invoke();
                events.RemoveAt(0);
                Debug.Log(events.Count);
            }
            else {
                untilNext = events[0].time - source.timeSamples;
            }
        }
        else {
            this.enabled = false;
        }


        #region Arrays Implementation
        //for (int i = 0; i < triggers.Length; i++)
        //{
        //    if (triggers[i] == null) { break; }
        //    //               ||              \\
        //    else if (source.timeSamples > times[i])
        //    {
        //        triggers[i].Invoke();
        //        Debug.Log("Used: " + i);
        //        triggers[i] = null;

        //        if(i <= triggers.Length / 2)
        //        {
        //            Array.Resize(ref triggers, triggers.Length / 2);
        //        }
        //        break;
        //    }
        //}
        #endregion

        #region List Implementation
        //removed = false;

        //foreach (TimedEvents ev in events)
        //{
        //    if (ev.time >= source.timeSamples)
        //    {
        //        ev.trigger.Invoke();
        //        ev.used = true;
        //        removed = true;
        //    }
        //}

        //if (removed)
        //{
        //    events.RemoveAll(x => x.used);
        //}
        #endregion
    }
}

[Serializable]
public class TimedEvent
{
    public long time; //ago..
    public UnityEvent trigger;
}
