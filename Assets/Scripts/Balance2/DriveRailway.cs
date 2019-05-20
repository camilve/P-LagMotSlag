using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AudioSpace;
using SwitchSpace;
using BodySourceSpace;

namespace Balance2Space
{
    /// <summary>
    /// Makes the cart in the second game drive along the railway, and leans the cart as the player leans over to the side.
    /// </summary>
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
        public static int nrCoins;
        public GameObject CoinPrefab;
        public Text points;
        public static int totalNrCoins;
        public Text info;
        private float speed;
        public static float percent = 0;
        private bool infoShowed = false;
        private bool firstInfoShowed = false;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player");
            counter = 1;
            spineStartPos = new Vector3(0, 0, 0);
            Quaternion rotation = Quaternion.Euler(90, 0, 0);

            nrCoins = 0;
            totalNrCoins = 0;

            //Create an instance of a coin in the game
            Instantiate(CoinPrefab, new Vector3(4.6f, 5.55f, 30f), rotation);


            //create instance of coins randomly alone railway
            float startPos = 70f;
            float endPos = 1744f;
            Vector3 prevPos = new Vector3(0, 0, startPos);
            while (prevPos.z < endPos)
            {
                Vector3 pos;
                pos.y = 5.55f;
                float randomValue = Random.value;
                if (randomValue < 0.4)
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

                //Longer space between coins that aren't at the same side since the player needs to change position.
                if (prevPos.x == pos.x)
                {
                    pos.z = prevPos.z + Random.Range(4f, 15f);
                }
                else
                {
                    pos.z = prevPos.z + Random.Range(10f, 21f);
                }

                if (pos.x != 0)
                {
                    Instantiate(CoinPrefab, pos, rotation);
                    totalNrCoins++;
                }

                prevPos = pos;

            }

