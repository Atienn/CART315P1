using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MvmtCtrl))]
[RequireComponent(typeof(CharacterController))]
public class Gunship : MonoBehaviour
{
    [SerializeField] MvmtCtrl mvmtCtrl;
    [SerializeField] float speed;

    [SerializeField] Bullet bulletTemplate;
    Vector3 fireOffset;

    [SerializeField] LayerMask canPoint;
    CharacterController charCtrl;

    Ray mousePosRay;
    RaycastHit hitInfo;

    Vector3 dirInput;
    Vector3 velocity;


    bool isAiming;
    float fireRate;


    // Start is called before the first frame update
    void Start() {
        isAiming = false;
        charCtrl = GetComponent<CharacterController>();

        fireOffset = this.transform.up * -this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update() {

        dirInput = mvmtCtrl.GetDir();

        if (mvmtCtrl.GetSpecial2()) {
            isAiming = true;
        }
        else {
            isAiming = false;
        }
    }
    void FixedUpdate() {

        if (dirInput.magnitude > 0.01f) {
            transform.forward = dirInput;
        }

        if (isAiming) {
            GameObject.Instantiate<Bullet>(bulletTemplate, this.transform.position + this.fireOffset, Quaternion.identity)
                .rb.velocity = this.transform.forward * 500f;
        }
        else {
            if (dirInput.magnitude > speed) {
                charCtrl.Move(dirInput.normalized * speed);
            }
            else {
                charCtrl.Move(dirInput);
            }
        }
    }
}