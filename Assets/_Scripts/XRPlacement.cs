using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class XRPlacement : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    
    private ARRaycastManager raycastManager;
    private List<GameObject> spawnedPrefabs;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        #if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            ARRayCasting(mousePos);
        }
        
        #elif UNITY_ANDROID 
        
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = touch.position;
            ARRayCasting(touchPosition);
        }
        
        #endif
    }

    private void ARRayCasting(Vector2 pos)
    {
        List<ARRaycastHit> hits = new();
        if (raycastManager.Raycast(pos, hits, TrackableType.PlaneEstimated))
        {
            Pose spawnPose = hits[0].pose;
            SpawnAR(spawnPose);
        }
    }

    private void SpawnAR(Pose pose)
    {
        GameObject instantiatedObject = Instantiate(spawnPrefab, pose.position, pose.rotation);
    }
}
