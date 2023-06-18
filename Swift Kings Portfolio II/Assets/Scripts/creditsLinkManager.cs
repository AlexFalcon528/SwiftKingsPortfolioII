using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class creditsLinkManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text text;
    string id;
    int index;
  public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("0");
        index = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
        if (index != -1)
        {
            id= text.textInfo.linkInfo[index].GetLinkID();
            Application.OpenURL(id);
        }
    }
}
