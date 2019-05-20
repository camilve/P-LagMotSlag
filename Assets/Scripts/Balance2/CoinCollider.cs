using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AudioSpace;

namespace Balance2Space
{
    /// <summary>
    /// Makes the coins rotate during the game, and gives the player points as it collides with the coin.
    /// </summary>
    public class CoinCollider : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(0, 4, 0, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "Player")
            {
                DriveRailway.nrCoins++;
                FindObjectOfType<AudioManager>().Play("CoinCollect");
                Destroy(gameObject);
            }
        }
    }
}
