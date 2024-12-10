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
    private BoundriesController boundriesController;

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
        boundriesController = FindFirstObjectByType<BoundriesController>()
            .GetComponent<BoundriesController>();
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
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    private IEnumerator WaitForJoystickMovement()
    {
        while (isPlayerControllable)
        {
            yield return new WaitUntil(() => isMoving);
            ApplyJoystickMovement();
        }
    }

    private IEnumerator WaitForPlayerControllable()
    {
        yield return new WaitUntil(() => isPlayerControllable);
        StartCoroutine(WaitForJoystickMovement());
    }

    private void ApplyJoystickMovement()
    {
        float speed = PlayerStats.playerSpeed; //Replace after integrating with statsmanager
        Vector3 updatedPosition = joystickValue * Time.deltaTime * speedModifier;
        transform.position += updatedPosition;
        float boundryX = boundriesController.playerBoundries.x / 2;
        float boundryY = boundriesController.playerBoundries.y / 2;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -boundryX, boundryX ),
            Mathf.Clamp(transform.position.y, -boundryY, boundryY),
            transform.position.z
            );
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
