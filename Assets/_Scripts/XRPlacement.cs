using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class XRPlacement : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPrefabs;
    [SerializeField] private List<GameObject> imageSpawnPrefab;
    [SerializeField] private GameObject cursor;
    [SerializeField] private Vector3 offset;
    private ARRaycastManager raycastManager;
    private ARTrackedImageManager trackImageManager;
    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private Pose cursorPose;
    private Vector2 screenCenter;
    private bool isSpawnMidAir;
    private bool imagePrefabVisibility;
    private Camera mainCamera;
    private GameObject trackedGameObject;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        trackImageManager = GetComponent<ARTrackedImageManager>();
        mainCamera = Camera.main;
    }
    


    private void Update()
    {
        screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(.5f, .5f));
        Debug.Log(screenCenter);
        cursorPose = ARRayCasting(screenCenter);
        UpdateCursorPosition(cursorPose);
    }

    private void UpdateCursorPosition(Pose currentPose)
    {
        cursor.transform.position = currentPose.position;
        //cursor.transform.rotation = currentPose.rotation;
    }

    private Pose ARRayCasting(Vector2 pos)
    {
        List<ARRaycastHit> hits = new();
        raycastManager.Raycast(pos, hits, TrackableType.Planes);
        if (hits.Count>0)
        {
            UpdateCursorPosition(hits[0].pose);
            return hits[0].pose;
        }
        return cursorPose;
    }

    public void SpawnAR(int currentIndex)
    {
        GameObject instantiatedObject;
        if (isSpawnMidAir)
        {
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward.normalized * 1f ;
            instantiatedObject = Instantiate(spawnPrefabs[currentIndex], spawnPosition, Quaternion.identity);
        }
        else
        {
            instantiatedObject = Instantiate(spawnPrefabs[currentIndex], cursorPose.position, cursorPose.rotation);
        }

        instantiatedObject.transform.forward = -mainCamera.transform.forward;
        spawnedPrefabs.Add(instantiatedObject);
        
    }

    public void HideAllObjects()
    {
        foreach (GameObject spawn in spawnedPrefabs)
        {
            spawn.SetActive(!spawn.activeSelf);
        }
    }

    public void ToggleImage()
    {
        if (trackedGameObject == null)
        {
            trackedGameObject = GameObject.FindWithTag("Respawn").gameObject;
        }
        trackedGameObject.SetActive(imagePrefabVisibility);
        imagePrefabVisibility = !imagePrefabVisibility;
    }

    public void ChangeBool(bool isTrue)
    {
        isSpawnMidAir = isTrue;
    }
}
