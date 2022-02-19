using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EncounterEvent : MonoBehaviour
{
    private bool isFinished = false;
    private bool isActive = false;

    private void FixedUpdate()
    {
        if(isActive && !isFinished)
        {
            runEvent();
            if(finishCondition())
            {
                finish();
            }
        }
    }

    public void finish()
    {
        isFinished = true;
        isActive = false;
        postEvent();
        EEventManager.removeEvent(this);
    }

    public void run()
    {
        EEventManager.addEvent(this);
        preEvent();
        isActive = true;
    }

    public abstract void preEvent();
    public abstract void runEvent();
    public abstract void postEvent();
    public abstract bool finishCondition();
    public abstract bool receiveInteract();
}
