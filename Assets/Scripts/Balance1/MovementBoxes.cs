using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using AudioSpace;
using SwitchSpace;

namespace Balance1Space
{
    /// <summary>
    /// Moves the obstacle boxes in the first game, and gives point if the player does not collide with the boxes.
    /// </summary>
    public class MovementBoxes : MonoBehaviour
    {
        public Rigidbody rb;
        protected GameObject player;
        public static int score;

        private bool isGivenPoint = false;

        void Start()
        {
            score = 0;
            Balance1Script.enableBoxes = true;

        }

        void Update()
        {
            player = GameObject.Find("Player");

            Animator a = player.GetComponent<Animator>();
            Transform playerTransform = player.transform;
            Transform transRight = a.GetBoneTransform(HumanBodyBones.RightFoot);
            Transform transLeft = a.GetBoneTransform(HumanBodyBones.LeftFoot);
            Vector3 positionRight = transRight.position;
            Vector3 positionLeft = transLeft.position;


            Transform thisTransform = this.transform;

            //The first two boxes is going slower and stops if the user does it wrong. Help for the user to understand the game.
            if (score < 2)
            {
                if (Mathf.Abs(positionRight.z - (this.transform.position.z - this.transform.localScale.z / 2)) < 0.5f)
                {
                    bool hit = false;

                    for (int i = 0; i < thisTransform.childCount; i++)
                    {
                        Vector3 p = new Vector3(0f, 0f, 0f);
                        Vector3 scale = new Vector3(0f, 0f, 0f);
                        p = thisTransform.GetChild(i).position;
                        scale = thisTransform.GetChild(i).localScale;
                        if ((p.x - (scale.x / 2) < positionLeft.x && p.x + (scale.x / 2) > positionLeft.x) || (p.x - (scale.x / 2) < positionRight.x && p.x + (scale.x / 2) > positionRight.x))
                        {
                            hit = true;
                        }


                    }
                    if (hit)
                    {
                        foreach (GameObject boxes in Balance1Script.infoBoxes)
                        {
                            boxes.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                        }
                        Balance1Script.enableBoxes = false;
                    }
                    else
                    {
                        foreach (GameObject boxes in Balance1Script.infoBoxes)
                        {
                            boxes.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -50f * Time.deltaTime);
                        }

                        Balance1Script.enableBoxes = true;
                        if (!isGivenPoint)
                        {
                            FindObjectOfType<AudioManager>().Play("BoxPoint");
                            score++;
                            isGivenPoint = true;
                        }

                    }
                }


                else
                {
                    rb.velocity = new Vector3(0f, 0f, -50f * Time.deltaTime);
                }

            }
            else
            {
                //The speed is relative to the level the user has chosen. 
                if (SwitchScene.lvl == 1)
                {
                    rb.velocity = new Vector3(0, 0, -50 * Time.deltaTime);
                }
                else if (SwitchScene.lvl == 2)
                {
                    rb.velocity = new Vector3(0, 0, -100 * Time.deltaTime);
                }
                else
                {
                    rb.velocity = new Vector3(0, 0, -150 * Time.deltaTime);
                }



                if (this.transform.position.z < (positionRight.z + this.transform.localScale.z / 2) && this.transform.position.z > (positionRight.z - this.transform.localScale.z / 2))
                {
                    bool point = true;
                    for (int i = 0; i < thisTransform.childCount; i++)
                    {
                        Vector3 p = new Vector3(0f, 0f, 0f);
                        Vector3 scale = new Vector3(0f, 0f, 0f);

                        p = thisTransform.GetChild(i).position;
                        scale = thisTransform.GetChild(i).localScale;

                        if (!(p.x - (scale.x / 2) < positionLeft.x && p.x + (scale.x / 2) > positionLeft.x) && !(p.x - (scale.x / 2) < positionRight.x && p.x + (scale.x / 2) > positionRight.x))
                        {
                            Debug.Log(p.x - (scale.x / 2) + "   " + positionLeft.x + "  " + positionRight.x);
                        }
                        else
                        {
                            point = false;
                        }
                    }
                    if (point)
                    {
                        if (!isGivenPoint)
                        {
                            score++;
                            FindObjectOfType<AudioManager>().Play("BoxPoint");
                            isGivenPoint = true;
                        }
                    }
                }
            }
        }
    }
}
