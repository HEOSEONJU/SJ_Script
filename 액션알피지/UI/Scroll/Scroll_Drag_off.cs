using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scroll_Drag_off : ScrollRect
{

    #region//드래그 비활성화
    public override void OnBeginDrag(PointerEventData eventData)
    {

    }
    public override void OnDrag(PointerEventData eventData)
    {

    }
    public override void OnEndDrag(PointerEventData eventData)
    {

    }
    #endregion

    #region//마우스휠 비활성화
    public override void OnScroll(PointerEventData data)
    {

    }
    #endregion


}
