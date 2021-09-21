using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public float hunger;
    public float thirst;
    public float horny;

    public float speed = 1.0f;
    public float sightDistance = 15.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nearestFox = lookForFoxes();
        if (nearestFox != null)
        {
            Vector3 target = 2 * this.transform.position - nearestFox.transform.position;
            this.transform.position = Vector3.Lerp(this.transform.position, target, Time.deltaTime * speed);
        }
    }

    GameObject lookForFoxes()
    {
        List<GameObject> nearbyRabbits = new List<GameObject>();
        Collider[] nearbyObjects = Physics.OverlapSphere(this.transform.position, sightDistance);
        for (int i = 0; i < nearbyObjects.Length; i++)
        {
            Collider collider = nearbyObjects[i];
            if (collider.gameObject.CompareTag("Fox"))
            {
                nearbyRabbits.Add(collider.gameObject);
            }
        }
        float nearestDist = sightDistance;
        GameObject nearestRabbit = null;
        foreach (GameObject rabbit in nearbyRabbits)
        {
            float dist = Vector3.Distance(this.transform.position, rabbit.transform.position);
            if (dist <= nearestDist)
            {
                nearestDist = dist;
                nearestRabbit = rabbit;
            }
        }
        return nearestRabbit;
    }
}
