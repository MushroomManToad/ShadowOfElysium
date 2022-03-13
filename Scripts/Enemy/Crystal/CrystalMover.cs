using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMover : MonoBehaviour
{
    private Queue<CrystalFunctionEncoder> uPos = new Queue<CrystalFunctionEncoder>();
    /*
     * New MoveCodes must be given functionality here, and added to the enum MoveCode.
     */
    public void sendInstruction(MoveCode code, bool append, float[] ops)
    {
        if(!append)
        {
            uPos.Clear();
        }
        switch (code)
        {
            case (MoveCode.START_CENTER_BOB):
                CrystalFunctions.start_center_bob(uPos, ops);
                break;
            case (MoveCode.CENTER_BOB):
                CrystalFunctions.center_bob(uPos, ops);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        /*
         * Each frame, move the crystal to the next frame in its animation instructions.
         */
        if (uPos.Count > 0)
        {
            // Run the current instruction.
            CrystalFunctionEncoder cfe = uPos.Dequeue();
            cfe.runCFE(this);

            // Peek at the next instruction. If it's an INSTRUCTION type, dequeue it and add its positions to the array so that we don't lose frames.
            if(uPos.Count > 0 && uPos.Peek().checkType() == CrystalFunctionEncoder.CFEType.INSTRUCTION)
            {
                CrystalFunctionEncoder cfi = uPos.Dequeue();
                cfi.runCFE(this);
            }
        }
    }

    public Queue<CrystalFunctionEncoder> getPositionUpdates()
    {
        return uPos;
    }
}