            //Sets the speed according to the level the player chose
            if (SwitchScene.lvl == 1)
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
        }

        // Update is called once per frame
        void Update()
        {
            //If no coins are picked up, the cart is going slower and make sure the player has to pick up the first coin
            if (nrCoins == 0)
            {
                if (!infoShowed)
                {
                    //The audio information have to be showed before the game is starting (can only be started once)
                    if (!firstInfoShowed)
                    {
                        FindObjectOfType<AudioManager>().Stop("Theme");
                        StartCoroutine(infoAudio());
                    }
                }
                else
                {
                    infoRound();
                }

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

                        //distance from spine to left ankle and distance from spine to right ankle
                        float dSpineLeft = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleLeft.x), 2) + Mathf.Pow((spinePos.y - ankleLeft.y), 2) + Mathf.Pow((spinePos.z - ankleLeft.z), 2));
                        float dSpineRight = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleRight.x), 2) + Mathf.Pow((spinePos.y - ankleRight.y), 2) + Mathf.Pow((spinePos.z - ankleRight.z), 2));

                        //Find angle of left ankle to floor
                        float leftAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineRight, 2) - Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineLeft * d));
                        float rightAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(dSpineRight, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineRight * d));


                        info.text = "";


                        //to make sure the player moves the spine when leaning over (to not cheat).
                        if (Mathf.Abs(spineStartPos.x - spinePos.x) > 0.03)
                        {
                            if ((leftAngleAnkle * 180 / Mathf.PI) > startAngleLeft) //Leans toward left
                            {
                                //So the cart won't fall over.
                                if (Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3 > 30)
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, 30f);
                                }
                                //so the cart won't be so sensitive to a little movement to the side when the player does not stand the same place as he/her started.
                                else if (Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3 < 4)
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                                    player.transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                                }
                                else
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * 3);
                                }
                            }
                            else if ((rightAngleAnkle * 180 / Mathf.PI) > startAngleRight) //Leans toward right
                            {
                                if (Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3 < -30)
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, -30f);
                                }
                                else if (Mathf.Abs((leftAngleAnkle * 180 / Mathf.PI) - startAngleLeft) * -3 > -4)
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                                    player.transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                                }
                                else
                                {
                                    player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3);
                                }
                            }
                            else
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                                player.transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                            }

                        }
                        else
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                            player.transform.position = new Vector3(0f, transform.position.y, transform.position.z);
                        }
                        rb.velocity = new Vector3(0, 0, speed * Time.deltaTime);
                    }
                }
            }
            if (player.transform.position.z > 1744f)
            {
                percent = Mathf.Round(nrCoins / totalNrCoins * 100);
                SceneManager.LoadScene("Balance 2 Score");
            }

        }

        /// <summary>
        /// Changes a CameraSpacePoint to a vector 
        /// (Kinects sensor returns the body joint in CameraSpacePoint)
        /// </summary>
        /// <param name="point">point from Kinect body joint</param>
        /// <returns></returns>
        private Vector3 GetVector(CameraSpacePoint point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }





        /// <summary>
        ///  The player has to collect the first coin before the game starts properly. 
        ///  This is created as a help for the player to understand the way to collect a coin.
        /// </summary>
        private void infoRound()
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
                //When body is tracked with Kinect
                if (body.IsTracked)
                {
                    //Finds the position of the spine and knees.
                    Vector3 spinePos = GetVector(body.Joints[Windows.Kinect.JointType.SpineBase].Position); ;
                    Vector3 ankleRight = GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position);
                    Vector3 ankleLeft = GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position);


                    //distance between the ankles
                    float d = Mathf.Sqrt(Mathf.Pow((ankleLeft.x - ankleRight.x), 2) + Mathf.Pow((ankleLeft.y - ankleRight.y), 2) + Mathf.Pow((ankleLeft.z - ankleRight.z), 2));

                    //distance from spine to left ankle and distance from spine to right ankle
                    float dSpineLeft = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleLeft.x), 2) + Mathf.Pow((spinePos.y - ankleLeft.y), 2) + Mathf.Pow((spinePos.z - ankleLeft.z), 2));
                    float dSpineRight = Mathf.Sqrt(Mathf.Pow((spinePos.x - ankleRight.x), 2) + Mathf.Pow((spinePos.y - ankleRight.y), 2) + Mathf.Pow((spinePos.z - ankleRight.z), 2));

                    //Find angle of left ankle to floor, uses the law of cosines formula
                    float leftAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineRight, 2) - Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineLeft * d));
                    float rightAngleAnkle = Mathf.Acos((Mathf.Pow(dSpineLeft, 2) - Mathf.Pow(dSpineRight, 2) - Mathf.Pow(d, 2)) / (-2 * dSpineRight * d));



                    //Saves start position, so the player can lean relative to his/hers starting position 
                    //Important since stoke patients often lean more over to one of the legs
                    if (counter == 1)
                    {
                        spineStartPos = spinePos;
                        startAngleLeft = leftAngleAnkle * 180 / Mathf.PI;
                        startAngleRight = rightAngleAnkle * 180 / Mathf.PI;
                        counter++;
                    }


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

                            rb.velocity = new Vector3(0, 0, 150f * Time.deltaTime);

                            //The speed stops so the player has to pick up the first coin before it continues.
                            if (player.transform.position.z > 28f)
                            {
                                rb.velocity = new Vector3(0f, 0f, 0f);
                            }
                        }
                        else if ((rightAngleAnkle * 180 / Mathf.PI) > startAngleRight) //Leans toward right
                        {
                            if (Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3 < -30)
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, -30f);
                            }
                            else
                            {
                                player.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Abs((rightAngleAnkle * 180 / Mathf.PI) - startAngleRight) * -3);
                            }
                            rb.velocity = new Vector3(0f, 0f, 150f * Time.deltaTime);

                        }
                        else
                        {
                            player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                            info.text = "Len deg over mot høyre for å fange mynten";
                            rb.velocity = new Vector3(0, 0, 150f * Time.deltaTime);
                            if (player.transform.position.z > 28f)
                            {
                                rb.velocity = new Vector3(0f, 0f, 0f);
                            }
                        }

                    }
                    else
                    {
                        player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        info.text = "Len deg over mot høyre for å fange mynten";
                        rb.velocity = new Vector3(0, 0, 150f * Time.deltaTime);
                        if (player.transform.position.z > 28f)
                        {
                            rb.velocity = new Vector3(0f, 0f, 0f);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Starts the info audio at the start of the game, and waits to start until the audio is finish
        /// </summary>
        /// <returns>Number of seconds the game has to wait to start</returns>
        IEnumerator infoAudio()
        {
            firstInfoShowed = true;
            Debug.Log("coroutine");
            FindObjectOfType<AudioManager>().Play("Balance2");
            float songLength = GameObject.Find("AudioInfo").GetComponent<UnityEngine.AudioSource>().clip.length;
            Debug.Log(songLength);
            yield return new WaitForSeconds(songLength);
            FindObjectOfType<AudioManager>().Play("Theme");
            infoShowed = true;
            infoRound();
        }

    }
}
