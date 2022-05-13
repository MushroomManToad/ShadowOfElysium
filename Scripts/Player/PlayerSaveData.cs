using System.Collections;
using System.IO;
using UnityEngine;

/**
 * An object that can be used to interface with files. Can be created or destroyed at any time -- is really only needed for its methods.
 * 
 * To add a new variable to serialize, add it in:
 * PlayerStats#savePlayerData
 * PlayerStats#loadPlayerData
 * PlayerSaveData#saveData
 * PlayerSaveData#loadData else case
 * PlayerSaveData.PlayerDataBundle#var + getter + setter
 */
public class PlayerSaveData
{
    // DEFAULT PLAYER STAT VARIABLE VALUES
    private float defaultMaxHealth = 50.0f;
    private float defaultCurrHealth = 50.0f;
    private int defaultLumenPickups = 0;

    private string saveFileLocation;

    private PlayerStats playerStats;

    public PlayerSaveData(PlayerStats stats)
    {
        this.playerStats = stats;
    }

    public void saveData(PlayerDataBundle dataBundle)
    {
        saveFileLocation = Application.persistentDataPath + "/save0.data";
        // Write to file
        string jsonString = JsonUtility.ToJson(dataBundle);
        File.WriteAllText(saveFileLocation, jsonString);
    }

    public void loadSaveData()
    {
        saveFileLocation = Application.persistentDataPath + "/save0.data";

        if (File.Exists(saveFileLocation))
        {
            // Load from JSON
            string fileContents = File.ReadAllText(saveFileLocation);

            PlayerDataBundle dataBundle = JsonUtility.FromJson<PlayerDataBundle>(fileContents);
            playerStats.loadSaveData(dataBundle);
        }
        else
        {
            // Populate with default values
            PlayerDataBundle defaultBundle = new PlayerDataBundle();
            defaultBundle.setMaxHealth(defaultMaxHealth);
            defaultBundle.setCurrHealth(defaultCurrHealth);
            defaultBundle.setLumenPickups(defaultLumenPickups);

            // Write to file
            saveData(defaultBundle);
            playerStats.loadSaveData(defaultBundle);
        }
    }

    [System.Serializable]
    public class PlayerDataBundle
    {
        public float maxHealth;
        public float currHealth;
        public int lumenPickups;
        // Last Checkpoint Data


        public PlayerDataBundle() {}

        public void setMaxHealth(float val) { maxHealth = val; }
        public float getMaxHealth() { return maxHealth; }
        public void setCurrHealth(float val) { currHealth = val; }
        public float getCurrHealth() { return currHealth; }
        public void setLumenPickups(int val) { lumenPickups = val; }
        public int getLumenPickups() { return lumenPickups; }
    }
}