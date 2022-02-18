using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : EncounterEvent
{
    public string[] dialogue;
    private int currDialogue = 0;
    public OPlayerController player;

    public override void preEvent()
    {
        // Load Dialogue Box in player UI
    }

    public override void runEvent()
    {
        if(currDialogue < dialogue.Length)
        {
            // TODO if a new input has been received. 
            Debug.Log(DialogueManager.getDialogueByID(dialogue[currDialogue]));
            // Set that a new input was used.
            // 
        }
    }

    public override void postEvent()
    {
        
    }

    public override bool finishCondition()
    {
        return currDialogue >= dialogue.Length;
    }
}
