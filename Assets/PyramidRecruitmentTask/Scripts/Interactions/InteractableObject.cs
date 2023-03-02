using System;
using System.Collections;
using System.Collections.Generic;
using PyramidRecruitmentTask;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Material _mouseOverMaterial;

    private Renderer     _renderer;
    private Material     _regularMaterial;
    
    protected InputManager _inputManager;
    protected bool         _pointerEventsAllowed;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_pointerEventsAllowed)
        {
            return;
        }
        
        HandlePointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_pointerEventsAllowed)
        {
            return;
        }
        
        HandlePointerExit();
    }

    private void Awake()
    {
        _renderer             = GetComponent<Renderer>();
        _regularMaterial      = _renderer.material;
        _inputManager         = FindObjectOfType<InputManager>();
        _pointerEventsAllowed = true;
    }

    protected abstract void HandleInteraction();

    protected virtual void HandlePointerEnter()
    {
        _inputManager.P_InteractionButton.E_ButtonPress += HandleInteraction;
        if (_mouseOverMaterial != null)
        {
            _renderer.material = _mouseOverMaterial;
        }
    }

    protected virtual void HandlePointerExit()
    {
        _inputManager.P_InteractionButton.E_ButtonPress -= HandleInteraction;
        if (_mouseOverMaterial != null)
        {
            _renderer.material = _regularMaterial;
        }
    }
}
