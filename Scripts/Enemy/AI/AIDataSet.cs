using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDataSet
{
    private EnemyAI ai;
    private GameObject player;
    private int duration;
    private float damage, spawnVel;
    private GameObject splitter;
    private GameObject proj;

    public AIDataSet(EnemyAI ai, GameObject player, int duration, float damage, float spawnVel, GameObject splitter, GameObject proj)
    {
        this.ai = ai;
        this.player = player;
        this.duration = duration;
        this.damage = damage;
        this.spawnVel = spawnVel;
        this.splitter = splitter;
        this.proj = proj;
    }

    public AIDataSet Clone()
    {
        return new AIDataSet(ai, player, duration, damage, spawnVel, splitter, proj);
    }

    public AIDataSet Clone(EnemyAI ai, GameObject player, int duration, float damage, float spawnVel, GameObject splitter, GameObject proj)
    {
        return new AIDataSet(ai, player, duration, damage, spawnVel, splitter, proj);
    }

    public EnemyAI getAI() { return ai; }
    public GameObject getPlayer() { return player; }
    public int getDuration() { return duration; }
    public float getDamage() { return damage; }
    public float getSpawnVel() { return spawnVel; }
    public GameObject getSplitter() { return splitter; }
    public GameObject getProj() { return proj; }
}
