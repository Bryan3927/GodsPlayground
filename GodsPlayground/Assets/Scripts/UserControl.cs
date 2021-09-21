using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{

    public float horizontalSpeed = 10.0f;
    public float verticalSpeed = 10.0f;
    public TerrainGeneration.TerrainGenerator TerrainGenerator;

    // Start is called before the first frame update
    void Start()
    {
        float x = TerrainGenerator.worldSize / 2f;
        transform.position = new Vector3(x, 20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalTranslation = Input.GetAxis("Horizontal") * horizontalSpeed;
        float verticalTranslation = Input.GetAxis("Vertical") * verticalSpeed;

        horizontalTranslation *= Time.deltaTime;
        verticalTranslation *= Time.deltaTime;

        transform.Translate(horizontalTranslation, 0, verticalTranslation, Space.World);
    }
}
