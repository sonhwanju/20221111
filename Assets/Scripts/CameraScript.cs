using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private SphericalCoordinates sphericalCoordinates;

    [SerializeField]
    private Transform cubeTrm;
    [SerializeField]
    private Transform cameraTransform;

    private Vector3 backDir = Vector3.zero;

    private RaycastHit hitInfo;

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;

    private void Start()
    {
        sphericalCoordinates = new SphericalCoordinates(cameraTransform.position);
    }

    void LateUpdate()
    {
        Rotate();
        Zoom();

        CheckInObj();
    } 

    private void Rotate()
    {
        if(Input.GetMouseButton(1))
        {
            cameraTransform.position = sphericalCoordinates.Rotate(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")).ToCartesian + cubeTrm.position;
            cameraTransform.LookAt(cubeTrm);
        }
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y == 0) return;

        backDir = cameraTransform.position - cubeTrm.position;

        cameraTransform.position = sphericalCoordinates.TranslateRadius(-Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed).ToCartesian + cubeTrm.position;
    }

    private void CheckInObj()
    {
        backDir = cameraTransform.position - cubeTrm.position;

        if (Physics.Raycast(cubeTrm.position, backDir, out hitInfo, backDir.magnitude))
        {
            cameraTransform.position = hitInfo.point;
        }
    }
}

[System.Serializable]
public class SphericalCoordinates
{
    public float radius, azimuth, elevation;

    public float Radius
    {
        get { return radius; }
        private set
        {
            radius = Mathf.Clamp(value, minRadius, maxRadius);
        }
    }

    public float Azimuth
    {
        get { return azimuth; }
        private set
        {
            azimuth = Mathf.Repeat(value, _maxAzimuth - _minAzimuth);
        }
    }

    public float Elevation
    {
        get { return elevation; }
        private set
        {
            elevation = Mathf.Clamp(value, _minElevation, _maxElevation);
        }
    }

    public float minRadius = 3f;
    public float maxRadius = 15f;

    public float minAzimuth = 0f;
    private float _minAzimuth;

    public float maxAzimuth = 360f;
    private float _maxAzimuth;

    public float minElevation = 0f;
    private float _minElevation;

    public float maxElevation = 80f;
    private float _maxElevation;

    public SphericalCoordinates() { }

    // Returns the spherical coordinates of the current camera position.
    public SphericalCoordinates(Vector3 cartesianCoordinate)
    {
        _minAzimuth = Mathf.Deg2Rad * minAzimuth;
        _maxAzimuth = Mathf.Deg2Rad * maxAzimuth;

        _minElevation = Mathf.Deg2Rad * minElevation;
        _maxElevation = Mathf.Deg2Rad * maxElevation;

        Radius = cartesianCoordinate.magnitude;
        Debug.Log(Radius);
        Azimuth = Mathf.Atan2(cartesianCoordinate.z, cartesianCoordinate.x);
        Elevation = Mathf.Asin(cartesianCoordinate.y / Radius);
    }

    // Converts the current camera position to Cartesian coordinates and returns it.
    public Vector3 ToCartesian
    {
        get
        {
            float t = Radius * Mathf.Cos(Elevation);
            return new Vector3(t * Mathf.Cos(Azimuth), Radius * Mathf.Sin(Elevation), t * Mathf.Sin(Azimuth));
        }
    }

    // Move the camera in a spherical coordinate system.
    public SphericalCoordinates Rotate(float newAzimuth, float newElevation)
    {
        Azimuth += newAzimuth;
        Elevation += newElevation;
        return this;
    }

    public SphericalCoordinates TranslateRadius(float x)
    {
        Radius += x;
        return this;
    }
}