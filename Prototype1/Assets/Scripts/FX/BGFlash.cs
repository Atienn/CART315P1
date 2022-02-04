using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFlash : Singleton<BGFlash>
{
    static Color flashColor = new Color(0.1f, 0f, 0f, 1f);
    MeshRenderer mesh; 
    const float fade = 0.005f;
    const string id = "_Tint";

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void Flash() {
        this.enabled = true;
        mesh.material.SetColor(id, flashColor);
    }

    void FixedUpdate()
    {
        mesh.material.SetColor(id, new Color(mesh.material.GetColor(id).r - fade, 0f, 0f, 1f));
        if(mesh.material.GetColor(id).r <= 0) {
            this.enabled = false;
        }
    }
}
