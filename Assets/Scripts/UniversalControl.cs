using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UniversalControl : MonoBehaviour
{
    public GameObject[] collectors = new GameObject[5];
    public List<GameObject>[] buildinglists = new List<GameObject>[5];
    public float planetRadius;
    public GameObject planetOrigin;
    //============================================================================
    Vector3 destination;
    public float speed = 1F;
    public float moveSpeed = 1F;
    public Rigidbody galaxyCamera;
    public float rotateFactor = 1F;
    public Rigidbody childCamera;

    public int[] numCollectors;
    // Use this for initialization
    void Start()
    {
        numCollectors = new int[5];

        buildinglists[0] = new List<GameObject>();
        buildinglists[1] = new List<GameObject>();
        buildinglists[2] = new List<GameObject>();
        buildinglists[3] = new List<GameObject>();
        buildinglists[4] = new List<GameObject>();
        //============================================================================
        galaxyCamera.transform.position = new Vector3(0, 0, -100);
        childCamera.transform.localPosition = new Vector3(0, 0, 0);
        //============================================================================
        WithMenu();
    }

    // Update is called once per frame
    void Update()
    {
        //Demo
        //if (Input.GetKey(KeyCode.M)) WithMenu();
        //if (Input.GetKey(KeyCode.LeftShift)) WithDock();
        //if (Input.GetKey(KeyCode.LeftAlt)) WithDockAndMenu();
        //if (Input.GetKey(KeyCode.Space)) WithDefault();
        //if (Input.GetKeyDown(KeyCode.Alpha1)) AddCollector(0);
        //if (Input.GetKeyDown(KeyCode.Alpha2)) AddCollector(1);
        //if (Input.GetKeyDown(KeyCode.Alpha3)) AddCollector(2);
        //if (Input.GetKeyDown(KeyCode.Alpha4)) AddCollector(3);
        //if (Input.GetKeyDown(KeyCode.Alpha5)) AddCollector(4);
        //if (Input.GetKeyDown(KeyCode.Alpha6)) RemoveCollector(0);
        //if (Input.GetKeyDown(KeyCode.Alpha7)) RemoveCollector(1);
        //if (Input.GetKeyDown(KeyCode.Alpha8)) RemoveCollector(2);
        //if (Input.GetKeyDown(KeyCode.Alpha9)) RemoveCollector(3);
        //if (Input.GetKeyDown(KeyCode.Alpha0)) RemoveCollector(4);
        //if (Input.GetKeyDown(KeyCode.Q)) SunView();
        //if (Input.GetKeyDown(KeyCode.E)) PlanetView();
        //============================================================================
        galaxyCamera.transform.RotateAround(Vector3.zero, Vector3.up, -rotateFactor * 1 * Time.deltaTime);
        galaxyCamera.transform.RotateAround(Vector3.zero, Vector3.up, moveSpeed * Input.GetAxis("Horizontal") * 2);
        galaxyCamera.transform.LookAt(new Vector3(0, 0, 0));
        //galaxyCamera.transform.position += galaxyCamera.transform.forward * moveSpeed * Input.GetAxis("Mouse ScrollWheel") * 1000;
        galaxyCamera.transform.position += galaxyCamera.transform.forward * moveSpeed * Input.GetAxis("Vertical") * 10;
        childCamera.transform.localPosition = Vector3.Lerp(childCamera.transform.localPosition, destination, 3 * speed * Time.deltaTime);

        for (int i = 0; i < 5; i++)
        {
            if (Res.numCollector[i] > numCollectors[i])
            {
                numCollectors[i]++;
                AddCollector(i);
            }
            if (Res.numCollector[i] < numCollectors[i])
            {
                numCollectors[i]--;
                RemoveCollector(i);
            }
        }
        
        //bounce!!!
        childCamera.transform.Translate (0.4f * Mathf.Sin(Time.time * (Timer.secondsElapsed / 30f)),  0.3f*Mathf.Sin(Time.time * (Timer.secondsElapsed/60f)), 0.2f * Mathf.Sin(Time.time * (Timer.secondsElapsed / 90f)));
    }

    public void AddCollector(int type)
    {
        Vector3 origin = planetOrigin.gameObject.transform.position;
        Vector3 onPlanet = UnityEngine.Random.onUnitSphere * planetRadius;
        GameObject newGO = Instantiate(collectors[type], onPlanet, Quaternion.identity, planetOrigin.transform) as GameObject;
        newGO.transform.LookAt(planetOrigin.transform.position);
        newGO.transform.rotation = newGO.transform.rotation * Quaternion.Euler(-90, 0, 0);
        buildinglists[type].Add(newGO);
    }

    public void RemoveCollector(int type)
    {
        if (buildinglists[type].Count != 0)
        {
            Destroy(buildinglists[type][0]);
            buildinglists[type].RemoveAt(0);
        }
    }

    //============================================================================
    void SetNewLocation(Vector3 nlocation)
    {
        destination = nlocation;
    }

    void WithMenu()
    {
        destination = new Vector3(30, 0, 0);
    }
}
