using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause
    }
   
    private PlayerInputActions PlayerInputActions;
    private void Awake()
    {
        Instance = this;

       PlayerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            PlayerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        PlayerInputActions.Player.Enable();

        PlayerInputActions.Player.Interact.performed += Interact_performed;
        PlayerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        PlayerInputActions.Player.Pause.performed += Pause_performed;

    }
    private void OnDestroy()
    {
        PlayerInputActions.Player.Interact.performed -= Interact_performed;
        PlayerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        PlayerInputActions.Player.Pause.performed -= Pause_performed;

        PlayerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {


        OnInteractAction?.Invoke(this, EventArgs.Empty);
        
        
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = PlayerInputActions.Player.Move.ReadValue<Vector2>();

     
        inputVector = inputVector.normalized;

        return inputVector;
    }
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                return PlayerInputActions.Player.Interact.bindings[0].ToDisplayString();   
            case Binding.InteractAlternate:
                return PlayerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();   
            case Binding.Pause:
                return PlayerInputActions.Player.Pause.bindings[0].ToDisplayString();   
            case Binding.Move_Up:
                return PlayerInputActions.Player.Move.bindings[1].ToDisplayString();   
            case Binding.Move_Down:
                return PlayerInputActions.Player.Move.bindings[2].ToDisplayString();   
            case Binding.Move_Left:
                return PlayerInputActions.Player.Move.bindings[3].ToDisplayString(); 
            case Binding.Move_Right:
                return PlayerInputActions.Player.Move.bindings[4].ToDisplayString();
               
           
            
               
        }
    }
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        PlayerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = PlayerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = PlayerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = PlayerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = PlayerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = PlayerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = PlayerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = PlayerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
          
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
           
            callback.Dispose();
            PlayerInputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, PlayerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        })
            .Start();

    }
    
}
