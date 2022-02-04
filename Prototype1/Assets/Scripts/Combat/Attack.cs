using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

public class Attack : MonoBehaviour
{
    static Color noneColor = new Color(0f, 0f, 0f, 0f);
    static Color attkColor = new Color(1f, 0f, 0f, 1f);
    static Color warnColor = new Color(1f, 0.5f, 0f, 1f);
    static Color hitcColor = new Color(0.5f, 1f, 0f, 1f);


    public enum AimMode {
        None,
        LookAt,
        MoveTo,
        MoveRandom
    }

    [SerializeField] AimMode aimMode;
    VectorInStrategy AimStrat;

    const float attkFade = 0.025f;
    const float warnFade = 0.025f;

    Collider[] hitBoxes;
    MeshRenderer[] render;

    float alpha;

    VoidStrategy fadeStrat;


    void Start() {
        hitBoxes = GetComponentsInChildren<Collider>();
        render = GetComponentsInChildren<MeshRenderer>();

        fadeStrat = StandardMethods.None;

        switch(this.aimMode) {
            case AimMode.LookAt:
                this.AimStrat = LookAtAim;
                break;

            case AimMode.MoveTo:
                this.AimStrat = MoveToAim;
                break;

            case AimMode.MoveRandom:
                this.AimStrat = MoveToRand;
                break;

            default:
                this.AimStrat = NoAim;
                break;
        }

        ChangeColor(noneColor);
        SetRender(false);
        this.enabled = false;
    }


    public void Aim(Vector3 dir) {
        AimStrat(dir);
        //Enable(Attack.warnColor);
        //fadeStrat = WarnFade;
    }

    public void Warn() {
        Enable(Attack.warnColor);
        fadeStrat = WarnFade;
    }
    public bool Trigger() {

        Enable(Attack.attkColor);
        fadeStrat = AttkFade;

        //Check each hitbox against each entity.
        foreach(Entity target in EntityManager.Instance.all) {
            foreach (Collider hitBox in hitBoxes) {
                //If the closest point within the collider to the 
                //target is the target itself, then the attack hit.
                if (hitBox.ClosestPoint(target.transform.position) == target.transform.position) {
                    target.OnHit();
                    return true;
                }
            }
        }

        return false;
    }


    void FixedUpdate() { fadeStrat(); }

    void WarnFade() {
        alpha -= warnFade;
        if (alpha < 0) {
            fadeStrat = StandardMethods.None;
            alpha = 0;
        }
        else { ChangeColor(new Color(warnColor.r, warnColor.g, warnColor.b, alpha)); }
    }
    void AttkFade() {
        alpha -= attkFade;
        if (alpha < 0) { Disable(); }
        else { ChangeColor(new Color(attkColor.r, attkColor.g, attkColor.b, alpha)); }
    }
    //Hit confirm fade, unused.
    void HitCFade()
    {
        alpha -= attkFade;
        if (alpha < 0) { Disable(); }
        else { ChangeColor(new Color(hitcColor.r, hitcColor.g, hitcColor.b, alpha)); }
    }


    void Enable(Color clr) {
        SetRender(true);
        this.enabled = true;
        ChangeColor(clr);
        alpha = clr.a;
    }
    void Disable() {
        this.enabled = false;
        SetRender(false);
        fadeStrat = StandardMethods.None;
        alpha = 0;
    }


    void ChangeColor(Color clr) {
        foreach (MeshRenderer mesh in render) {
            mesh.material.color = clr;
        }
    }
    void SetRender(bool active) {
        foreach(MeshRenderer mesh in render) {
            mesh.enabled = active;
        }
    }


    void NoAim(Vector3 target) { }
    void LookAtAim(Vector3 target) {
        this.transform.forward = (this.transform.position - target).normalized;
    }
    void MoveToAim(Vector3 target) {
        transform.position = target;
    }
    void MoveToRand(Vector3 target) {
        Vector3 rand = UnityEngine.Random.onUnitSphere;
        rand.y = 0f;
        transform.localPosition = rand * Mathf.Pow(UnityEngine.Random.Range(5f, 10f), 2);
    }
}

//Allows class to be displayed/edited in inspector.
[Serializable]
class TimedAttack {
    public Attack atk;
    public int time;
    public bool isWarning;

    public TimedAttack(TimedAttack ta) {
        this.atk = ta.atk;
        this.time = ta.time;
        this.isWarning = ta.isWarning;
    } 
}
