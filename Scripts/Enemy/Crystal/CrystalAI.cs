using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrystalAI : EnemyAI
{
    public CrystalMover crystal;

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

    protected abstract MoveCode defaultMove();
}
