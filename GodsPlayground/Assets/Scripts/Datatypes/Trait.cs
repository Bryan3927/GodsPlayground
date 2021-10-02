using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for Animal Traits
public abstract class Trait : MonoBehaviour
{
    public abstract string Name { get; }

    public abstract string Description { get; }

    public abstract void Apply(Animal animal);

    public abstract void Mutate();
}
