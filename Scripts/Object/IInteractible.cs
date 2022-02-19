using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractible : MonoBehaviour
{
    public virtual bool onInteract() { return false; }
}
