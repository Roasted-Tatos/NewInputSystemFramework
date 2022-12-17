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
    [SerializeField] private Crate _crate;

    public bool isFlying;
    public bool isDriving;
    public bool canPunch = false;
    private bool chargingPunch = false;

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
        inputActions.Punching.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        SwappingControls();
        LaptopControls();

        isFlying = _drone.isFlying;
        isDriving = _forkLift.isDriving;
        chargingPunch = _crate.canPunchBox;
    }

    private void SwappingControls()
    {
        if(isFlying == true)
        {
            Flying();
            Debug.Log("turnning on flight controls");
        }
        else if(isDriving == true)
        {
            Driving();
            Debug.Log("turning on driving controls");
        }
        else if(chargingPunch == true)
        {
            Punching();
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

    void Punching()
    {
        inputActions.Punching.Punch.started += Punch_started;
        inputActions.Punching.Punch.performed += Punch_performed;
        inputActions.Punching.Punch.canceled += Punch_canceled;
    }

    private void Punch_canceled(InputAction.CallbackContext obj)
    {
        if(canPunch == true)
        {
            _crate.ChargingPunch(false);
            _player.anim.SetTrigger("Punching");
            canPunch = false;
        }
    }

    private void Punch_performed(InputAction.CallbackContext obj)
    {
        _crate.ChargingPunch(true);
        _player.anim.SetTrigger("StrongPunch");
        canPunch=false;
    }

    private void Punch_started(InputAction.CallbackContext obj)
    {
        canPunch = true;
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
