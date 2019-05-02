using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;
using Kinect = Windows.Kinect;
using UnityEngine.SceneManagement;

/// <summary>
/// For the first exsercise. To make the humaniod move as the player moves.
/// Creates obstacles randomly.
/// </summary>
public class Balance1Script : MonoBehaviour {

    protected Animator animator;
    private GameObject player;
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;
    public GameObject Left;
    public GameObject Right;
    public GameObject Middle;

    public Text points;

    private List<GameObject> prefabList;
    public static bool enableBoxes = false;

    public static int totalBoxes;

    public static List<GameObject> infoBoxes;

    private bool infoShowed = false;
    private int infoCounter = 0;

    void Start ()
    {
        totalBoxes = 0;
        player = gameObject;
        animator = player.GetComponent<Animator>();
        infoBoxes = new List<GameObject>();
        prefabList = new List<GameObject>();


        //Place the obstacles randomly. Make sure the obstacles does not come to close to each other.
        Vector3 prevPos = new Vector3(0.16f, 2f, 0f);
        int prevRandom = 2;        
        infoBoxes.Add(Instantiate(Middle, prevPos, Quaternion.identity));
        totalBoxes++;
        prevPos.z = prevPos.z + 10f;
        infoBoxes.Add(Instantiate(Right, prevPos, Quaternion.identity));
        totalBoxes++;
        while (prevPos.z < 540f)
        {
            Vector3 pos = new Vector3(0.16f, 2f, 0f);

            float randomZpos = Random.Range(7f, 12f);

            pos.z = prevPos.z + randomZpos;
            int randomVal = Random.Range(0, 3);

            //Left side 
            if (randomVal == 0)
            {
                //Checks if last obstacle was to go to right, because then it is longer space between them            
                if (prevRandom == 1)
                {
                    pos.z = prevPos.z + randomZpos + 5f;
                }

                GameObject l = Instantiate(Left, pos, Quaternion.identity);
                prefabList.Add(l);
            }
            //Right side              
            else if (randomVal == 1)
            {
                if (prevRandom == 0)
                {
                    pos.z = prevPos.z + randomZpos + 5f;
                }
                GameObject l = Instantiate(Right, pos, Quaternion.identity);
                prefabList.Add(l);
            }
            //Stand in the middle
            else if (randomVal == 2)
            {
                GameObject l = Instantiate(Middle, pos, Quaternion.identity);
                prefabList.Add(l);
            }           

            prevPos = pos;
            prevRandom = randomVal;

        }
        totalBoxes = prefabList.Count + infoBoxes.Count;



        //Stops the boxes from moving before the game starts.
        foreach (GameObject boxes in prefabList)
        {
            boxes.GetComponent<MovementBoxes>().enabled = false;
        }
        foreach (GameObject boxes in infoBoxes)
        {
            boxes.GetComponent<MovementBoxes>().enabled = false;
        }
        
        

    }


    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (!infoShowed)
        {
            FindObjectOfType<AudioManager>().Stop("Theme");
            StartCoroutine(infoAudio());
        }
        else
        {
            //Checks if there is someone in the camera, otherwise the boxes won't move.
            if (BodySourceManager == null)
            {
                Debug.Log("1");
                enablePrefabList(false);
                return;
            }

            _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
            if (_BodyManager == null)
            {
                Debug.Log("2");
                enablePrefabList(false);
                return;
            }

            Kinect.Body[] data = _BodyManager.GetData();
            if (data == null)
            {
                Debug.Log("3 " + _BodyManager);
                enablePrefabList(false);

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

                    //Start the boxes
                    foreach (GameObject boxes in infoBoxes)
                    {
                        boxes.GetComponent<MovementBoxes>().enabled = true;
                    }
                    enablePrefabList(enableBoxes);

                    points.text = "" + MovementBoxes.score;


                    //Default position player (-0.00532963, 0.594, -8.363), calculates so the avatar is placed where we want it.
                    Vector3 spineBase = GetVector(body.Joints[Kinect.JointType.SpineBase].Position);

                    float diffX = spineBase.x - (spineBase.x);
                    float diffY = spineBase.y - (1.25f);
                    float diffZ = spineBase.z - (-8.363f);
                    Vector3 diff = new Vector3(diffX, diffY, diffZ);


                    //Place the gameobject with the spine as the centre.
                    player.transform.position = GetVector(body.Joints[Kinect.JointType.SpineBase].Position) - diff;

                    //Place the feet the same place as the Kinect.            
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);


                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootLeft].Position) - diff);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootRight].Position) - diff);

                    animator.SetIKPosition(AvatarIKGoal.LeftHand, GetVector(body.Joints[Windows.Kinect.JointType.HandLeft].Position) - diff);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, GetVector(body.Joints[Windows.Kinect.JointType.HandRight].Position) - diff);

                    animator.SetIKHintPosition(AvatarIKHint.LeftKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position) - diff);
                    animator.SetIKHintPosition(AvatarIKHint.RightKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position) - diff);

                    animator.SetIKHintPosition(AvatarIKHint.LeftElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowLeft].Position) - diff);
                    animator.SetIKHintPosition(AvatarIKHint.RightElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowRight].Position) - diff);

                }
            }
        }


     
        //Show the scoreboard when the player has finished the last obstacle
        if (prefabList[prefabList.Count - 1].transform.position.z < this.transform.position.z) 
        {
            SceneManager.LoadScene("Balance 1 Score");
        }
    }

    /// <summary>
    /// Gets the vector from the CameraSpacePoint. Since the Kinect's forth is the avatar's backward it is -z. 
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private Vector3 GetVector(CameraSpacePoint point)
    {
        return new Vector3(point.X, point.Y, -point.Z);
    }


    /// <summary>
    /// Playes the information before the game begins
    /// </summary>
    /// <returns>the time in seconds of the information audio</returns>
    IEnumerator infoAudio()
    {
        //So the information only starts once
        if (infoCounter == 0)
        {
            infoCounter++;
            FindObjectOfType<AudioManager>().Play("Balance1");
            float songLength = GameObject.Find("AudioInfo").GetComponent<UnityEngine.AudioSource>().clip.length;
            yield return new WaitForSeconds(songLength);
            FindObjectOfType<AudioManager>().Play("Theme");
            infoShowed = true;
        } 

    }

    /// <summary>
    /// eenables or disables the script of the boxes in the prefablist 
    /// </summary>
    /// <param name="enable"></param>
    private void enablePrefabList(bool enable)
    {
        foreach (GameObject boxes in prefabList)
        {
            boxes.GetComponent<MovementBoxes>().enabled = enable;
        }
    }

}
