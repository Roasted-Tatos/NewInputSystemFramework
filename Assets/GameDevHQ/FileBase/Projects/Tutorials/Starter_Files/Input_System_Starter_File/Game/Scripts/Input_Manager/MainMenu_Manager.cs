using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    [SerializeField] private GameInputActions inputActions;
    [SerializeField] private GameObject _explosions;

    public bool isExploding = false;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new GameInputActions();
        inputActions.MainMenu.Enable();
        inputActions.MainMenu.Explosion.performed += Explosion_performed;
        //inputActions.MainMenu.Start.performed += Start_performed;
    }

    public void Start_performed()
    {
        Debug.Log("start game");
        StartCoroutine(StartGameDelay());
    }

    private void Explosion_performed(InputAction.CallbackContext obj)
    {
        if(isExploding == false)
        {
            _explosions.SetActive(true);
            isExploding = true;
            StartCoroutine(ExplosionReset());
        }
    }


    IEnumerator ExplosionReset()
    {
        yield return new WaitForSeconds(5f);
        _explosions.SetActive(false);
        isExploding=false;
    }

    IEnumerator StartGameDelay()
    {
        _explosions.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
