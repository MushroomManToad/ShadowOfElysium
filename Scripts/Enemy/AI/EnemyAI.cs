using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    public GameObject tPlayer;
    private AIState state = new AIState();

    protected ArrayList aiTaskS = new ArrayList();
    protected ArrayList aiTaskR = new ArrayList();
    protected ArrayList aiTaskF = new ArrayList();

    [SerializeField]
    private float startHealth;

    private float maxHealth, currHealth;

    // aiTaskS.add();
    protected abstract void AITaskSequence();

    // aiTaskR.add();
    protected abstract void AITaskRandom();

    // aiTaskF.add();
    protected abstract void AITaskForce();

    private void Start()
    {
        maxHealth = startHealth;
        currHealth = startHealth / 4;
        AITaskSequence();
        AITaskRandom();
        AITaskForce();
    }

    private void FixedUpdate()
    {
        renderEffects();
        preUpdate();
        runAITask();
        postUpdate();
    }


    /*
     * Manages the render thread in battle. First, do subclass effects (i.e. crystal movement), then attack-specific effects not tied to any specific object.
     */
    private void renderEffects()
    {
        subclassRenderer();
        aiRenderer();
    }

    protected virtual void subclassRenderer(){}

    protected virtual void aiRenderer(){}

    protected virtual void preUpdate()
    {

    }

    protected void runAITask()
    {
        if(state.getTimer() <= 0)
        {
            if(state.getAITask() != null) state.getAITask().endAction();
            state.setAITask(null);

            // This part is the algorithm that chooses the AI Task

            bool taskChosen = false;
            foreach (AITask task in aiTaskF)
            {
                if (!taskChosen)
                {
                    if (task != null && task.runCondition())
                    {
                        state.setAITask(task);
                        state.setTimer(task.getDuration());
                        taskChosen = true;
                    }
                }
            }
            if(!taskChosen)
            {
                if(state.getIsInSequence())
                {
                    // TODO : Sequence handler when state is in sequence.
                }
                else
                {
                    AITask chosenTask = chooseRandomTask();
                    if(chosenTask != null) taskChosen = true;
                }
            }
        }
        else
        {
            if (state.getAITask() != null)
            {
                AITask activeTask = state.getAITask();
                if(!activeTask.endCondition())
                {
                    activeTask.runAction();
                }
            }
            else state.setTimer(0);
        }
        state.setTimer(state.getTimer() - 1);
    }

    protected virtual void postUpdate()
    {

    }

    public Vector3 genPosOnEdge()
    {
        bool isXLocked = Random.Range(0.0f, 1.0f) > 0.5 ? true : false;
        float x = isXLocked ? (Random.Range(0.0f, 1.0f) > 0.5 ? 5.0f : -5.0f) : (Random.Range(-5.0f, 5.0f));
        float y = isXLocked ? (Random.Range(-5.0f, 5.0f)) : (Random.Range(0.0f, 1.0f) > 0.5 ? 5.0f : -5.0f);

        return new Vector3(x, y, 0);
    }

    public void setCurrHealth(float val) { currHealth = val; }
    public float getCurrHealth() { return currHealth; }
    public void setMaxHealth(float val) { maxHealth = val; }
    public float getMaxHealth() { return maxHealth; }

    private AITask chooseRandomTask()
    {
        if (aiTaskR.Count > 0)
        {
            ArrayList workingList = aiTaskR;
            bool taskChosen = false;
            while (workingList.Count > 0 && !taskChosen)
            {
                AITask task = (AITask)aiTaskR.ToArray().GetValue(
                              (int)(Random.Range(0, aiTaskR.Count)));
                if (task != null && task.runCondition())
                {
                    state.setAITask(task);
                    state.setTimer(task.getDuration());
                    taskChosen = true;
                    return task;
                }
                else
                {
                    if(task != null)
                    {
                        workingList.Remove(task);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        return null;
    }

    private class AIState
    {
        AITask currentTask;
        int timer = 0;
        bool isInSequence = false;

        public AIState()
        {

        }

        public void setAITask(AITask task)
        {
            currentTask = task;
        }

        public AITask getAITask()
        {
            return currentTask;
        }

        public void setTimer(int time) { timer = time; }
        public int getTimer() { return timer; }
        public void setIsInSequence(bool val) { isInSequence = val; }
        public bool getIsInSequence() { return isInSequence; }

        public int[] genPosOnEdgeWithDir()
        {
            return new int[0];
        }
    }
}
