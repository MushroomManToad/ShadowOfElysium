using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITaskRedSwordOmnihit : AITask
{
    private GameObject longSword;
    private int breakDur, ssT, num;
    private bool nextState = false;

    public AITaskRedSwordOmnihit(AIDataSet data, GameObject longSword, int breakDur) : base(data)
    {
        this.longSword = longSword;
        this.breakDur = breakDur;
        nextState = Random.Range(0.0f, 1.0f) < 0.5f ? true : false;
    }

    public override void firstFrameUpdate()
    {
        nextState = Random.Range(0.0f, 1.0f) < 0.5f ? true : false;
        ssT = 0;
        num = 0;
        base.firstFrameUpdate();
    }

    public override void setFirstFrameRender()
    {
        throw new System.NotImplementedException();
    }

    public override void runAction()
    {
        if (getDuration() - getTimer() < 31)
        {
            if (ssT == 0)
            {
                spawnSwordAt((float) num * 1.5f + -4.5f, 7.0f);
                num++;
                ssT = 4;
            }
            else ssT--;
        }
        else if (getDuration() - getTimer() - breakDur < 31 && getDuration() - getTimer() - breakDur >= 0)
        {
            if (nextState)
            {
                if (ssT == 0)
                {
                    spawnSwordAt(-7.0f, (float)(num - 7) * 1.5f + -4.5f);
                    num++;
                    ssT = 4;
                }
                else ssT--;
            }
            else
            {
                if (ssT == 0)
                {
                    spawnSwordAt(7.0f, (float)(num - 7) * 1.5f + -4.5f);
                    num++;
                    ssT = 4;
                }
                else ssT--;
            }
        }
        else if (getDuration() - getTimer() - breakDur * 2 < 31 && getDuration() - getTimer() - breakDur * 2 >= 0)
        {
            if (nextState)
            {
                if (ssT == 0)
                {
                    spawnSwordAt(7.0f, (float)(num - 14) * 1.5f + -4.5f);
                    num++;
                    ssT = 4;
                }
                else ssT--;
            }
            else
            {
                if (ssT == 0)
                {
                    spawnSwordAt(-7.0f, (float)(num - 14) * 1.5f + -4.5f);
                    num++;
                    ssT = 4;
                }
                else ssT--;
            }
        }
        else if(ssT != 0)
        {
            ssT = 0;
        }
    }

    public override void endAction()
    {
    }

    public override bool endCondition()
    {
        return false;
    }

    public override bool runCondition()
    {
        if (getAI().getCurrHealth() / getAI().getMaxHealth() <= 0.33f)
        {
            return true;
        }
        else return false;
    }

    private void spawnSwordAt(float x, float y)
    {
        GameObject sword = MonoBehaviour.Instantiate(longSword);
        if (sword != null && sword.GetComponent<TrackerSword>() != null)
        {
            sword.transform.position = new Vector3(x, y, 0);
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
