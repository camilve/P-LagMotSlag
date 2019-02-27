using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

using LpmsCSharpWrapper;

public class LpmsTest : MonoBehaviour {

    public Button objectResetButton;
    string lpmsSensor33 = "00:04:3e:4b:33:26";
    string lpmsSensor31 = "00:04:3e:4b:31:ee";
    string lpmsSensor36 = "00:04:3e:30:36:eb";

    // Use this for initialization
    void Start ()
    {
        // Initialize sensor manager
        LpSensorManager.initSensorManager();

        // connects to sensor
        LpSensorManager.connectToLpms(LpSensorManager.DEVICE_LPMS_B2, lpmsSensor33);
        LpSensorManager.connectToLpms(LpSensorManager.DEVICE_LPMS_B2, lpmsSensor31);

        // Wait for establishment of sensor1 connection
        while (LpSensorManager.getConnectionStatus(lpmsSensor33) != 1 && LpSensorManager.getConnectionStatus(lpmsSensor31) != 1)
        {
            System.Threading.Thread.Sleep(100);
        }
        Debug.Log("Sensor connected");

        // Sets sensor offset
        LpSensorManager.setOrientationOffset(lpmsSensor33, LpSensorManager.LPMS_OFFSET_MODE_HEADING);
        LpSensorManager.setOrientationOffset(lpmsSensor31, LpSensorManager.LPMS_OFFSET_MODE_HEADING);
        Debug.Log("Offset set");
    }

    // Update is called once per frame
    void Update () {        
        if(LpSensorManager.getConnectionStatus(lpmsSensor33) == LpSensorManager.SENSOR_CONNECTION_CONNECTED)
        {
            SensorData sd;
            unsafe
            {
                sd = *((SensorData*)LpSensorManager.getSensorData(lpmsSensor33));
            }
            Debug.Log("Timestamp: " + sd.timeStamp + " q: " + sd.qw + " " + sd.qx + " " + sd.qy + " " + sd.qz);
            Quaternion q = new Quaternion( sd.qx, sd.qz, sd.qy, sd.qw);
            transform.rotation = q;
        }

        
    }

    void OnDestroy()
    {
        Debug.Log("PrintOnDestroy");
        LpSensorManager.disconnectLpms(lpmsSensor33);
        LpSensorManager.disconnectLpms(lpmsSensor31);
        // Destroy sensor manager and free up memory
        LpSensorManager.deinitSensorManager();
    }
}
