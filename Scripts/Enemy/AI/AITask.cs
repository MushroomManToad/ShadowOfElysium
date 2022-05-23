using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITask
{
    // THIS IS A STATIC INT. PLEASE DO NOT REDUCE THIS. THIS IS USED IN THE ENEMYAI CLASS!
    private int duration;
    // Use this instead. It decrements each frame.
    private int timer;
    private float damage, spawnVel;
    private EnemyAI ai;
    private GameObject player, splitter, proj;


    public AITask(AIDataSet set)
    {
        this.duration = set.getDuration();
        this.ai = set.getAI();
        this.damage = set.getDamage();
        this.spawnVel = set.getSpawnVel();
        this.player = set.getPlayer();
        this.splitter = set.getSplitter();
        this.proj = set.getProj();
    }

    /* A method that handles everything that should be done frame 1 of the AITask being run.
     * Called when AITask is chosen, so before ANYTHING else can even be run.
     * Includes:
     * Setting a decrementing timer to the total duration.
     * Sending the initial MoveCode(s) to the CrystalMover
     */
    public virtual void firstFrameUpdate()
    {
        timer = getDuration();
        setFirstFrameRender();
    }

    /* A method that handles things the attack should do each frame. DO NOT USE in place of run action, which controls the AI's actual behavior.
     * This is for behind-the-scenes things, like decrementing the timer each frame. Called AFTER each frame. (So the first task frame will have timer = getDuration)
     */
    public virtual void frameUpdate()
    {
        timer--;
    }

    // Condition must be met, or the task will be skipped when attempted to be chosen.
    public abstract bool runCondition();
    // Returns true when the attack should end early. Can return false to never interrupt attack.
    public abstract bool endCondition();
    // Runs every frame while the task is active.
    public abstract void runAction();
    // Runs when the task is finished, one frame after the last runAction.
    public abstract void endAction();
    // Place the MoveCode(s) to enque here. Should be called at the end of firstFrameUpdate(), although can be moved earlier at implementer's disgression.
    public abstract void setFirstFrameRender();

    /** The total time the Task runs for */
    public virtual int getDuration() { return duration; }
    /** The time remaining for the task */
    public virtual int getTimer() { return timer; }
    public virtual float getDamage() { return damage; }
    public virtual float getSpawnVel() { return spawnVel;}

    public virtual EnemyAI getAI() { return ai; }
    public virtual GameObject getPlayer() { return player; }
    public virtual GameObject getSplitter() { return splitter; }
    public virtual GameObject getProj() { return proj; }

    // Crystal AI Util. Returns null if the attached EnemyAI is not a CrystalAI
    public CrystalAI getCAI()
    {
        if (getAI() is CrystalAI)
        {
            return (CrystalAI) getAI();
        }
        return null;
    }
}
