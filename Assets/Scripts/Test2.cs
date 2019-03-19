using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class Test2 : MonoBehaviour {
    Animator animator;
    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        Transform trans = animator.GetBoneTransform(HumanBodyBones.Head);
        Debug.Log(trans);
        /*https://csharp.hotexamples.com/examples/UnityEngine/Animator/GetBoneTransform/php-animator-getbonetransform-method-examples.html */

		
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
        

    }

}
