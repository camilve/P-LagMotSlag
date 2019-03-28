using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 4, 0, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") {
            DriveRailway.nrCoins++;
            Destroy(gameObject);
        }
    }
}
