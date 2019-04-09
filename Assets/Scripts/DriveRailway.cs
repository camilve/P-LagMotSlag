using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public static int nrCoins = 0;
    public GameObject CoinPrefab;
    public Text points;
    public int totalNrCoins = 0;
    private bool infoFinished = false;
    public Text info;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {      
        player = GameObject.Find("Player");
        counter = 1;
        spineStartPos = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);

#warning Gjøre om dette, høre med Elise hva hun tenker!
        //Test coin
        Instantiate(CoinPrefab, new Vector3(4.6f, 5.55f, 30f), rotation);


        float startPos = 70f;
        float endPos = 1744f;
        Vector3 prevPos = new Vector3(0,0,startPos);
        while (prevPos.z < endPos)
        {
            Vector3 pos;
            pos.y = 5.55f;
            float randomValue = Random.value;
            if(randomValue < 0.4)
            {
                pos.x = -4.6f;
            }
            else if (randomValue < 0.8)
            {
                pos.x = 4.6f;
            }
            else
            {
                pos.x = 0f;
            }

            if(prevPos.x == pos.x)
            {               
                pos.z = prevPos.z + Random.Range(4f, 15f);
            }
            else
            {
                pos.z = prevPos.z + Random.Range(10f, 21f);
            }

            if(pos.x != 0)
            {
                Instantiate(CoinPrefab, pos, rotation);
                totalNrCoins++;
            }
            
            prevPos = pos;
            
        }
        if(SwitchScene.lvl == 1)
        {
            speed = 200f;
        }
        else if (SwitchScene.lvl == 2)
        {
            speed = 350f;
        } 
        else
        {
            speed = 500f;
        }


        FindObjectOfType<AudioManager>().Stop("Theme");

    }









    // Update is called once per frame
    void Update()
    {              
        if (nrCoins == 0)
        {
            wait();
            infoRound();
        }
        else
        {
            points.text = "" + nrCoins;
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
                    Vector3 spinePos = GetVector(body.Joints[Windows.Kinect.JointType.SpineBase].Position); ;
                    Vector3 ankleRight = GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position);
                    Vector3 ankleLeft = GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position);


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



                    //Save start position
                    if (counter == 1)
                    {
                        spineStartPos = spinePos;
                        startAngleLeft = leftAngleAnkle * 180 / Mathf.PI;
                        startAngleRight = rightAngleAnkle * 180 / Mathf.PI;
                        counter++;
                    }

                    while (!infoFinished)
                    {
                        info.text = "";




                        infoFinished = true;
                    }



                    if (Mathf.Abs(spineStartPos.x - spinePos.x) > 0.03)
                    {
                        if ((leftAngleAnkle * 180 / Mathf.PI) > startAngleLeft) //Leans toward left
                        {
                            if(Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3 > 30)
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, 30f);
                            }
                            else
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3);
                            }                     
                        }
                        else if ((rightAngleAnkle * 180 / Mathf.PI) > startAngleRight) //Leans toward right
                        {
                            if (Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3 > -30)
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, -30f);
                            }
                            else
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3);
                            }
                        }
                        else
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        }

                    }
                    else
                    {
                        player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    }


                    //Check if the player is leaning over to one of the sides. Z is the rotation that has to change (20 to -20 -- depends on the degrees)




                    // Første går det raskere og raskere, kanskje i et høyere level
                    //rb.AddForce(0, 0, 500 * Time.deltaTime);
                    rb.velocity = new Vector3(0, 0, speed*Time.deltaTime);
                }
            }        
        }

        if(player.transform.position.z > 1744f)
        {
            SceneManager.LoadScene("Menu");
        }

    }
    private Vector3 GetVector(CameraSpacePoint point)
    {
        Vector3 v = new Vector3(0f, 0f, 0f);
        v.x = point.X;
        v.y = point.Y;
        v.z = point.Z;
        return v;
    }







    private void infoRound ()
    {
        info.text = "sg";
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
                Vector3 spinePos = GetVector(body.Joints[Windows.Kinect.JointType.SpineBase].Position); ;
                Vector3 ankleRight = GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position);
                Vector3 ankleLeft = GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position);


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



                //Save start position
                if (counter == 1)
                {                      
                    spineStartPos = spinePos;
                    startAngleLeft = leftAngleAnkle * 180 / Mathf.PI;
                    startAngleRight = rightAngleAnkle * 180 / Mathf.PI;
                    counter++;
                }

                Debug.Log(player.transform.position.z);

                if (Mathf.Abs(spineStartPos.x - spinePos.x) > 0.05)
                {
                    if ((leftAngleAnkle * 180 / Mathf.PI) > startAngleLeft) //Leans toward left
                    {
                        if (Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3 > 30)
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, 30f);
                        }
                        else
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3);
                        }
                        //player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3);
                        //Debug.Log(Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 2);
                    }
                    else if ((rightAngleAnkle * 180 / Mathf.PI) > startAngleRight) //Leans toward right
                    {
                        if (Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3 > -30)
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, -30f);
                        }
                        else
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3);
                        }
                        rb.velocity = new Vector3(0f, 0f, 200f * Time.deltaTime);

                    }
                    else
                    {
                        player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        info.text = "Len deg over mot høyre for å fange mynten";
                        rb.velocity = new Vector3(0, 0, 100 * Time.deltaTime);
                        if(player.transform.position.z > 28f)
                        {
                            rb.velocity = new Vector3(0f, 0f, 0f);
                        }
                    }

                }
                else
                {
                    player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    info.text = "Len deg over mot høyre for å fange mynten";
                    rb.velocity = new Vector3(0, 0, 100 * Time.deltaTime);
                    if (player.transform.position.z > 28f)
                    {
                        rb.velocity = new Vector3(0f, 0f, 0f);
                    }
                }
            }
        }
    }

    IEnumerator wait()
    {
        info.text = "Gelo";
        yield return new WaitForSeconds(10);
    }
}
