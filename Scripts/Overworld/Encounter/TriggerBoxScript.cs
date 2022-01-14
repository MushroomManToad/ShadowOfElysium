using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxScript : MonoBehaviour
{
    public Encounter encounter;
    public GameObject cam;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<OPlayerController>() != null)
        {
            encounter.StartEncounter(collider.gameObject, cam);
        }
    }
}
