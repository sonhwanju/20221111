using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;

    private Vector3 backDir = Vector3.zero;

    private RaycastHit hitInfo;

    [SerializeField] private int minZoomDist = 5;
    [SerializeField] private int maxZoomDist = 30;

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float adjustment = 1f; 

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

    void FixedUpdate()
    {
        backDir = gameObject.transform.position - parentTransform.position;

        if (Physics.Raycast(parentTransform.position, backDir, out hitInfo, backDir.magnitude))
        {
            gameObject.transform.position = hitInfo.point;
        }

        if (Physics.Raycast(parentTransform.position, backDir, out hitInfo, backDir.magnitude + adjustment))
        {
            canZoomOut = false;
        }
        else 
        {
            canZoomOut = true;
        }

    }
}
