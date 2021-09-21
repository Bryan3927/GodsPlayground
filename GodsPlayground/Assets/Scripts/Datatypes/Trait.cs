using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Animal Traits
public abstract class Trait : MonoBehaviour
{
    public abstract void Apply();

    public abstract void Mutate();
}
