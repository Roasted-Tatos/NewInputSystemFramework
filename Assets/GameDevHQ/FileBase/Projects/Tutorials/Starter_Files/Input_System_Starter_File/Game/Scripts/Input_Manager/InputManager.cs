using Game.Scripts.LiveObjects;
using Game.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameInputActions inputActions;
    [SerializeField] private Player _player;
    [SerializeField] private Laptop _laptop;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    void Initialized()
    {
        inputActions = new GameInputActions();
        inputActions.Player.Enable();
        inputActions.Laptop.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        LaptopControls();
    }

    void Movement()
    {
        var inputDirections = inputActions.Player.Movement.ReadValue<Vector3>();
        _player.InputMovement(inputDirections);
    }
    void LaptopControls()
    {
        inputActions.Laptop.Swapping_Cameras.performed += Swapping_Cameras_performed;
        inputActions.Laptop.Escape.performed += Escape_performed;
    }

    private void Escape_performed(InputAction.CallbackContext obj)
    {
        _laptop.ReturnCamera();
    }

    private void Swapping_Cameras_performed(InputAction.CallbackContext obj)
    {
        _laptop.SwappingCameras();
    }
}
