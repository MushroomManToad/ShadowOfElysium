using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Instructions MUST be pushed to uPos in reverse order.
 * One way to push in order is to make a temporary stack 'q', push elements to that, then call pushToStack(uPos, q);
 */

public static class CrystalFunctions
{
    private static void pushToStack(Stack<CrystalFunctionEncoder> uPos, Stack<CrystalFunctionEncoder> sendingStack)
    {
        while(sendingStack.Count != 0)
        {
            uPos.Push(sendingStack.Pop());
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
    public static void lerp_to(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        if(ops.Length == 5)
        {
            // Vars
            float tx = ops[0];
            float ty = ops[1];
            float tf = ops[2];
            float af = ops[3];
            float df = ops[4];
            float sx = transform.position.x;
            float sy = transform.position.y;

            ArrayList tempList = new ArrayList();
            // Make sure the target frames is more than 1. Otherwise, just move it instantly.
            if (tf > 1)
            {
                // Compute Max velocities (mx) using a formula I found in Microsoft Whiteboard.
                float mx = (tx - sx) / ((1.0f / af) * (sum(1, (int)af - 1)) + (1.0f / df) * (sum(1, (int)df - 1)) + (tf - af - df + 2));
                float my = (ty - sy) / ((1.0f / af) * (sum(1, (int)af - 1)) + (1.0f / df) * (sum(1, (int)df - 1)) + (tf - af - df + 2));

                // Displacement after acceleration.
                float dxa = 0;
                float dya = 0;
                if(af > 1)
                {
                    for(int i = 1; i < af; i++)
                    {
                        tempList.Add(new CrystalFunctionEncoder(sx + ((float)sum(1, i) / af) * mx, sy + ((float)sum(1, i) / af) * my));
                    }

                    dxa = ((float)sum(1, (int)af - 1) / af) * mx; 
                    dya = ((float)sum(1, (int)af - 1) / af) * my;
                }
                if(tf - af - df + 2 > 0)
                {
                    for(int i = 0; i < tf - af - df + 2; i++)
                    {
                        tempList.Add(new CrystalFunctionEncoder(sx + dxa + i * mx, sy + dya + i * my));
                    }
                    dxa = dxa + (tf - af - df + 1) * mx;
                    dya = dya + (tf - af - df + 1) * my;
                }
                if (df > 1)
                {
                    for (int i = 1; i < df; i++)
                    {
                        tempList.Add(new CrystalFunctionEncoder(sx + dxa + ((float)sum((int)df - i, (int)df - 1) / df) * mx, sy + dya + ((float)sum((int)df - i, (int)df - 1) / df) * my));
                    }
                }
            }
            else
            {
                tempList.Add(new CrystalFunctionEncoder(tx, ty));
            }
            for (int i = (int)tf - 1; i >= 0; i--)
            {
                uPos.Push((CrystalFunctionEncoder)tempList[i]);
            }
        }
        else
        {
            Debug.LogError("Lerp called with illegal number of Arguments!");
        }
    }

    /* Moves the crystal along an Ellipse {x = acost, y = bsint}
     * ops[0] = frames
     * ops[1] = a
     * ops[2] = b
     * ops[3] = tMax
     * ops[4] = startingAngle
     */
    public static void ellipticalMotion(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        // Radians Per Frame
        float frames = ops[0];
        float a = ops[1];
        float b = ops[2];
        float tMax = ops[3];
        float startingAngle = ops[4];
        float rpf = tMax / frames;

        float initialX = transform.position.x - a * Mathf.Cos(startingAngle);
        float initialY = transform.position.y - b * Mathf.Sin(startingAngle);

        Stack<CrystalFunctionEncoder> q = new Stack<CrystalFunctionEncoder>((int)frames);

        for (int i = 0; i < frames; i++)
        {
            float t = i * rpf;

            q.Push(new CrystalFunctionEncoder(initialX + a * Mathf.Cos(startingAngle + t), initialY + b * Mathf.Sin(startingAngle + t)));
        }

        pushToStack(uPos, q);
    }

        /*
         * Takes no ops.
         */
        public static void start_center_bob(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        Stack<CrystalFunctionEncoder> q = new Stack<CrystalFunctionEncoder>(2);
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO_CENTER, true, 30));
        q.Push(new CrystalFunctionEncoder(MoveCode.CENTER_BOB, true));
        pushToStack(uPos, q);
    }

