using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMvmtCtrl : MvmtCtrl
{
    [SerializeField] Transform cam;
    Vector3 dirInput;

    public override Vector3 GetDir()
    {
        dirInput =
            cam.forward * Input.GetAxisRaw("Vertical") +
            cam.right * Input.GetAxisRaw("Horizontal");

        //Project the directioal input vector onto horizontal plane.
        dirInput.y = 0f;
        //When normalized, this vector represents the desired direction to move in.
        return dirInput;
    }

    public override bool GetSpecial1()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool GetSpecial2() {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}
