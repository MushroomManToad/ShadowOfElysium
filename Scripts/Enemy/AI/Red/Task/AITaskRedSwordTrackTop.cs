using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedSwordTrackTop : AITask
{
    GameObject shortsword, longsword;
    int timer;
    bool dir = false;

    public AITaskRedSwordTrackTop(AIDataSet data, GameObject shortsword, GameObject longsword) : base(data)
    {
        this.shortsword = shortsword;
        this.longsword = longsword;

        timer = getDuration();
    }

    public override void runAction()
    {
        if(timer == getDuration())
        {
            dir = Random.value > 0.5f;
            // The Render Line
            if (getCAI() != null) getCAI().sendMoveCode(MoveCode.TOP_SWORD_SPAWN, false, dir ? 1 : 0);
        }
        int f = dir ? -1 : 1;
        // Heck math, man.
        switch(getDuration() - timer - 34)
        {
            case 0:
                spawnSwordAt(f * -4.5f);
                break;
            case 4:
                spawnSwordAt(f * -3.0f);
                break;
            case 9:
                spawnSwordAt(f * -1.5f);
                break;
            case 14:
                spawnSwordAt(0.0f);
                break;
            case 19:
                spawnSwordAt(f * 1.5f);
                break;
            case 24:
                spawnSwordAt(f * 3.0f);
                break;
            case 29:
                spawnSwordAt(f * 4.5f);
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
