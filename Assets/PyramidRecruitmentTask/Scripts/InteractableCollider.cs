using System;
using System.Collections;
using System.Collections.Generic;
using PyramidRecruitmentTask;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class InteractableCollider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Material _mouseOverMaterial;

    private Renderer     _renderer;
    private Material     _regularMaterial;
    private InputManager _inputManager;
    
    private void Awake()
    {
        _renderer        = GetComponent<Renderer>();
        _regularMaterial = _renderer.material;
        _inputManager    = FindObjectOfType<InputManager>();
    }

    private void ClickTest()
    {
        Debug.Log("Click!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter");
        _inputManager.P_InteractionButton.E_ButtonPress += ClickTest;
        if (_mouseOverMaterial != null)
        {
            _renderer.material = _mouseOverMaterial;
        }
    }

    public void OnPointerExit(PointerEventData  eventData)
    {
        Debug.Log("Mouse exit");
        _inputManager.P_InteractionButton.E_ButtonPress -= ClickTest;
        if (_mouseOverMaterial != null)
        {
            _renderer.material = _regularMaterial;
        }
    }
}
