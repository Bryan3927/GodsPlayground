using UnityEngine;

public class LivingEntity : MonoBehaviour {

    public int colourMaterialIndex;
    public Species species;
    public Material material;

    public Coord coord;
    //
    [HideInInspector]
    public int mapIndex;
    [HideInInspector]
    public Coord mapCoord;

    protected bool dead;

    public GameObject death;

    public virtual void Init (Coord coord) {
        this.coord = coord;
        transform.position = Environment.tileCentres[coord.x, coord.y];

        // Set material to the instance material
        var meshRenderer = transform.GetComponentInChildren<MeshRenderer> ();
        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
        {
            if (meshRenderer.sharedMaterials[i] == material) {
                material = meshRenderer.materials[i];
                break;
            }
        }
    }

    public virtual float Consume(float amount)
    {
        return 1.0f;
    }

    protected virtual void Die (CauseOfDeath cause) {
        if (!dead) {
            GameObject deathParticles = Instantiate(death);
            deathParticles.transform.position = this.coord;
            Death particles = deathParticles.GetComponent<Death>();
            switch (cause)
            {
                case CauseOfDeath.Eaten:
                    particles.Init(Color.black);
                    break;
                case CauseOfDeath.Age:
                    particles.Init(Color.black);
                    break;
                case CauseOfDeath.Hunger:
                case CauseOfDeath.Thirst:
                    particles.Init(Color.black);
                    break;
                default:
                    particles.Init(Color.black);
                    break;
            }
                
            dead = true;
            Environment.RegisterDeath (this);
            Destroy (gameObject);
        }
    }
}