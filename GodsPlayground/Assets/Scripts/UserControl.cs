using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{

    public float horizontalSpeed = 10.0f;
    public float verticalSpeed = 10.0f;
    public float zoomSpeed = 20.0f;
    public TerrainGeneration.TerrainGenerator TerrainGenerator;
    public LineGraphManager lineGraphScript;

    // Start is called before the first frame update
    void Start()
    {
        float x = TerrainGenerator.worldSize / 2f;
        transform.position = new Vector3(x, 20, 0);
        GameObject lineGraphCamera = lineGraphScript.gameObject.transform.GetChild(0).gameObject;
        lineGraphCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalTranslation = Input.GetAxis("Horizontal") * (horizontalSpeed * Mathf.Clamp01((2 + transform.position.y) / 20.0f));
        float verticalTranslation = Input.GetAxis("Vertical") * (verticalSpeed * Mathf.Clamp01((2 + transform.position.y) / 20.0f));
        float zoomTranslation = Input.GetAxis("Zoom") * zoomSpeed;
        GameObject lineGraphCamera = lineGraphScript.gameObject.transform.GetChild(0).gameObject;

        horizontalTranslation *= Time.deltaTime;
        verticalTranslation *= Time.deltaTime;
        zoomTranslation *= Time.deltaTime;

        transform.Translate(horizontalTranslation, 0, verticalTranslation, Space.World);
        transform.Translate(0, 0, zoomTranslation, Space.Self);

        if (Input.GetKeyDown(KeyCode.Tab)) 
            lineGraphCamera.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.Tab))
            lineGraphCamera.SetActive(false);
    }
}
