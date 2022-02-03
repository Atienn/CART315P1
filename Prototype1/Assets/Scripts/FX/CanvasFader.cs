using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    Canvas canvas;
    CanvasGroup canvGroup;

    [SerializeField] [Range(0f, 1f)] float delta;
    [SerializeField] bool startHidden;

    float maxAlpha;

    //Strategy pattern, I hate using if statements.
    VoidStrategy RampStrategy;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvGroup = GetComponent<CanvasGroup>();
        RampStrategy = StandardMethods.None;

        maxAlpha = canvGroup.alpha;

        if(startHidden) {
            canvas.enabled = false;
            canvGroup.alpha = 0;
        }
        this.enabled = false;
    }

    void FixedUpdate() {
        RampStrategy();
    }


    public void FadeIn()
    {
        this.enabled = true;
        canvas.enabled = true;
        RampStrategy = RampUp;
    }
    public void FadeOut()
    {
        this.enabled = true;
        RampStrategy = RampDown;
    }


    void RampUp()
    {
        canvGroup.alpha += delta;

        if(canvGroup.alpha >= maxAlpha) {
            canvGroup.alpha = maxAlpha;

            RampStrategy = StandardMethods.None;
            this.enabled = false;
        }
    }

    void RampDown()
    {
        canvGroup.alpha -= delta;

        if (canvGroup.alpha <= 0) {
            canvGroup.alpha = 0;

            RampStrategy = StandardMethods.None;
            this.enabled = false;
            canvas.enabled = false;
        }
    }
}
