using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAttackTimer : MonoBehaviour {
    //Audio source to follow.
    [SerializeField] AudioSource source;
    //Object to target (player).
    [SerializeField] Transform target;

    //Treated as a stack.
    [SerializeField] List<TimedAttack> attks;
    //Not tied to any functionality, only to be able to see in the inspector how long until the next event.
    [SerializeField] int untilNext;


    void Start() {
        //TO ADD ATTACKS
        //List<TimedAttack> list = new List<TimedAttack>();

        TimedAttack last = null;
        short i = 0;

        foreach (TimedAttack atk in attks) {
            //TO ADD MORE ATTACK TO LIST
            /*
            if (i > 171 && i < 198) {
                TimedAttack addAtk = new TimedAttack(atk);
                addAtk.time += 1627200;
                list.Add(addAtk);

                atk.time += 4800;
            }
            */

            //To ensure that attacks are placed in an ascending time order.
            if (last != null && atk.time <= last.time) {
                Debug.LogWarning("Invalid timed attack ordering at " + i + $"\n{atk.time} vs {last.time}");
            }
            i++;
            last = atk;
        }

        //TO ADD ATTACKS
        //attks.AddRange(list);
    }


    void FixedUpdate() {
        //If there's no more attacks to play, disable the component.
        //(this will stop the update methods from being called)
        if (attks.Count <= 0) {
            this.enabled = false;
            return;
        }

        //Only to show in editor.
        untilNext = attks[0].time - source.timeSamples;

        //If attack time was reached by source, play it.
        if (source.timeSamples >= attks[0].time) {
            //If warning, position the attack and telegraph it.
            if (attks[0].isWarning) {
                attks[0].atk.Aim(target.position);
                attks[0].atk.Warn();
            }
            //If actual attack, trigger it.
            else {
                attks[0].atk.Trigger();
            }

            //Pop the played attack from the stack.
            attks.RemoveAt(0);
        }
    }
}
