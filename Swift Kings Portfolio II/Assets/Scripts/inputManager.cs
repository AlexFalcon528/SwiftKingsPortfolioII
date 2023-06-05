using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    // Singleton
    public static inputManager instance;

    public bool MenuOpenInput { get; private set; }

    [SerializeField] private PlayerInput playerInput;
    private InputAction menuOpenAction;

    private void Awake() {
        if (instance == null) instance = this;
        menuOpenAction = playerInput.actions["MenuOpenClose"];
    }

    private void Update() {
        MenuOpenInput = menuOpenAction.WasPerformedThisFrame();
    }

}
