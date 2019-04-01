using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;

public class Balance1Script : MonoBehaviour {

    protected Animator animator;
    private GameObject player;
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;

    public GameObject Left;
    public GameObject Right;
    public GameObject Middle;


    void Start ()
    {
        player = gameObject;
        animator = player.GetComponent<Animator>();
        //animator = GetComponent<Animator>();
        Debug.Log(animator);


        Vector3 prevPos = new Vector3(0.16f, 2f, 0f);
        int prevRandom = 2;
        Instantiate(Middle, prevPos, Quaternion.identity);
        while (prevPos.z < 540f)
        {
            Vector3 pos = new Vector3(0.16f, 2f, 0f);
            pos.z = prevPos.z + 7f;
            int randomVal = Random.Range(0, 3);
            

            if(randomVal == 0)
            {
                //Left side             
                if (prevRandom == 1)
                {
                    pos.z = prevPos.z + 11f;
                }

                Instantiate(Left, pos, Quaternion.identity);
            }
            else if (randomVal == 1)
            {
                //Right side              
                if (prevRandom == 0)
                {
                    pos.z = prevPos.z + 10f;
                }
                Instantiate(Right, pos, Quaternion.identity);
            }
            else if (randomVal == 2)
            {                            
                Instantiate(Middle, pos, Quaternion.identity);
            }         
            

            prevPos = pos;
            prevRandom = randomVal;

        }
        

       /* boxBetween = GameObject.Find("obstacle1");
        boxOutside = GameObject.Find("obstacle2");
        boxBetween.GetComponent<MovementBoxes>().enabled = false;
        boxOutside.GetComponent<MovementBoxes>().enabled = false;*/
    }

    void Update()
    {
        
    }


    //a callback for calculating IK
    void OnAnimatorIK()
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
                //Start the boxes
                /*boxBetween.GetComponent<MovementBoxes>().enabled = true;
                boxOutside.GetComponent<MovementBoxes>().enabled = true;
                */


                //Default position player (-0.00532963, 0.594, -8.363)
                Vector3 spineBase = GetVector(body.Joints[Kinect.JointType.SpineBase].Position);

                float diffX = spineBase.x - (spineBase.x);
                float diffY = spineBase.y - (1.25f);
                float diffZ = spineBase.z - (-8.363f);
                Vector3 diff = new Vector3(diffX, diffY, diffZ);


                //Place the gameobject with the spine as the centre.
                player.transform.position = GetVector(body.Joints[Kinect.JointType.SpineBase].Position) - diff;

                //Place the feet the same place as the Kinect.            
#warning Vet ikke om foten går frem eller tilbake
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);


                animator.SetIKPosition(AvatarIKGoal.LeftFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootLeft].Position)-diff);
                //animator.SetIKRotation(AvatarIKGoal.LeftFoot, new Quaternion(body.Joints[Windows.Kinect.JointType.FootLeft].Position.X, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Y, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Z));
                animator.SetIKPosition(AvatarIKGoal.RightFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootRight].Position)-diff);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, GetVector(body.Joints[Windows.Kinect.JointType.HandLeft].Position)-diff);
                animator.SetIKPosition(AvatarIKGoal.RightHand, GetVector(body.Joints[Windows.Kinect.JointType.HandRight].Position)-diff);

                animator.SetIKHintPosition(AvatarIKHint.LeftKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position)-diff);
                animator.SetIKHintPosition(AvatarIKHint.RightKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position)-diff);

                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowLeft].Position)-diff);
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowRight].Position)-diff);

                //animator.SetLookAtPosition(new Vector3(body.Joints[Windows.Kinect.JointType.Head].Position.X, body.Joints[Windows.Kinect.JointType.Head].Position.Y, body.Joints[Windows.Kinect.JointType.Head].Position.Z));

                
            }
        }



        
    }
    /* Try to make the code better with this...
    private int getBodypartNr(string bodypart)
    {
        int nr = -1;
        nr = Windows.Kinect.JointType. bodypart

        Vector3 position = new Vector3(, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Y, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Z);    
    }
    */

    private Vector3 GetVector(CameraSpacePoint point)
    {
        return new Vector3(point.X, point.Y, -point.Z);
    }

}
