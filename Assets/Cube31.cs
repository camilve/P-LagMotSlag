using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

using LpmsCSharpWrapper;

public class Cube31 : MonoBehaviour
{

    public Button objectResetButton;
    string lpmsSensor31 = "00:04:3e:4b:31:ee";
    float prevSpeedX = 0.0f;
    float prevSpeedY = 0.0f;
    float prevSpeedZ = 0.0f;
    double prevTimestamp = 0.0;
    float prevAX = 0.0f;
    float prevAY = 0.0f;
    float prevAZ = 0.0f;

    // Use this for initialization
    void Start()
    {
        // Initialize sensor manager
        LpSensorManager.initSensorManager();

        // connects to sensor
        LpSensorManager.connectToLpms(LpSensorManager.DEVICE_LPMS_B2, lpmsSensor31);

        // Wait for establishment of sensor1 connection
        while (LpSensorManager.getConnectionStatus(lpmsSensor31) != 1)
        {
            System.Threading.Thread.Sleep(100);
        }
        Debug.Log("Sensor connected");

        // Sets sensor offset
        LpSensorManager.setOrientationOffset(lpmsSensor31, LpSensorManager.LPMS_OFFSET_MODE_HEADING);
        Debug.Log("Offset set");
    }

    // Update is called once per frame
    void Update()
    {
        if (LpSensorManager.getConnectionStatus(lpmsSensor31) == LpSensorManager.SENSOR_CONNECTION_CONNECTED)
        {
            SensorData sd;
            unsafe
            {
                sd = *((SensorData*)LpSensorManager.getSensorData(lpmsSensor31));
            }
            //Debug.Log("Timestamp: " + sd.timeStamp + " q: " + sd.qw + " " + sd.qx + " " + sd.qy + " " + sd.qz);
            Quaternion q = new Quaternion(sd.qx, sd.qz, sd.qy, sd.qw);
            transform.rotation = q;

            // Quaternion qinverse = Quaternion.Inverse(q);
            //Debug.Log("inverse q " + qinverse.w + " " + qinverse.x + " " + qinverse.y + " " + qinverse.z);

            float gravity = -9.80665f;

            Vector3 gravityAcc = transform.rotation * new Vector3(0, gravity, 0);

            Vector3 absoluteAccGravity = transform.rotation * new Vector3(sd.ax* gravity, sd.az* gravity, sd.ay* gravity);

            Vector3 absoluteAcc = absoluteAccGravity + gravityAcc;

            // Debug.Log("Akselerasjon " + accrt.x + " " + accrt.y + " " + accrt.z);

            //sd.ax is calibrated accelerometer sensor data

            if ((sd.timeStamp/1000) - prevTimestamp > 0.03)
            {
                Debug.Log("Akselerasjon " + absoluteAcc.x + " " + absoluteAcc.y + " " + absoluteAcc.z);
                Debug.Log(transform.rotation);

                /*
                double t = (sd.timeStamp/1000) - prevTimestamp;
                prevTimestamp = (sd.timeStamp / 1000);
                Debug.Log("timestamp: " + (sd.timeStamp/1000) + "sec. t: " + t);
                Debug.Log("Akselerasjon " + accrt.x + " " + accrt.y + " " + accrt.z);
                float posX = prevSpeedX * (float)t + 0.5f*(prevAX + accrt.x) / 2.0f * (float)t * (float)t;
                float posY = prevSpeedY * (float)t + 0.5f*(prevAY + accrt.y) / 2.0f * (float)t * (float)t;
                float posZ = prevSpeedZ * (float)t + 0.5f*(prevAZ + accrt.z) / 2.0f * (float)t * (float)t;

                float prevPosX = transform.position.x;
                float prevPosY = transform.position.y;
                float prevPosZ = transform.position.z;

                Debug.Log("pos " + posX + " " + posY + " " + posZ);

                transform.position = new Vector3(prevPosX + posX, prevPosY + posY, prevPosZ + posZ);

                
                prevSpeedX = prevSpeedX + (prevAX + accrt.x) / 2.0f * (float)t;
                prevSpeedY = prevSpeedY + (prevAY + accrt.y) / 2.0f * (float)t;
                prevSpeedZ = prevSpeedZ + (prevAZ + accrt.z) / 2.0f * (float)t;
                prevAX = accrt.x;
                prevAY = accrt.y;
                prevAZ = accrt.z;*/
            }


        }


    }

    void OnDestroy()
    {
        Debug.Log("PrintOnDestroy");
        LpSensorManager.disconnectLpms(lpmsSensor31);
        // Destroy sensor manager and free up memory
        LpSensorManager.deinitSensorManager();
    }
}