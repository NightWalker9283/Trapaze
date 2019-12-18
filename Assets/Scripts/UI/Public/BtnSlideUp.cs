using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BtnSlideUp : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    Canvas cvsPublic;
    RectTransform rectCvsPublic;
    Vector2 sizeCvsPuvlic;
    Vector3 startPosCvs, endPosCvs;
    // Start is called before the first frame update
    void Start()
    {
       
        
        cvsPublic = PlayingManager.playingManager.cvsPublic;
        rectCvsPublic = cvsPublic.GetComponent<RectTransform>();
        sizeCvsPuvlic = rectCvsPublic.sizeDelta;
        startPosCvs = rectCvsPublic.position;
        endPosCvs = new Vector3(startPosCvs.x, startPosCvs.y + sizeCvsPuvlic.y - 30);
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData e)
    {
        StartSlide();
    }
    // ドラック中に呼ばれる.
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    // ドラックが終了したとき呼ばれる.
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    void StartSlide()
    {
        StartCoroutine(MonitorSlide());
    }

    IEnumerator MonitorSlide()
    {
        var TapPos = DragMonitor.TapPosition;
        while (DragMonitor.Drag)
        {
            var dragLen= (DragMonitor.DragPosition.y - TapPos.y)*sizeCvsPuvlic.y;
            var newPosCvsY = startPosCvs.y + dragLen;
            newPosCvsY = Mathf.Clamp(newPosCvsY, startPosCvs.y, endPosCvs.y);
            rectCvsPublic.position =
                new Vector3(startPosCvs.x, newPosCvsY);
            yield return null;
        }
    }
}
