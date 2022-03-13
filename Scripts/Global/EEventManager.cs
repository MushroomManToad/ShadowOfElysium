using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EEventManager
{
    public static ArrayList activeEvents = new ArrayList();

    private static BattleEvent activeBattle;

    /*
     * Btw, Dying needs to wipe all data in here.
     */

    public static void addEvent(EncounterEvent e)
    {
        activeEvents.Add(e);
    }

    public static void removeEvent(EncounterEvent e)
    {
        if(activeEvents.Contains(e))
        {
            activeEvents.Remove(e);
        }
    }

    public static bool contains(EncounterEvent e)
    {
        return activeEvents.Contains(e);
    }

    public static bool sendInteract()
    {
        foreach(EncounterEvent e in activeEvents)
        {
            if(e.receiveInteract())
            {
                return true;
            }
        }
        return false;
    }

    public static int size()
    {
        return activeEvents.Count;
    }

    public static void setBattleEvent(BattleEvent e)
    {
        activeBattle = e;
    }

    public static void endBattleEvent()
    {
        activeBattle.setIsFinished(true);
    }
}
