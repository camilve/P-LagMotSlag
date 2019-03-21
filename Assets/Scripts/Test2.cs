using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using Kinect = Windows.Kinect;

public class Test2 : MonoBehaviour {
    /* Animator animator;
     public GameObject BodySourceManager;
     private BodySourceManager _BodyManager;


     // Use this for initialization
     void Start () {
         animator = GetComponent<Animator>();
         Transform trans = animator.GetBoneTransform(HumanBodyBones.Head);
         Debug.Log(trans);
         //https://csharp.hotexamples.com/examples/UnityEngine/Animator/GetBoneTransform/php-animator-getbonetransform-method-examples.html 


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
                 var pos = body.Joints[Windows.Kinect.JointType.HandRight].Position;
                 var orientation = body.JointOrientations[Windows.Kinect.JointType.HandRight].Orientation;
                 animator.SetBoneLocalRotation(HumanBodyBones.RightHand, new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W));

                 //this.gameObject.transform.position = new Vector3(pos.X * 10, pos.Y * 10, pos.Z * 10);
                 //this.gameObject.transform.rotation = new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W);
                 break;
             }
         }


     }*/

    protected Animator animator;
    private GameObject player;
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;


    void Start ()
    {
        player = gameObject;
        animator = player.GetComponent<Animator>();
        //animator = GetComponent<Animator>();
        Debug.Log(animator);
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


                //Place the gameobject with the spine as the centre.
                player.transform.position = GetVector(body.Joints[Kinect.JointType.SpineBase].Position);

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


                animator.SetIKPosition(AvatarIKGoal.LeftFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootLeft].Position));
                //animator.SetIKRotation(AvatarIKGoal.LeftFoot, new Quaternion(body.Joints[Windows.Kinect.JointType.FootLeft].Position.X, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Y, body.Joints[Windows.Kinect.JointType.FootLeft].Position.Z));
                animator.SetIKPosition(AvatarIKGoal.RightFoot, GetVector(body.Joints[Windows.Kinect.JointType.FootRight].Position));

                animator.SetIKPosition(AvatarIKGoal.LeftHand, GetVector(body.Joints[Windows.Kinect.JointType.HandLeft].Position));
                animator.SetIKPosition(AvatarIKGoal.RightHand, GetVector(body.Joints[Windows.Kinect.JointType.HandRight].Position));

                animator.SetIKHintPosition(AvatarIKHint.LeftKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeLeft].Position));
                animator.SetIKHintPosition(AvatarIKHint.RightKnee, GetVector(body.Joints[Windows.Kinect.JointType.KneeRight].Position));

                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowLeft].Position));
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, GetVector(body.Joints[Windows.Kinect.JointType.ElbowRight].Position));

                //animator.SetLookAtPosition(new Vector3(body.Joints[Windows.Kinect.JointType.Head].Position.X, body.Joints[Windows.Kinect.JointType.Head].Position.Y, body.Joints[Windows.Kinect.JointType.Head].Position.Z));

                /*
                if (GetVector(body.Joints[Kinect.JointType.ShoulderLeft].Position) != Vector3.zero && GetVector(body.Joints[Windows.Kinect.JointType.ShoulderRight].Position) != Vector3.zero)
                {
                    float _leftShoulderX = body.Joints[Kinect.JointType.ShoulderLeft].Position.X;
                    float _leftShoulderY = body.Joints[Kinect.JointType.ShoulderLeft].Position.Y;
                    float _leftShoulderZ = body.Joints[Kinect.JointType.ShoulderLeft].Position.Z;
                    float _rightShoulderX = body.Joints[Kinect.JointType.ShoulderRight].Position.X;
                    float _rightShoulderY = body.Joints[Kinect.JointType.ShoulderRight].Position.Y;
                    float _rightShoulderZ = body.Joints[Kinect.JointType.ShoulderRight].Position.Z;

                    float _absoluteX = Mathf.Abs(_leftShoulderX) + Mathf.Abs(_rightShoulderX);
                    float _absoluteZ = Mathf.Abs(_leftShoulderZ - _rightShoulderZ);

                    float _division = _absoluteX / _absoluteZ;
                    float _spineAngleRadians = Mathf.Atan(_division);

                    if (_leftShoulderZ > _rightShoulderZ)
                    {
                        _spineAngle = 90 + (_spineAngleRadians * (180.0f / Mathf.PI));
                    }
                    else
                    {
                        _spineAngle = 270 - (_spineAngleRadians * (180.0f / Mathf.PI));
                    }
                }
                _player.transform.rotation = Quaternion.AngleAxis(-_spineAngle, Vector3.up);
                */
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
        return new Vector3(point.X, point.Y, point.Z);
    }

}
