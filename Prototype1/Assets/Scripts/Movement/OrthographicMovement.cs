using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicMovement : MonoBehaviour
{
    [SerializeField] CharacterController charCtrl;
    [SerializeField] MvmtCtrl mvmtCtrl;
    [SerializeField] Transform display;

    [Header("General")]
    [SerializeField] float speed = 4;

    [Header("Dash")]
    [SerializeField] float dashForce = 40;
    [SerializeField] byte dashCooldown = 30;

    Vector3 velocity;
    Vector3 dirInput;
    Vector3 targetRotation;

    bool isDashing;
    byte dashTimer;

    const float turnSmoothTime = 0.075f;
    float turnSmoothVel;


    // Start is called before the first frame update
    void Start()
    {
        isDashing = false;
        dashTimer = 0;

        velocity = Vector3.zero;
        dirInput = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            dirInput = mvmtCtrl.GetDir().normalized;

            if (mvmtCtrl.GetSpecial1())
            {
                isDashing = true;
                dashTimer = dashCooldown;

                velocity += dirInput * dashForce;
                dirInput = Vector3.zero;
            }
        }

    }

    void FixedUpdate()
    {
        if(!isDashing)
        {
            //Compute friction.
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, 0.075f * Mathf.Abs(velocity.x) + 0.25f);
            velocity.z = Mathf.MoveTowards(velocity.z, 0f, 0.075f * Mathf.Abs(velocity.z) + 0.25f);
        }


        //Compute dash cooldown if any.
        if (dashTimer > 0 && --dashTimer == 0) { isDashing = false; }

        if (dirInput != Vector3.zero)
        {
            targetRotation = dirInput;
            //Wouldn't matter if it was executed each frame, but this saves an operation.
            velocity += dirInput * speed;
        }

        //All of this is just for the character rotation to be smooth. It is purely for asthetics.
        display.rotation =
            Quaternion.Euler(
                0f,
                Mathf.SmoothDampAngle(
                    display.eulerAngles.y,
                    Mathf.Atan2(targetRotation.x, targetRotation.z) * Mathf.Rad2Deg,
                    ref turnSmoothVel,
                    turnSmoothTime),
                0f);

        if(velocity != Vector3.zero) { charCtrl.Move(velocity * Time.fixedDeltaTime); }
    }
}
