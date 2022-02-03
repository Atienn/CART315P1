using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    Dialogue data;
    [SerializeField] DialogueManager manager;
    void Start()
    {
        manager = DialogueManager.Instance;
    }

    private void Update()
    {
        string x = manager + "x";
    }
    // Start is called before the first frame update
    public void Trigger()
    {

    }
}
