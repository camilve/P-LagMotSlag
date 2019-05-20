using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSpace
{
    /// <summary>
    /// Makes the camera follow the cart when it's driving and collecting coins
    /// </summary>
    public class CameraFollowB2 : MonoBehaviour
    {
        public GameObject player;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = player.transform.position;
            pos.y += 13f;
            pos.z -= 25f;
            transform.position = pos;
        }
    }
}
