using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointMvmtCtrl : MvmtCtrl
{
    [SerializeField] Camera cam;
    Vector3 dirInput;

    Ray pointRay;
    RaycastHit hitInfo;

    public override Vector3 GetDir() {
        pointRay = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(pointRay, out hitInfo)) {
            dirInput = hitInfo.point - transform.position;
            dirInput.y = 0;
            return dirInput;
        }
        else return Vector3.zero;
    }

    public override bool GetSpecial1() {
        return Input.GetMouseButtonDown(0);
    }

    public override bool GetSpecial2() {
        return Input.GetMouseButton(1);
    }
}
