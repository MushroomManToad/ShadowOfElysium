using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrystalAI : EnemyAI
{
    public CrystalMover crystal;

    /*
     * Once HP reaches (or drops below) 0, the crystal breaks. Taking another hit ends the battle. (a few second crystal i-frame window to play the breaking animation)
     */
    private bool broken = false;

    /*
     * For basic commands that require no Ops.
     */
    public void sendMoveCode(MoveCode code, bool append)
    {
        crystal.sendInstruction(code, append, new float[0]);
    }

    /*
     * For commands requiring ops.
     */
    public void sendMoveCode(MoveCode code, bool append, params float[] ops)
    {
        crystal.sendInstruction(code, append, ops);
    }

    /*
     * Keep the crystal moving in between attack animations.
     */
    protected override void subclassRenderer()
    {
        if(crystal.getPositionUpdates().Count == 0)
        {
            sendMoveCode(defaultMove(), false);
        }
    }

    public override void damage(float amount)
    {
        /*
         * Deal the damage
         */
        if(!broken)
        {
            base.damage(amount);
            if (getCurrHealth() <= 0)
            {
                broken = true;
                startBreakAnim();
            }
        }
        /*
         * Once damage makes it through when the Crystal is broken, the battle should end. Exact way it ends depends on whether or not the crystal was struck with Red light.
         */
        else
        {

        }
    }

    /*
     * Applies I-frames and does the breaking animation before the fight's finale.
     */
    protected void startBreakAnim()
    {

    }

    protected abstract MoveCode defaultMove();
}
