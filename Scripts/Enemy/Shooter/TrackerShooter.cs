using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerShooter : MonoBehaviour
{
    public IShooter shooter;
    public GameObject target;

    private void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 nAim = (target.transform.position - transform.position).normalized * Mathf.Sqrt(shooter.getSpawnVelocity().sqrMagnitude);
            shooter.setSpawnVelocity(nAim);
        }
    }

    public void setTarget(GameObject go)
    {
        target = go;
    }
}
