using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAttackTimer : MonoBehaviour {
    [SerializeField] AudioSource source;
    [SerializeField] Transform target;

    [SerializeField] [Range(0f, 3f)] long defaultWarnTime;
    //Treated as a stack.
    [SerializeField] List<TimedAttack> attks;
    //Not tied to any functionality, only to be able to see in the inspector how long until the next event.
    [SerializeField] long untilNext;


    void Start() {
        defaultWarnTime *= source.clip.frequency;

        //List<TimedAttack> list = new List<TimedAttack>();
        TimedAttack last = null;
        byte i = 0;
        foreach (TimedAttack atk in attks) {
            //        \\       //        \\   <-- support beams
            if (last != null && atk.attkTime <= last.attkTime) {
                Debug.LogWarning("Invalid timed attack ordering.");
            }
            if(i++ > 7) {
                TimedAttack addAtk = new TimedAttack(atk);
                addAtk.attkTime += 537600;
                //list.Add(addAtk);
            }

            //IF WANT ALL ATTACKS TO HAVE A DEFAULT WARN. IMPLEMENTED DIFFERENTLY
            //if(atk.aimTime <= 0) {
            //    atk.aimTime = atk.attkTime - defaultWarnTime;
            //}
            last = atk;
        }

        //attks.AddRange(list);
    }


    void FixedUpdate() {
        if (attks.Count > 0) {
            if (source.timeSamples >= attks[0].attkTime) {

                if (attks[0].isWarning) {
                    attks[0].atk.Aim(target.position);
                    attks[0].atk.Warn();
                }
                else {
                    attks[0].atk.Trigger();
                }
                attks.RemoveAt(0);
            }
            //else {
            //    if (source.timeSamples >= attks[0].aimTime && !attks[0].atk.enabled) {
            //        attks[0].atk.Aim(target.position);
            //    }
            //    untilNext = attks[0].attkTime - source.timeSamples;
            //}
        }
        else {
            this.enabled = false;
        }
    }
}
