using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITask
{
    // THIS IS A STATIC INT. PLEASE DO NOT REDUCE THIS. THIS IS USED IN THE ENEMYAI CLASS!
    private int duration;
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

    public abstract bool runCondition();
    public abstract bool endCondition();
    public abstract void runAction();
    public abstract void endAction();

    public virtual int getDuration() { return duration; }
    public virtual float getDamage() { return damage; }
    public virtual float getSpawnVel() { return spawnVel;}

    public virtual EnemyAI getAI() { return ai; }
    public virtual GameObject getPlayer() { return player; }
    public virtual GameObject getSplitter() { return splitter; }
    public virtual GameObject getProj() { return proj; }
}
