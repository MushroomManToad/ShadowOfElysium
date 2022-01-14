using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : IShooter
{
    public override void onShoot()
    {
        GameObject proj = Instantiate(getShootPrefab(), transform.position, Quaternion.identity);
        if (proj.GetComponent<Rigidbody2D>() != null)
        {
            proj.GetComponent<Rigidbody2D>().velocity = getSpawnVelocity();
        }
        setProjDefaults(proj);
    }
}
