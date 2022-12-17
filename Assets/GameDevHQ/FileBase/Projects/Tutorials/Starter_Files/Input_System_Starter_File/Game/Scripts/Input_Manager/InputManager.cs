using Game.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameInputActions inputActions;
    [SerializeField] private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    void Initialized()
    {
        inputActions = new GameInputActions();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        var inputDirections = inputActions.Player.Movement.ReadValue<Vector3>();
        _player.InputMovement(inputDirections);
    }
}
