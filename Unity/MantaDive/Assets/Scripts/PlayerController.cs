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

    //Player stats for the run
    [SerializeField]
    private float stamina;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float magnetStrength;

    //Movement
    private Vector3 joystickValue;
    private bool isMoving = false;
    [SerializeField]
    private bool isPlayerControllable = false;
    [SerializeField]
    private float speedModifier = 0.1f;
    private BoundriesController boundriesController;
    private float staminaLossSpeed = 0.5f;

    [SerializeField]
    LevelController levelController;

    private void Start()
    {
        LoadPlayerStats();
        StartCoroutine(ReduceStamina());
    }
    private void OnEnable()
    {
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
        Vector3 updatedPosition = speed * joystickValue * Time.deltaTime * speedModifier;
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

    public void LoadPlayerStats()
    {
        //stamina = PlayerStatsManager.GetPlayerMaxStamina();
        //speed = PlayerStatsManager.GetPlayerBaseSpeed();
        //magnetStrength = PlayerStatsManager.GetPlayerMagnetStrength();
        stamina = 100;
        speed = 2;
        magnetStrength = 5;
    }

    private IEnumerator ReduceStamina()
    {
        Debug.Log("Reducing stamina");
        yield return new WaitUntil(() => LevelController.isRunOngoing);
        while (LevelController.isRunOngoing)
        {
            yield return new WaitUntil(() => isPlayerControllable);
            yield return new WaitForSeconds(staminaLossSpeed);
            stamina -= 1;
            string currentStaminaText = levelController.StaminaText.text.Split(' ')[0];
            currentStaminaText += " " + stamina.ToString();
            levelController.StaminaText.text = currentStaminaText;
            if (stamina <= 0)
            {
                LevelController.isRunOngoing = false;
                yield break;
            }
        }
    }

    public void UpdateCurrentStaminaByValue(int value) 
    {
        stamina += value;
    }

    public void UpdateMagnetStrengthTemp(float strength, float time)
    {
        StartCoroutine(TempUpdate(strength,0,time));
    }

    public void UpdatePlayerSpeedTemp(float speed, float time)
    {
        StartCoroutine(TempUpdate(0,speed,time));

    }

    private IEnumerator TempUpdate(float magnetUpdate, float speedUpdate, float time)
    {
        magnetStrength += magnetUpdate;
        speed += speedUpdate;
        yield return new WaitForSeconds(time);
        magnetStrength -= magnetUpdate;
        speed -= speedUpdate;
    }

}
