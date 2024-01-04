using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetInputSystemDevice : MonoBehaviour
{
    Transform playerParent;
    private void Start()
    {
        playerParent = GameObject.FindGameObjectWithTag("Animation").transform;
        InvokeRepeating("CheckDevice", 0f, 1f); //1�b�Ɉ����s
    }
    void CheckDevice()
    {
        Dictionary<int, InputDevice> inputDevices = new Dictionary<int, InputDevice>(InputSystem.devices.Count);

        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            if (inputDevices.Count > 4)
                return;
            
            var device = InputSystem.devices[i];

            if (!(device is Gamepad || device is Keyboard))
            {
                Debug.Log(device);    
                continue;
            }

            Debug.Log(inputDevices.Count);    
            Debug.Log(playerParent.GetChild(inputDevices.Count).tag);
            inputDevices.Add(GameMaster.GetTeamID(playerParent.GetChild(inputDevices.Count).tag), device);
        }
        GameMaster.ChangeGamePad(inputDevices);
    }
}
