using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedSwordTrackTop : AITask
{
    GameObject shortsword, longsword;
    int timer;

    public AITaskRedSwordTrackTop(AIDataSet data, GameObject shortsword, GameObject longsword) : base(data)
    {
        this.shortsword = shortsword;
        this.longsword = longsword;

        timer = getDuration();
    }

    public override void runAction()
    {
        // Heck math, man.
        switch(getDuration() - timer)
        {
            case 0:
                spawnSwordAt(-4.5f);
                break;
            case 4:
                spawnSwordAt(-3.0f);
                break;
            case 9:
                spawnSwordAt(-1.5f);
                break;
            case 14:
                spawnSwordAt(0.0f);
                break;
            case 19:
                spawnSwordAt(1.5f);
                break;
            case 24:
                spawnSwordAt(3.0f);
                break;
            case 29:
                spawnSwordAt(4.5f);
                break;
            default:
                break;
        }
        timer--;
    }

    public override void endAction()
    {
        timer = getDuration();
    }

    public override bool endCondition()
    {
        return false;
    }

    public override bool runCondition()
    {
        if (getAI().getCurrHealth() / getAI().getMaxHealth() > 0.33f)
        {
            return true;
        }
        else return false;
    }

    private void spawnSwordAt(float xPos)
    {
        GameObject sword;
        if ((float)base.getAI().getCurrHealth() / (float)base.getAI().getMaxHealth() > 0.66f)
        {
            sword = MonoBehaviour.Instantiate(shortsword);
        }
        else
        {
            sword = MonoBehaviour.Instantiate(longsword);
        }
        if (sword != null && sword.GetComponent<TrackerSword>() != null)
        {
            sword.transform.position = new Vector3(xPos, 7, 0);
            TrackerSword ts = sword.GetComponent<TrackerSword>();
            ts.setColor(Color.RED, false);
            ts.setDamage(getDamage());
            ts.setLifeTime(200);
            ts.setSpeed(getSpawnVel());
            ts.setTargetPlayer(getPlayer());
            ts.setTrackingTimer(35);
        }
    }
}
