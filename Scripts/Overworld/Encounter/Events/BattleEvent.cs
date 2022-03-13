using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvent : EncounterEvent
{
    public Encounter encounter;
    public GameObject oCam, oPlayer;

    private bool battleFinished = false;

    public override void preEvent()
    {
        encounter.StartEncounter(oPlayer, oCam);
    }

    public override void runEvent()
    {
        // Can't really think of anything that needs to go here.
    }

    public override void postEvent()
    {
        // Battle should reset itself, so just call encounter.FinishEncounter();
        battleFinished = false;
        encounter.finishEncounter();
    }

    public override bool finishCondition()
    {
        return battleFinished;
    }

    /*
     * This should only be called by EEventManager.
     */
    public void setIsFinished(bool val)
    {
        battleFinished = val;
    }
}