    /*
     * Takes no ops.
     */
    public static void center_bob(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        Stack<CrystalFunctionEncoder> q = new Stack<CrystalFunctionEncoder>(3);
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 6.5f, 70, 25, 25));
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 7.5f, 70, 25, 25));
        q.Push(new CrystalFunctionEncoder(MoveCode.CENTER_BOB, true));
        pushToStack(uPos, q); 
    }

    /*
     * Takes 1 op, the number of frames over which the return to center should occur.
     * Center is presumed to be (0.0f, 7.5f). If the board is not in a state such that this is the case, use lerp_to instead.
     */
    public static void lerp_to_center(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        /*
         * Not using the Queue System
         */
        if(ops.Length == 1)
        {
            bool upLeft = transform.position.y <= 6 && transform.position.x <= -5;
            bool upRight = transform.position.y <= 6 && transform.position.x >= 5;
            if(upRight || upLeft)
            {
                uPos.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 7.5f, ops[0] / 2.0f, ops[0] / 8.0f, ops[0] / 8.0f));
            }
            else
            {
                uPos.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 7.5f, ops[0], ops[0] / 2.0f, ops[0] / 2.0f));
            }
            // If the crystal is at board height but outside the bounds of the board, move it up the relevant side, then to the center.
            if (upLeft)
            {
                uPos.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, -6.5f, 7.5f, ops[0] / 2.0f, ops[0] / 8.0f, ops[0] / 8.0f));
            }
            else if(upRight)
            {
                uPos.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 6.5f, 7.5f, ops[0] / 2.0f, ops[0] / 8.0f, ops[0] / 8.0f));
            }
        }
        else
        {
            Debug.LogError("Lerp To Center called with illegal number of Arguments!");
        }
    }

    /*
     * Takes 1 op. Determines the number of frames over which the Crystal should maintain its position.
     */
    public static void crystal_wait(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        for(int i = 0; i < ops[0]; i++)
        {
            uPos.Push(new CrystalFunctionEncoder(transform.position.x, transform.position.y));
        }
    }

    // Rise up, smash the center, then linger there a moment.
    public static void center_smash(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        Stack<CrystalFunctionEncoder> q = new Stack<CrystalFunctionEncoder>(3);
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 9.0f, 50.0f, 25.0f, 15.0f));
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, 0.0f, 4.2f, 5.0f, 5.0f, 1.0f));
        q.Push(new CrystalFunctionEncoder(MoveCode.WAIT, true, 50.0f));
        pushToStack(uPos, q);
    }

    /* Out to the side, then swipe across the top in the direction given by ops.
     * Takes 1 op.
     * ops[0] -> 0 = left, else = right
     */
    public static void top_sword_spawn(Stack<CrystalFunctionEncoder> uPos, float[] ops, Transform transform)
    {
        Stack<CrystalFunctionEncoder> q = new Stack<CrystalFunctionEncoder>(3);
        int f = ops[0] == 0 ? 1 : -1;
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, f * -6.0f, 7.0f, 30.0f, 5.0f, 5.0f));
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, f * -4.5f, 7.0f, 5.0f, 4.0f, 1.0f));
        q.Push(new CrystalFunctionEncoder(MoveCode.LERP_TO, true, f * 6.0f, 7.0f, 35.0f, 1.0f, 5.0f));
        pushToStack(uPos, q);
    }

    // The sum function
    private static int sum(int minInclusive, int maxInclusive)
    {
        int result = 0;
        if(maxInclusive >= minInclusive)
        {
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                result += i;
            }
        }
        return result;
    }
}