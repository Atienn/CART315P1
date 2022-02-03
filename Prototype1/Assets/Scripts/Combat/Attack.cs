using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;
using System;

public class Attack : MonoBehaviour
{
    static Color noneColor = new Color(0f, 0f, 0f, 0f);
    static Color attkColor = new Color(1f, 0f, 0f, 1f);
    static Color warnColor = new Color(1f, 0.5f, 0f, 1f);

    public enum AimMode {
        None,
        LookAt,
        MoveTo
    }

    [SerializeField] AimMode aimMode;
    VectorInStrategy AimStrat;

    const float attkFade = 0.025f;
    const float warnFade = 0.025f;

    Collider[] hitBox;
    MeshRenderer[] render;

    float alpha;

    VoidStrategy fadeStrat;


    void Start() {
        hitBox = GetComponentsInChildren<Collider>();
        render = GetComponentsInChildren<MeshRenderer>();

        fadeStrat = StandardMethods.None;
        if(this.aimMode == AimMode.LookAt) {
            this.AimStrat = LookAtAim;
        }
        else if (this.aimMode == AimMode.MoveTo) {
            this.AimStrat = MoveToAim;
        }
        else {
            this.AimStrat = NoAim;
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
    public void Trigger() {
        Enable(Attack.attkColor);
        fadeStrat = AttkFade;
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
}

[Serializable]
class TimedAttack {
    public Attack atk;
    //public long aimTime;
    public long attkTime;
    public bool isWarning;

    public TimedAttack(TimedAttack ta) {
        this.atk = ta.atk;
        this.attkTime = ta.attkTime; 
        this.isWarning = ta.isWarning;
    } 
}
