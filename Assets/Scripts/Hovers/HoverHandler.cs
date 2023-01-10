using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private IHoverable _hoverObj;
    
    private void Start() {
        gameObject.TryGetComponent<IHoverable>(out _hoverObj);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_hoverObj != null)
        {
            _hoverObj.OnHoverEnter();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_hoverObj != null)
        {
            _hoverObj.OnHoverExit();
        }
    }
}
