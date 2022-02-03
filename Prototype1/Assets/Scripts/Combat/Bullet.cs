using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 vel;
    short timer;

    void Awake() {
        timer = 100;
    }

    void FixedUpdate() {
        if(timer-- <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        GameObject.Destroy(this.gameObject);
    }
}
