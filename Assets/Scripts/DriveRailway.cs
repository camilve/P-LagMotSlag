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
    private float startAngleLeft;
    private float startAngleRight;
    static public int nrCoins = 0;
    public GameObject CoinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        counter = 1;
        spineStartPos = new Vector3(0, 0, 0);



        for(int i=0; i<500; i++)
        {
            Vector3 pos;
            pos.y = 5.55f;
            if(Random.value < 0.5)
            {
                pos.x = -4.45f;
            }
            else
            {
                pos.x = 4.45f;
            }
            pos.z = Random.Range(70f, 1744f);

            Instantiate(CoinPrefab, pos, Quaternion.identity);
            
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            Debug.Log("1");
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            Debug.Log("2");
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            player.transform.eulerAngles = new Vector3(0, 0, 0);
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


                //Debug.Log("left ankle: " + leftAngleAnkle * 180 / Mathf.PI);
                //Debug.Log("right ankle: " + rightAngleAnkle * 180 / Mathf.PI);

                //Save start position
                if (counter == 1)
                {
                    spineStartPos = spinePos;
                    startAngleLeft = leftAngleAnkle * 180 / Mathf.PI;
                    startAngleRight = rightAngleAnkle * 180 / Mathf.PI;
                    counter++;
                }

                

                if (Mathf.Abs(spineStartPos.x - spinePos.x) > 0.05)
                {
                    if((leftAngleAnkle * 180 / Mathf.PI) > startAngleLeft) //Leans toward left
                    {
                        player.transform.eulerAngles = new Vector3(0,0, Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 2);
                        //Debug.Log(Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 2);
                    }
                    else if ((rightAngleAnkle * 180 / Mathf.PI) > startAngleRight) //Leans toward right
                    {
                        player.transform.eulerAngles = new Vector3(0, 0, Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -2);
                        //Debug.Log(Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 2);
                    }
                    else
                    {
                        player.transform.eulerAngles = new Vector3(0, 0, 0);
                    }

                }
                else
                {
                    player.transform.eulerAngles = new Vector3(0, 0, 0);
                }


                //Check if the player is leaning over to one of the sides. Z is the rotation that has to change (20 to -20 -- depends on the degrees)




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
