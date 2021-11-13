using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControl : MonoBehaviour
{

    public float lowerLimit = 5;
    public float upperLimit = 40;

    public float horizontalSpeed = 10.0f;
    public float verticalSpeed = 10.0f;
    public float zoomSpeed = 20.0f;
    public TerrainGeneration.TerrainGenerator TerrainGenerator;
    public PopulationChart populationChartScript;
    public Zoom zoom;

    // Start is called before the first frame update
    void Start()
    {
        float x = TerrainGenerator.worldSize / 2f;
        transform.position = new Vector3(x, 20, 0);
        GameObject populationChartCamera = populationChartScript.gameObject.transform.GetChild(0).gameObject;
        populationChartCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject populationChartCamera = populationChartScript.gameObject.transform.GetChild(0).gameObject;

        moveCamera();

        populationChartScript.DisplayPopulations();

        if (Input.GetKeyDown(KeyCode.Tab)) {
            populationChartCamera.SetActive(true);
        } else if (Input.GetKeyUp(KeyCode.Tab)) {
            populationChartCamera.SetActive(false);
        }
        zoom.DisplayZoom();
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

        transform.Translate(horizontalTranslation, 0, verticalTranslation, Space.World);
       
        zoom.SetZoom(transform.position.y + zoomTranslation);
        if (transform.position.y < lowerLimit) {
           transform.position = new Vector3(transform.position.x, lowerLimit, transform.position.z);
            //zoomTranslation = 0;
            zoom.SetZoom(lowerLimit);
        } else if (transform.position.y > upperLimit) {
            transform.position = new Vector3(transform.position.x, upperLimit, transform.position.z);
            zoom.SetZoom(upperLimit);
            //zoomTranslation = 0;
        }
        
        else
        {

           if (((transform.position.y!=lowerLimit) || (zoomTranslation<=0)) && ((transform.position.y != upperLimit) || (zoomTranslation >= 0)))
            {
                transform.Translate(0, 0, zoomTranslation, Space.Self);
            }
        }
        
        //transform.Translate(0, 0, zoomTranslation, Space.Self);
    }
}
