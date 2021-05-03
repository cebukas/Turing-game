using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text text;
    private Color32 gray = new Color32(140, 132, 132, 255);
    private Color white = Color.white;
    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = gray;
    }
}