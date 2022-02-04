using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MvmtCtrl))]
[RequireComponent(typeof(CharacterController))]
public class Gunship : Entity
{
    [Header("Movement")]
    [SerializeField] MvmtCtrl mvmtCtrl;
    [SerializeField] float speed;

    CharacterController charCtrl;
    Vector3 dirInput;
    Vector3 velocity;


    [Header("Combat")]
    [SerializeField] Bullet bulletTemplate;
    [SerializeField] float bulletSpeed = 750f;

    [SerializeField] byte maxFireDelay;
    [SerializeField] byte minFireDelay;
    [SerializeField] byte hurtDelay;

    Vector3 fireOffset;
    byte fireDelay;
    byte fireCurrent;
    byte hurtCurrent;
    bool isAiming;


    [Header("Other")]
    [SerializeField] AudioSource hurtSource;
    [SerializeField] LayerMask canPoint;
    int score;


    // Start is called before the first frame update
    void Start() {
        EntityManager.Instance.all.Add(this);
        charCtrl = GetComponent<CharacterController>();

        fireOffset = this.transform.up * -this.transform.localScale.y;

        fireDelay = maxFireDelay;
        fireCurrent = 0;
        hurtCurrent = 0;
        isAiming = false;

        score = 0;
    }


    void Update() {
        //Get inputs.
        dirInput = mvmtCtrl.GetDir();
        isAiming = mvmtCtrl.GetSpecial2();
    }
    void FixedUpdate() {
        //Face towards movement if it's not minuscule.
        if (dirInput.magnitude > 0.001f) {
            transform.forward = dirInput;
        }

        //If lowering hurtDelay by 1 makes it 0, remove the music modifier.
        if (hurtCurrent > 0) { 
            if (--hurtCurrent == 0) { AudioModif.Instance.RemoveAudioModif(); }
        }
        //If hurtDelay is 0 (not hurt), increase score.
        else { ++score; }

        //If aiming, fire bullets.
        if (isAiming) {
            if(fireCurrent-- <= 0) {
                //Create a bullet.
                Bullet b = GameObject.Instantiate<Bullet>(bulletTemplate, this.transform.position + this.fireOffset, Quaternion.identity);
                b.firedFrom = this;
                //Make it fly in the direction the ship is facing.
                b.rb.velocity = this.transform.forward * bulletSpeed;

                //Reduce the delay between shots (increase fire rate as the gun is fired).
                if(fireDelay > minFireDelay) { --fireDelay; }
                //
                fireCurrent = fireDelay;
            }

            velocity *= 0.85f;
        }
        //If not aiming, move towards mouse position.
        else {
            if(fireDelay < maxFireDelay) { ++fireDelay; }

            if (dirInput.magnitude > speed) { velocity = dirInput.normalized * speed; }
            else { velocity = dirInput; }
        }
        
        charCtrl.Move(velocity);
    }


    public override void OnHit() {
        hurtCurrent = hurtDelay;
        //Play hurt sound.
        hurtSource.Play();
        //Add modifier to music.
        AudioModif.Instance.AddAudioModif();
    }

    public void BulletHit() {
        score += 2;
    }

    public void LogScore() {
        Debug.Log("SCORE: " + score);
    }
}