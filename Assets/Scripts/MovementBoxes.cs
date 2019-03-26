using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;

public class MovementBoxes : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player;
    public int score = 0;
    Material p_material;
    Color boxOutside = new Color(0.840f, 0.281f, 0.334f, 1.000f);
    Color boxBetween = new Color(0.000f, 0.749f, 0.000f, 1.000f);

    // Start is called before the first frame update
    void Start()
    {
        p_material = GetComponent<Renderer>().material;
        
    }

    // FixedUpdate because we use it to mess with physics
#warning Usikker på om jeg skal lage en update og en fixedUpdate
    void FixedUpdate()
    {
       
        if (p_material.color == (boxBetween))
        {
            Debug.Log("true:e");
        }

        rb.velocity = new Vector3(0, 0, -500 * Time.deltaTime);
        //rb.AddForce(0, 0, -600 * Time.deltaTime);

        player = GameObject.Find("Player");
        //Debug.Log(player);

        Animator a = player.GetComponent<Animator>();
        Transform playerTransform = player.transform;
        Transform transRight = a.GetBoneTransform(HumanBodyBones.RightFoot);
        Transform transLeft = a.GetBoneTransform(HumanBodyBones.LeftFoot);
        Vector3 positionRight = transRight.position;
        Vector3 positionLeft = transLeft.position;

        //Debug.Log(position1);

        /*Check if the feets are crashing with the obstacles*/
       
        if(this.transform.position.z < (positionRight.z + this.transform.localScale.z /2) && this.transform.position.z > (positionRight.z - this.transform.localScale.z / 2))
        {
            //checks if the box is going on the left side of the left foot and the right foot
            if(this.transform.position.x + (this.transform.localScale.x / 2) < positionLeft.x && this.transform.position.x + (this.transform.localScale.x / 2) < positionRight.x) 
            {
                
                Debug.Log("Boksen går på venstre side av venstre bein");

            }
            //checks if the box is going on the right side of the right foot
            else if (this.transform.position.x - (this.transform.localScale.x / 2) > positionRight.x && this.transform.position.x - (this.transform.localScale.x / 2) > positionLeft.x)
            {
                Debug.Log("Boksen går på høyre side av høyre bein");

            }
            else //The box is going between the left and the right foot
            {
                //Is the box between the left and the right foot
                if (this.transform.position.x + (this.transform.localScale.x / 2) < positionRight.x && this.transform.position.x - (this.transform.localScale.x / 2) > positionLeft.x)
                {
                    Debug.Log("BRA!!!!!!");
                } 
                else if (positionRight.x < positionLeft.x)
                {
                    Debug.Log("Har kryssa beina, what");
                }
                else
                {
                    Debug.Log("Minus poeng!!!!!");
                }
            }
        }

        
    }
}
