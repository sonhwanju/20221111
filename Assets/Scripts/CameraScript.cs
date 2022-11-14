using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cubeTrm;
    [SerializeField]
    private Transform cameraTransform;

    private Quaternion cameraRot = Quaternion.identity;

    private Vector3 backDir = Vector3.zero;

    private RaycastHit hitInfo;

    [SerializeField] private int minZoomDist = 5;
    [SerializeField] private int maxZoomDist = 30;

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float adjustment = 1f;
    [SerializeField] private float rotMinY = -45f;
    [SerializeField] private float rotMaxY = 45f;

    private float offset = 0f;

    private bool canZoomOut;

    private void Start()
    {
        cameraRot = transform.localRotation;
        offset = cameraTransform.localPosition.z;
    }

    void LateUpdate()
    {
        Rotate();
        Zoom();
    } 

    void FixedUpdate()
    {
        backDir = cameraTransform.position - cubeTrm.position;

        if (Physics.Raycast(cubeTrm.position, backDir, out hitInfo, backDir.magnitude))
        {
            cameraTransform.position = hitInfo.point;
        }
        else if(Input.mouseScrollDelta.y == 0)
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(0,0,offset), Time.deltaTime * 3f);
        }

        if (Physics.Raycast(cubeTrm.position, backDir, out hitInfo, backDir.magnitude + adjustment))
        {
            canZoomOut = false;
        }
        else
        {
            canZoomOut = true;
        }

    }

    private void Rotate()
    {
        if(Input.GetMouseButton(1))
        {
            cameraRot.x += -Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            cameraRot.y += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;

            if(cameraRot.x >= rotMaxY)
            {
                cameraRot.x = rotMaxY;
            }
            else if(cameraRot.x <= rotMinY)
            {
                cameraRot.x = rotMinY;
            }

            transform.localRotation = Quaternion.Euler(cameraRot.x, cameraRot.y, cameraRot.z);
        }
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y == 0) return;

        backDir = cameraTransform.position - cubeTrm.position;

        if (Input.mouseScrollDelta.y > 0 && backDir.magnitude >= minZoomDist)
        {
            cameraTransform.Translate(Vector3.forward * Time.deltaTime * zoomSpeed);
        }
        else if (Input.mouseScrollDelta.y < 0 && canZoomOut && backDir.magnitude <= maxZoomDist)
        {
            cameraTransform.Translate(Vector3.back * Time.deltaTime * zoomSpeed);
        }

        offset = cameraTransform.localPosition.z;
    }
}
