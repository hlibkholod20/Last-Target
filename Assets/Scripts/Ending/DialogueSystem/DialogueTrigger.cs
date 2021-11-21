using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager context;

    private void Awake()
    {
        if (this.tag != ("Final Dialogue"))
        { 
            TriggerDialogue();
        }
    }

    private void Update()
    {
        if (this.tag == "Final Dialogue" && PlayLastLevel.getDecision())
        {
            TriggerDialogue();
            PlayLastLevel.changeDecision();
        }
    }

    public void TriggerDialogue()
    {
        context.StartDialogue(dialogue);
    }
}
