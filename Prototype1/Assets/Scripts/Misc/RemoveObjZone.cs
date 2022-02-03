using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjZone : MonoBehaviour
{
    //Destroys any gameobject that falls into the trigger zone.
    private void OnTriggerEnter(Collider other) {
        GameObject.Destroy(other.gameObject);
    }
}
