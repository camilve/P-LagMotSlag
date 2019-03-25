using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;

public class DriveRailway : MonoBehaviour
{
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;
    protected GameObject player;
    public Rigidbody rb; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            Debug.Log("1");
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            Debug.Log("2");
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            Debug.Log("3 " + _BodyManager);
            return;
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {

                Quaternion t = new Quaternion(body.JointOrientations[Windows.Kinect.JointType.SpineShoulder].Orientation.W, body.JointOrientations[Windows.Kinect.JointType.SpineShoulder].Orientation.X, body.JointOrientations[Windows.Kinect.JointType.SpineShoulder].Orientation.Y, body.JointOrientations[Windows.Kinect.JointType.SpineShoulder].Orientation.Z);
                
                //Må lagre start posisjon for å vite om man lener seg over!!!

                Vector3 shoulderpos = GetVector(body.Joints[Windows.Kinect.JointType.ShoulderRight].Position);
                Vector3 footpos = GetVector(body.Joints[Windows.Kinect.JointType.FootRight].Position);

                Debug.Log("shoulder: " + shoulderpos);
                Debug.Log("foot: " + footpos);


                // Første går det raskere og raskere, kanskje i et høyere level
                //rb.AddForce(0, 0, 500 * Time.deltaTime);
                rb.velocity = new Vector3(0, 0, 500 * Time.deltaTime);
            }
        }
    }
    private Vector3 GetVector(CameraSpacePoint point)
    {
        return new Vector3(point.X, point.Y, point.Z);
    }


}
