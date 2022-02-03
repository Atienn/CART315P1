using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

[RequireComponent(typeof(Light))]
public class LightFader : MonoBehaviour
{
    Light fLight;

    [SerializeField] [Range(0f, 1f)] float maxFlicker;
    [SerializeField] [Range(0f, 1f)] float delta;

    [SerializeField] bool startHidden;

    float maxIntensity;
    float currentIntensity;

    //Strategy pattern, I hate using if statements.
    VoidStrategy RampStrategy;


    void Start()
    {
        fLight = GetComponent<Light>();
        RampStrategy = StandardMethods.None;

        maxIntensity = fLight.intensity;
        //Scale delta as for it to be independent of target intensity.
        delta *= maxIntensity;
        
        if(startHidden)
        {
            currentIntensity = 0;
            fLight.intensity = 0;

            //Hide the light.
            fLight.enabled = false;
            //Prevents update functions from being called.
            this.enabled = false;
        }
    }

    void FixedUpdate() {
        //Ensures the ramp up/down is at fixed speed.
        RampStrategy();
    }

    void Update() {
        //Only modify light intensity if the change will be visible.
        fLight.intensity = currentIntensity - Random.Range(0, maxFlicker * currentIntensity);
    }


    //Public methods to be invoked from events.
    public void FadeIn()
    {
        //Show the light.
        fLight.enabled = true;
        //Allows update function to take effect.
        this.enabled = true;

        RampStrategy = RampUp;
    }
    public void FadeOut() {
        RampStrategy = RampDown;
    }


    //Ramp strategies.
    void RampUp() {
        currentIntensity += delta;

        if(currentIntensity >= maxIntensity) {
            currentIntensity = maxIntensity;
            RampStrategy = StandardMethods.None;
        }
    }
    void RampDown()
    {
        currentIntensity -= delta;

        if (currentIntensity <= 0) {
            currentIntensity = maxIntensity;
            RampStrategy = StandardMethods.None;

            fLight.enabled = false;
            this.enabled = false;
        }
    }
}
