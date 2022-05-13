using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMover : MonoBehaviour
{
    private Stack<CrystalFunctionEncoder> uPos = new Stack<CrystalFunctionEncoder>();
    public CrystalAI ai;
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
                CrystalFunctions.start_center_bob(uPos, ops, transform);
                break;
            case (MoveCode.CENTER_BOB):
                CrystalFunctions.center_bob(uPos, ops, transform);
                break;
            case (MoveCode.LERP_TO):
                CrystalFunctions.lerp_to(uPos, ops, transform);
                break;
            case (MoveCode.LERP_TO_CENTER):
                CrystalFunctions.lerp_to_center(uPos, ops, transform);
                break;
            case (MoveCode.CENTER_SLAM):
                CrystalFunctions.center_smash(uPos, ops, transform);
                break;
            case (MoveCode.WAIT):
                CrystalFunctions.crystal_wait(uPos, ops, transform);
                break;
            case (MoveCode.TOP_SWORD_SPAWN):
                CrystalFunctions.top_sword_spawn(uPos, ops, transform);
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
            // Peek at the next instruction(s). If it's an INSTRUCTION type, dequeue it and add their positions to the array so that we don't lose frames.
            while (uPos.Count > 0 && uPos.Peek().checkType() == CrystalFunctionEncoder.CFEType.INSTRUCTION)
            {
                CrystalFunctionEncoder cfi = uPos.Pop();
                cfi.runCFE(this);
            }
            // Run the current instruction.
            CrystalFunctionEncoder cfe = uPos.Pop();
            cfe.runCFE(this);
        }
    }

    public Stack<CrystalFunctionEncoder> getPositionUpdates()
    {
        return uPos;
    }

    public CrystalAI getCrystalAI()
    {
        return ai;
    }
}
