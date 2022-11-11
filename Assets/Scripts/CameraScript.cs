using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;

    private Vector3 backDir = Vector3.zero;

    [SerializeField] private int minZoomDist = 5;
    [SerializeField] private int maxZoomDist = 30;

    [SerializeField] private float zoomSpeed = 10f;

    private bool canZoomOut;

    void LateUpdate()
    {
        if (Input.mouseScrollDelta.y == 0) return;

        backDir = gameObject.transform.position - parentTransform.position;

        if (Input.mouseScrollDelta.y > 0 && backDir.magnitude >= minZoomDist) 
        {
            gameObject.transform.Translate(Vector3.forward * Time.deltaTime * zoomSpeed);
        }
        else if (Input.mouseScrollDelta.y < 0 && canZoomOut && backDir.magnitude <= maxZoomDist)
        {
            gameObject.transform.Translate(Vector3.back * Time.deltaTime * zoomSpeed);
        }
    } 

}
