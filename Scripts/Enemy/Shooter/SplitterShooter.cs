using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterShooter : IShooter
{
    private float splitterAngle = 25.0f;
    private int extraProj = 1;

    public override void onShoot()
    {
        GameObject projBase = Instantiate(getShootPrefab(), transform.position, Quaternion.identity);
        calcRotation(projBase, 0);
        setProjDefaults(projBase);

        for (int i = 0; i < extraProj; i++)
        {
            GameObject proj1 = Instantiate(getShootPrefab(), transform.position, Quaternion.identity);
            calcRotation(proj1, splitterAngle * (i + 1));
            setProjDefaults(proj1);

            GameObject proj2 = Instantiate(getShootPrefab(), transform.position, Quaternion.identity);
            calcRotation(proj2, -splitterAngle * (i + 1));
            setProjDefaults(proj2);
        }
    }

    private void calcRotation(GameObject proj, float rotation)
    {
        if (proj.GetComponent<Rigidbody2D>() != null)
        {
            Vector2 rotVec = Quaternion.AngleAxis(rotation, Vector3.forward) * getSpawnVelocity();
            proj.GetComponent<Rigidbody2D>().velocity = rotVec;
            if (rotVec.x == 0)
            {
                proj.transform.eulerAngles = new Vector3(
                    proj.transform.eulerAngles.x,
                    proj.transform.eulerAngles.y,
                    (rotVec.y > 0 ? 90.0f : -90.0f) + getProjZRotation());
            }
            else
            {
                proj.transform.eulerAngles = new Vector3(
                    proj.transform.eulerAngles.x,
                    proj.transform.eulerAngles.y,
                    Mathf.Atan2(rotVec.y, rotVec.x) * (180.0f / Mathf.PI) + getProjZRotation());
            }
        }
    }

    public void setSplitterAngle(float val) { splitterAngle = val; }
    public float getSplitterAngle() { return splitterAngle; }
    public void setExtraProj(int val) { extraProj = val; }
    public int getExtraProj() { return extraProj;}
}
