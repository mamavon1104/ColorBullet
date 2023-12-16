using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetInputSystemDevice : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("CheckDevice", 0f, 1f); //1ïbÇ…àÍâÒé¿çs
    }
    void CheckDevice()
    {
        var playerParent = GameObject.FindGameObjectWithTag("Animation").transform;
        Dictionary<int, InputDevice> _inputDevices = new Dictionary<int, InputDevice>();

        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            var device = InputSystem.devices[i];

            if (!(device is Gamepad && device is Keyboard))
                return;

            _inputDevices.Add(GameMaster.GetTeamID(playerParent.GetChild(i).tag), Gamepad.all[i]);
        }
        GameMaster.AddGamePad(_inputDevices);
    }
}
