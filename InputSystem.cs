using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }
    public new Camera camera;
    private Player_InputActions _PlayerActions;

    public event EventHandler OnPlayerClick;

    private void Awake()
    {
        Instance = this;
        _PlayerActions = new Player_InputActions();
        _PlayerActions.Enable();

        _PlayerActions.Activity.AddLine.started += AddLine;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(mousePos); ;
    }

    private void AddLine(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerClick?.Invoke(this, EventArgs.Empty);
    }
}
