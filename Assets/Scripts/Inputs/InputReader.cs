using UnityEngine;
using System;
using UnityEngine.InputSystem;
using RPG.SceneManagement;

public class InputReader : MonoBehaviour, Controls.IPlayerActionsActions
{
    private Controls controls;

    public Vector2 pointerPosition {get; private set;}
    
    public event Action MouseEnterEvent;
    public event Action PlayerActionEvent;
    public event Action ToggleInventory;
    public bool MouseDown{get; private set;} = false;

    // Note that this is bad and you should feel bad. Change it asap
    private SavingWrapper savingWrapper;

    private void OnEnable() 
    {
        controls = new Controls();
        controls.PlayerActions.SetCallbacks(this);
        controls.PlayerActions.Enable();    
    }
    private void OnDisable() 
    {
        controls.PlayerActions.Disable();    
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            PlayerActionEvent?.Invoke();
        }
        if(context.performed) 
        {
            if(MouseDown){return;}
            MouseDown = true;
        }
        else if(context.canceled)
        {
            if(!MouseDown){return;}
            MouseDown = false;
        }
        // Debug.Log("Mouse Down: "+MouseDown);
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        pointerPosition = context.ReadValue<Vector2>();
    }

    // Again these are bad and you should feel bad. Change asap
    public void OnSave(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        if(savingWrapper == null)
        {
            savingWrapper = GameObject.FindGameObjectWithTag("Saving").GetComponent<SavingWrapper>();
        }
        savingWrapper.Save();
    }

    public void OnLoad(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        if(savingWrapper == null)
        {
            savingWrapper = GameObject.FindGameObjectWithTag("Saving").GetComponent<SavingWrapper>();
        }
        savingWrapper.Load();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        ToggleInventory?.Invoke();
    }
}

