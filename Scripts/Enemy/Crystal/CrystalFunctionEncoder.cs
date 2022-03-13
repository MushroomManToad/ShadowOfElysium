using System.Collections;
using UnityEngine;

public class CrystalFunctionEncoder
{
    public enum CFEType {
        NONE,
        POSITION,
        INSTRUCTION
    }

    private CFEType type;
    private MoveCode code;
    private bool append;
    private float[] ops;
    private Vector2 pos;

    public CrystalFunctionEncoder(float x, float y)
    {
        this.type = CFEType.POSITION;
        this.pos = new Vector2(x, y);
    }

    public CrystalFunctionEncoder(Vector2 newPos)
    {
        this.type = CFEType.POSITION;
        this.pos = newPos;
    }

    public CrystalFunctionEncoder(MoveCode code, bool append, params float[] ops)
    {
        this.type = CFEType.INSTRUCTION;
        this.code = code;
        this.append = append;
        this.ops = ops;
    }

    public void runCFE(CrystalMover cm)
    {
        if(type == CFEType.POSITION)
        {
            cm.gameObject.transform.position = new Vector3(pos.x, pos.y, cm.gameObject.transform.position.z);
        }
        else if(type == CFEType.INSTRUCTION)
        {
            cm.sendInstruction(code, append, ops);
        }
    }

    public CFEType checkType()
    {
        return type;
    }
}