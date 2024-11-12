using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestJoystick : MonoBehaviour
{
    [SerializeField]
    private JoystickController joystickController;

    private void Start()
    {
        joystickController = FindFirstObjectByType<JoystickController>();
        StartCoroutine(CheckJoystick());
    }
    private IEnumerator CheckJoystick()
    {
        while (joystickController != null)
        {
            yield return new WaitForSecondsRealtime(1);
            if (joystickController != null)
                Debug.Log(joystickController.GetHorizontalValue());
        }
    }
}
