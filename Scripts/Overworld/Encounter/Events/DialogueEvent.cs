using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : EncounterEvent
{
    public string[] dialogue;
    private int currDialogue = 0;
    private int activeDialogue = -1;
    public OPlayerController player;

    public override void preEvent()
    {
        // Load Dialogue Box in player UI
        player.setFreezeInputs(true);
    }

    public override void runEvent()
    {
        if(activeDialogue != currDialogue && currDialogue < dialogue.Length)
        {
            DialogueData data = DialogueManager.getDialogueByID(dialogue[currDialogue]);
            // TODO -- render this instead of in console.
            Debug.Log(data.getTextByKey(EnumDialogueKey.EN));
            activeDialogue = currDialogue;
        }
    }

    public override void postEvent()
    {
        // Remove DialogueBox
        player.setFreezeInputs(false);
        currDialogue = 0;
        activeDialogue = -1;
    }

    public override bool receiveInteract()
    {
        advanceDialogue();
        return true;
    }

    public override bool finishCondition()
    {
        return currDialogue >= dialogue.Length;
    }

    private bool advanceDialogue()
    {
        currDialogue++;
        return true;
    }
}
