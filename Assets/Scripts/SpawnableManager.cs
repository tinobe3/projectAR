using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class SpawnableManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public TMP_Text text;

    [SerializeField]
    List<GameObject> spawnablePrefabs;

    Camera arCamera;
    GameObject spawnedObject;

    int currentPrefabIndex = 0;
    public float collisionsCount = 0.0f;
    public int maxCollisions = 5;


    [SerializeField]
    float throwForce = 5f;

    [SerializeField]
    GameObject throwObject;

    void Start()
    {
        spawnedObject = null;
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);

        if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        spawnPrefab(hits[0].pose.position, currentPrefabIndex);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = hits[0].pose.position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && spawnedObject != null)
            {
                Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddForce(arCamera.transform.forward * throwForce, ForceMode.Impulse);
                }
                spawnedObject = null;
            }
        }
    }

    private void spawnPrefab(Vector3 position, int index = 0)
    {
        spawnedObject = Instantiate(spawnablePrefabs[index], position, Quaternion.identity);
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        CollisionDetector detector = spawnedObject.AddComponent<CollisionDetector>();
        detector.spawnableManager = this;
        detector.index = index;
    }


    public void togglePrefab()
    {
        currentPrefabIndex = (currentPrefabIndex + 1) % spawnablePrefabs.Count;
    }
}
