using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour
{
    public float hunger;
    public float thirst;
    public float horny;

    public float sightDistance = 20.0f;
    public float speed = 3.0f;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject closestRabbit = lookForRabbits();
        if (closestRabbit != null)
        {
            // this.transform.position = Vector3.Lerp(this.transform.position, closestRabbit.transform.position, Time.deltaTime * speed);
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            velocity.x = Vector3.Normalize(closestRabbit.transform.position).x * speed;
            velocity.z = Vector3.Normalize(closestRabbit.transform.position).z * speed;
            GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    GameObject lookForRabbits()
    {
        List<GameObject> nearbyRabbits = new List<GameObject>();
        Collider[] nearbyObjects = Physics.OverlapSphere(this.transform.position, sightDistance);
        for (int i = 0; i < nearbyObjects.Length; i++)
        {
            Collider collider = nearbyObjects[i];
            if (collider.gameObject.CompareTag("Rabbit"))
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
