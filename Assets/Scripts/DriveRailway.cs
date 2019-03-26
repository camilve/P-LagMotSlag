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
    private Vector3 spineStartPos;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        counter = 1;
        spineStartPos = new Vector3(0, 0, 0);
        
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
                Vector3 spinePos = GetVector(body.Joints[Windows.Kinect.JointType.SpineBase].Position);
                Vector3 ankleRight = GetVector(body.Joints[Windows.Kinect.JointType.AnkleRight].Position);
                Vector3 ankleLeft = GetVector(body.Joints[Windows.Kinect.JointType.AnkleLeft].Position);

                if (counter == 1)
                {
                    spineStartPos = spinePos;
                    counter++;
                }

                //distance between the ankles
                float d = Mathf.Sqrt(Mathf.Pow((ankleLeft.x - ankleRight.x), 2) + Mathf.Pow((ankleLeft.y - ankleRight.y), 2) + Mathf.Pow((ankleLeft.z - ankleRight.z), 2));

                //Distance between from ankle that the spine should be (center of gravity)
                float optimalSpinePos = d / 2;


                //distance from spine to left ankle and distance from spine to right ankle
                float dSpineLeft = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleLeft.x), 2) + Mathf.Pow((spinePos.y - ankleLeft.y), 2) + Mathf.Pow((spinePos.z - ankleLeft.z), 2));
                float dSpineRight = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleRight.x), 2) + Mathf.Pow((spinePos.y - ankleRight.y), 2) + Mathf.Pow((spinePos.z - ankleRight.z), 2));

                //Find angle of left ankle to floor
                float leftAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineRight, 2) - Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineLeft * d));
                float rightAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(dSpineRight, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineRight * d));


                Debug.Log("left ankle: " + leftAngleAnkle * 180 / Mathf.PI);
                Debug.Log("right ankle: " + rightAngleAnkle * 180 / Mathf.PI);


                //Check if the player is leaning over to one of the sides. 


               

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
