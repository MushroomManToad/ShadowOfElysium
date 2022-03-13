using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    public string battleScene;

    private GameObject oCam, oPlayer;

    public void StartEncounter(GameObject player, GameObject cam)
    {
        oPlayer = player;
        oCam = cam;
        player.SetActive(false);
        cam.SetActive(false);
        SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);
    }

    public void finishEncounter()
    {
        // Reactivate overworld objects.
        // Unload scene.
        // Set appropriate Flags.

        /*
         * Actually, now that I think about it, the flags system should probably be a Dictionary, linking an internal Flag object (or sth to communicate
         * with the save file) to an identifying String. Then, I can just pass a public string array for flags to update from the battle.
         */
    }
}
