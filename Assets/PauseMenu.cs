using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerInputActions _playerControls;
    public GameObject pauseMenuUI;
    public static bool isGamePaused = false;
    private InputAction _menu;

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _menu = _playerControls.UI.ManageMenu;        
        _menu.Enable();
    }

    private void OnDisable()
    {
        _menu = _playerControls.UI.ManageMenu;
        _menu.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_menu.triggered) MenuControl();
        
    }

    public void MenuControl()
    {
        pauseMenuUI.SetActive(!isGamePaused);
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
    }

}
