using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrystalFunctions
{
    /*
     * Takes no ops.
     */
    public static void start_center_bob(Queue<CrystalFunctionEncoder> uPos, float[] ops)
    {
        uPos.Enqueue(new CrystalFunctionEncoder(MoveCode.LERP_TO_CENTER, true, 10));
        uPos.Enqueue(new CrystalFunctionEncoder(MoveCode.CENTER_BOB, true));
    }

    /*
     * Takes no ops.
     */
    public static void center_bob(Queue<CrystalFunctionEncoder> uPos, float[] ops)
    {

        for (float i = 0; i < 10; i++)
        {
            uPos.Enqueue(new CrystalFunctionEncoder(0, 7.5f - (i / 10.0f)));
        }
        uPos.Enqueue(new CrystalFunctionEncoder(MoveCode.CENTER_BOB, true));
    }

    /*
     * Takes 1 op, the number of frames over which the return to center should occur.
     * Center is presumed to be (0.0f, 7.0f). If the board is not in a state such that this is the case, use lerp_to instead.
     */
    public static void lerp_to_center(Queue<CrystalFunctionEncoder> uPos, float[] ops)
    {
        if(ops.Length == 1)
        {
            float[] send = { 0.0f, 7.0f, ops[0], ops[0] / 4.0f, ops[0] / 4.0f };
            lerp_to(uPos, send);
        }
        else
        {
            Debug.LogError("Lerp To Center called with illegal number of Arguments!");
        }
    }

    /*
     * Takes 5 ops.
     * ops[0] -> TargetX
     * ops[1] -> TargetY
     * ops[2] -> frames
     * ops[3] -> startAccelFrames
     * ops[4] -> endAccelFrames
     * 
     * Lerps the crystal to a specified position at a specified rate. Probably the most important function in this whole animation set.
     * Used by most other animations at some level.
     * This function should *ONLY* create uPos POSITIONS. 
     */
    public static void lerp_to(Queue<CrystalFunctionEncoder> uPos, float[] ops)
    {

    }
}