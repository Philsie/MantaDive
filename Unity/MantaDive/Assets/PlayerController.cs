using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;
    private InputAction joystick;

    [SerializeField]
    private PlayerStats PlayerStats;

    private Vector3 joystickValue;
    private bool isMoving = false;
    [SerializeField]
    private bool isPlayerControllable = false;
    [SerializeField]
    private float speedModifier = 0.1f;
    //TODO: Integrate with PlayerStatsManager
    //private float playerSpeed = 0;

    private void OnEnable()
    {
        // Statsmanager get playerSpeed
        joystick = inputActionAsset.FindAction("Player/Move");
        if (joystick != null)
        {
            joystick.performed += OnMovePerformed;
            joystick.canceled += OnMoveCanceled;
            isPlayerControllable = true;
        }
        StartCoroutine(WaitForPlayerControllable());
    }
    private void OnDisable()
    {
        joystick.performed -= OnMovePerformed;
        joystick.canceled -= OnMoveCanceled;
        joystick = null;
        StopCoroutine(WaitForJoystickMovement());
        isPlayerControllable = false;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        joystickValue = context.ReadValue<Vector2>();
        isMoving = true;
        Debug.Log(joystickValue);
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    private IEnumerator WaitForJoystickMovement()
    {
        while (isPlayerControllable)
        {
            Debug.Log("Waiting for movement");
            yield return new WaitUntil(() => isMoving);
            Debug.Log("Movement done");
            ApplyJoystickMovement();
        }
    }

    private IEnumerator WaitForPlayerControllable()
    {
        Debug.Log("Waiting for controll");
        yield return new WaitUntil(() => isPlayerControllable);
        Debug.Log("Controll on");
        StartCoroutine(WaitForJoystickMovement());
    }

    private void ApplyJoystickMovement()
    {
        float speed = PlayerStats.playerSpeed; //Replace after integrating with statsmanager
        transform.position += joystickValue * Time.deltaTime * speedModifier;
    }

    public void SetPlayerControllable(bool isControllable)
    {
        isPlayerControllable = isControllable;
        if (!isPlayerControllable)
        {
            StartCoroutine(WaitForPlayerControllable());
        }
    }

    //Remove after integrating with statsmanager
    public void LoadPlayerStats(PlayerStats playerStats)
    {
        PlayerStats = playerStats;
    }
}
