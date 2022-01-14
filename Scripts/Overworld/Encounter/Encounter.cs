using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    public EncounterEvent preEvent, postEvent;

    public string battleScene;

    public void StartEncounter(GameObject player, GameObject cam)
    {
        if(preEvent != null)
        {

        }
        player.SetActive(false);
        cam.SetActive(false);
        SceneManager.LoadScene(battleScene, LoadSceneMode.Additive);
    }
}
