using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Animal Traits
public abstract class Trait : MonoBehaviour
{
    public abstract void Apply(Animal animal);

    public abstract void Mutate();
}
