using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Camera Movement Variable
    public float fSpeedCameraSwitch = .5f;
    public float fSpeedRotation = 2f;
    public bool bisFPSCamera = false;
    public bool bisTopMapView = false;

    public Transform tTopView, tFPSView, tTopMapView;
    private Vector3 refPos;

    private void Start()
    {
        GameManager.getInstance().cmCameraMovement = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (bisTopMapView)
        {
            //Interpolate Position
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, tTopMapView.position, ref refPos, fSpeedCameraSwitch);
            //Interpolate Rotation
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, tTopMapView.rotation, fSpeedRotation * Time.deltaTime);
        }
        else if (bisFPSCamera)
        {
            //Interpolate Position
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, tFPSView.position, ref refPos, fSpeedCameraSwitch);
            //Interpolate Rotation
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, tFPSView.rotation, fSpeedRotation * Time.deltaTime);
        }
        else
        {
            //Interpolate Position
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, tTopView.position, ref refPos, fSpeedCameraSwitch);
            //Interpolate Rotation
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, tTopView.rotation, fSpeedRotation * Time.deltaTime);
        }
    }

    public void ChangeView()
    {
        bisFPSCamera = !bisFPSCamera;
    }
}
