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

    public HPBar healthBar;

    // Update this value in editor to determine how many times each attack should be run in before rerandomizing sequence.
    // Prevents long strings of attacks at low values
    // Can (but probably won't have) longer strings of the same attack over and over at higher values
    // Default: 2
    public int randomTaskCycleAmount = 2;

    // ArrayList used by randomTaskChooser to track which AITasks are safe to choose randomly.
    ArrayList randomTaskWorkingList = new ArrayList();

    // aiTaskS.add();
    protected abstract void AITaskSequence();

    // aiTaskR.add();
    protected abstract void AITaskRandom();

    // aiTaskF.add();
    protected abstract void AITaskForce();

    private void Start()
    {
        maxHealth = startHealth;
        currHealth = startHealth / 2.0f;
        updateHealthRenderer(false);
        // Loads AITasks for future reference
        AITaskSequence();
        AITaskRandom();
        AITaskForce();
    }

    private void FixedUpdate()
    {
        renderEffects();
        subClassPreUpdate();
        preUpdate();
        subClassUpdate();
        runAITask();
        subClassUpdate();
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

    protected virtual void subClassPreUpdate() {}
    protected virtual void subClassUpdate() { }
    protected virtual void subClassPostUpdate() { }

    protected virtual void preUpdate() { }

    protected void runAITask()
    {
        // Once the AIState's timer is 0 (or less), we must choose a new AITask.
        // Recall that AITasks are NOT instantiated, but instead use a single, reusable Task instance regenerated on EnemyAI load.
        if(state.getTimer() <= 0)
        {
            // First, end the active task with any applicable behavior (generally resetting variables)
            if(state.getAITask() != null) state.getAITask().endAction();
            // 
            state.setAITask(null);

            //
            // This part is the algorithm that chooses the AI Task
            //

            // A bool to track when a task has been successfully chosen so that a higher priority task isn't overwritten by a lower priority task.
            bool taskChosen = false;
            // We begin by iterating through Forced Tasks (aiTaskF). If *ANY* of these can be run, queue it to run.
            // There should never be 2 forced tasks that can run at the same time, but the first one added will run first.
            foreach (AITask task in aiTaskF)
            {
                // If another Forced Task has been chosent to 
                if (!taskChosen)
                {
                    // If the task exists and meets its run condition(s)
                    if (task != null && task.runCondition())
                    {
                        // Finding a Forced Task that can be run sets it to the active task...
                        state.setAITask(task);
                        // ... updates the state's timer to the task's duration for tracking tasklength...
                        state.setTimer(task.getDuration());
                        // ... and sets taskChosen to true so no other tasks can be chosen.
                        taskChosen = true;
                    }
                }
            }
            // If no forced tasks were chosen...
            if(!taskChosen)
            {
                // Currently unused, and doesn't seem particularly useful atm, so assume returns false
                if(state.getIsInSequence())
                {
                    // TODO : Sequence handler when state is in sequence.
                }
                // ... choose a random task from aiTaskR
                else
                {
                    AITask chosenTask = chooseRandomTask();
                    // If a task is successfully chosen (which it should be), set taskChosen to true.
                    if(chosenTask != null) taskChosen = true;
                }
            }
            // Check that a task was chosen (and exists, which are, in theory, the same check). 
            if (taskChosen && state.getAITask() != null)
            {
                // If so, we will run the firstFrameUdpate before proceeding
                state.getAITask().firstFrameUpdate();
            }
        }
        // If the state's timer is greater than 0, then we can actually run the task inside the AIState
        else
        {
            // Check that an AITask exists. If it doesn't, then it was force ended by AITask#endCondition
            if (state.getAITask() != null)
            {
                AITask activeTask = state.getAITask();
                // If the endCondition is not satisfied
                if(!activeTask.endCondition())
                {
                    // Actually run the task
                    activeTask.runAction();
                    // Do backend post-task things such as decrementing the internal task timer.
                    activeTask.frameUpdate();
                }
            }
            // A force-ended task left over some timer data. Reset the timer to 0 
            else state.setTimer(0);
        }
        // The part where the timer actuall decrements every frame. Amazing!
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

    /*
     * Enemy HP Methods
     */
    public void setCurrHealth(float val, bool flash) 
    {
        if (val > maxHealth) 
        {
            currHealth = maxHealth;
        }
        if (val < 0)
        {
            currHealth = 0;
        }
        else
        {
            currHealth = val;
        }
        updateHealthRenderer(true);
    }
    public virtual void damage(float amount) { setCurrHealth(currHealth -= amount, true); }
    public float getCurrHealth() { return currHealth; }
    public void setMaxHealth(float val) 
    {
        maxHealth = val;
        updateHealthRenderer(false);
    }
    public float getMaxHealth() { return maxHealth; }

    public void updateHealthRenderer(bool flash)
    {
        healthBar.syncHP(maxHealth, currHealth, flash);
    }

    public float getHealthPercent()
    {
        return getCurrHealth() / getMaxHealth();
    }

    /*
     * Enemy AITask chooser helpers
     */

    /*
     * Chooses a random attack by looking at the list of remaining AITasks
     * (List of remaining Tasks determined by randomTaskCycleAmount. If list is empty, refill it.)
     */
    private AITask chooseRandomTask()
    {

        // Randomly chosen AI tasks must exist in some capacity, or this whole logic is useless. Check for them.
        if (aiTaskR.Count > 0)
        {
            // If the tasklist is empty at the start of the frame, populate it.
            if(randomTaskWorkingList.Count == 0)
            {
                populateRandomTaskWorkingList();
            }
            // A bool to track when a task has successfully been chosen.
            bool taskChosen = false;
            // We try to repopulate the list once and iterate through it again. 
            bool hasTriedRepopulation = false;
            // While we aren't out of tasks to try to choose and a task has not successfully been chosen
            while (!taskChosen && randomTaskWorkingList.Count > 0)
            {
                // Attempt to choose task. Convert rTWL to an array to get a random value from it.
                AITask task = (AITask)randomTaskWorkingList.ToArray().GetValue(
                              (int)(Random.Range(0, randomTaskWorkingList.Count)));
                // If the task can be run...
                if (task != null && task.runCondition())
                {
                    // Set the active task in the AIState to this task and set the state timer appropriately.
                    state.setAITask(task);
                    state.setTimer(task.getDuration());
                    taskChosen = true;
                    // And don't forget to remove it from the working array (Ya know, the whole point of the working array...)
                    randomTaskWorkingList.Remove(task);
                    return task;
                }
                else
                {
                    // If a task was chosen but failed to execute...
                    if(task != null)
                    {
                        // Remove it from the working list and retry generation via the while loop.
                        randomTaskWorkingList.Remove(task);
                    }
                    // If no task was chosen, the list must be empty.
                    else
                    {
                        // Try repopulation once.
                        if (!hasTriedRepopulation)
                        {
                            populateRandomTaskWorkingList();
                            hasTriedRepopulation = true;
                        }
                        // If it still fails, then return null this frame. No task is chosen. This should never happen and is only a failsafe.
                        else
                        {
                            Debug.LogWarning("AI Task choosing has failed. This is unexpected behavior - was a timer implemented incorrectly?");
                            return null;
                        }
                    }
                }
            }
        }
        return null;
    }

    /*
     * Populate the list of randomTasks according to aiTaskR and randomTaskCycleAmount
     */
    private void populateRandomTaskWorkingList()
    {
        // Nuke any data remaining in the list (shouldn't be any)
        randomTaskWorkingList.Clear();
        // Iterate over all AI Tasks
        foreach(AITask a in aiTaskR)
        {
            // For each time it should be in the cycle
            for(int i = 0; i < randomTaskCycleAmount; i++)
            {
                // Add it to the working array.
                randomTaskWorkingList.Add(a);
            }
        }
    }

    // This internal class stores all the data relevant to the *active* AI task
    private class AIState
    {
        AITask currentTask;
        int timer = 0;
        bool isInSequence = false;

        public AIState() {}

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
