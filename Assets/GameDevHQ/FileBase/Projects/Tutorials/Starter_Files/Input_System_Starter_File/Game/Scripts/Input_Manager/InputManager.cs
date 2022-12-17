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
    [SerializeField] private Drone _drone;
    [SerializeField] private Forklift _forkLift;

    public bool isFlying;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();

        isFlying = _drone.isFlying;
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
        SwappingControls();
        LaptopControls();
    }

    private void SwappingControls()
    {
        if(_drone.isFlying == true)
        {
            Flying();
            Debug.Log("turnning on flight controls");
        }
        else if(_forkLift.isDriving == true)
        {
            Driving();
            Debug.Log("turning on driving controls");
        }
        else
        {
            Movement();
        }
    }

    void Movement()
    {
        inputActions.Drone.Disable();
        inputActions.ForkLift.Disable();
        inputActions.Player.Enable();
        var inputDirections = inputActions.Player.Movement.ReadValue<Vector3>();
        _player.InputMovement(inputDirections);
    }

    void Flying()
    {
        inputActions.Player.Disable();
        inputActions.Drone.Enable();

        var flyingDirections = inputActions.Drone.Movement.ReadValue<Vector3>();
        _drone.MovementInputs(flyingDirections);

        var RotationDirections = inputActions.Drone.Rotate.ReadValue<float>();
        _drone.RotateInput(RotationDirections);

        var HeightDirections = inputActions.Drone.Flight.ReadValue<Vector3>();
        _drone.HeightInput(HeightDirections);

        inputActions.Drone.Escape.performed += Escape_performed1;
    }

    private void Escape_performed1(InputAction.CallbackContext obj)
    {
        _drone.CancelFlightMode();
    }

    private void Driving()
    {
        inputActions.Player.Disable();
        inputActions.ForkLift.Enable();

        var MovementDirections = inputActions.ForkLift.Movement.ReadValue<Vector3>();
        _forkLift.MovementControl(MovementDirections);
        inputActions.ForkLift.LiftArm.performed += LiftArm_performed;
        inputActions.ForkLift.LowerArm.performed += LowerArm_performed;
        inputActions.ForkLift.Escape.performed += Escape_performed2;
    }

    private void Escape_performed2(InputAction.CallbackContext obj)
    {
        _forkLift.ExitDriveMode();
    }

    private void LowerArm_performed(InputAction.CallbackContext obj)
    {
        _forkLift.LiftDownRoutine();
    }

    private void LiftArm_performed(InputAction.CallbackContext obj)
    {
        _forkLift.LiftUpRoutine();
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
