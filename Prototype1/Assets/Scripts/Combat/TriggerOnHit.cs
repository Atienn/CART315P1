using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnHit : MonoBehaviour
{
    [SerializeField] Entity target;

    private void OnTriggerEnter(Collider other) {
        foreach (Entity entity in EntityManager.Instance.all) {
            if (other.gameObject.Equals(target.gameObject)) {
                target.OnHit();
            }
        }
    }
}
