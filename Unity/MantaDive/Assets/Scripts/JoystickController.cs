using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickController : MonoBehaviour
{
    private InputAction slideJoystick;
    [SerializeField]
    private InputActionAsset inputActionAsset;
    private Vector2 lastCoordinates = Vector2.zero;
    private void OnEnable()
    {
        slideJoystick = inputActionAsset.FindAction("Player/Move");
        slideJoystick.Enable();
        slideJoystick.performed += OnJoystickMoved;
        slideJoystick.canceled += OnJoystickLetGo;
    }

    private void OnDisable()
    {
        slideJoystick.performed -= OnJoystickMoved;
        slideJoystick.canceled -= OnJoystickLetGo;
    }

    private void OnJoystickMoved(InputAction.CallbackContext context)
    {
        Vector2 joystickValue = context.ReadValue<Vector2>();
        lastCoordinates = joystickValue;
    }

    private void OnJoystickLetGo(InputAction.CallbackContext context)
    {
        lastCoordinates = Vector2.zero;
    }
    public float GetHorizontalValue()
    {
        return lastCoordinates.x;
    }
}
