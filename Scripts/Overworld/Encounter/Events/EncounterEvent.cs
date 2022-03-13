using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EncounterEvent : MonoBehaviour
{
    private bool isFinished = false;
    private bool isActive = false;
    /*
     * Every Event in the game is an EncounterEvent. 
     * The event runs as follows:
     * Begin event -> Add event to the EEventManager, run the preEvent, then set isActive to true and isFinished to false.
     * Run event -> While isActive remains true, run the event every physics frame.
     *              Check each frame for whether or not the event's finish condition has been satisfied. If it has been, end the event by calling finish().
     * Finish event -> Set isFinished to true and isActive to false. Call postEvent, then remove the event from EEventManager.
     * 
     * These events can also recieve Interacts, Left inputs, and Right inputs. 
     */
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
        isFinished = false;
    }

    public bool getIsFinished()
    {
        return isFinished;
    }

    public abstract void preEvent();
    public abstract void runEvent();
    public abstract void postEvent();
    public abstract bool finishCondition();
    public virtual bool receiveInteract() { return false; }
    public virtual bool receiveLeft() { return false; }
    public virtual bool receiveRight() { return false; }
}
