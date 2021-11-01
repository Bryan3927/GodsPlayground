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
    public PopulationChart populationChartScript;

    // Start is called before the first frame update
    void Start()
    {
        float x = TerrainGenerator.worldSize / 2f;
        transform.position = new Vector3(x, 20, 0);
        GameObject lineGraphCamera = lineGraphScript.gameObject.transform.GetChild(0).gameObject;
        GameObject populationChartCamera = populationChartScript.gameObject.transform.GetChild(0).gameObject;
        lineGraphCamera.SetActive(false);
        populationChartCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject lineGraphCamera = lineGraphScript.gameObject.transform.GetChild(0).gameObject;
        GameObject populationChartCamera = populationChartScript.gameObject.transform.GetChild(0).gameObject;

        moveCamera();

        populationChartScript.DisplayPopulations();

        if (Input.GetKeyDown(KeyCode.Tab)) {
            //lineGraphCamera.SetActive(true);
            populationChartCamera.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
            //lineGraphCamera.SetActive(false);
            populationChartCamera.SetActive(false);
        }
    }

    // Helper to move the camera
    void moveCamera()
    {
        float horizontalTranslation = Input.GetAxis("Horizontal") * (horizontalSpeed * Mathf.Clamp01((2 + transform.position.y) / 20.0f));
        float verticalTranslation = Input.GetAxis("Vertical") * (verticalSpeed * Mathf.Clamp01((2 + transform.position.y) / 20.0f));
        float zoomTranslation = Input.GetAxis("Zoom") * zoomSpeed;

        horizontalTranslation *= Time.deltaTime;
        verticalTranslation *= Time.deltaTime;
        zoomTranslation *= Time.deltaTime;

        float horizontalChange = transform.position.x + horizontalTranslation;
        float verticalChange = transform.position.z + verticalTranslation;
        float zoomChange = transform.position.z + zoomTranslation;

        if (horizontalChange > 60.0f) {
            horizontalTranslation = 60.0f - transform.position.x;
        } else if (horizontalChange < 0.0f) {
            horizontalTranslation = 0.0f - transform.position.x;
        }

        if (zoomChange > 20.0f) {
            zoomTranslation = 20.0f - transform.position.z;
        } else if (zoomChange < -20.0f) {
            zoomTranslation = -20.0f - transform.position.z;
        }

        Debug.Log(transform.position);
        
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q)) {
            transform.Translate(0, 0, zoomTranslation, Space.Self);
        } else {
            transform.Translate(horizontalTranslation, 0, verticalTranslation, Space.World);
        }
    }
}
