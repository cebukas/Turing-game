using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{

    public TMP_Text text;
    public RectTransform rectTransform;

    public LayoutElement layout;

    public int characterLimit;
    public void SetText(string msg)
    {
        text.text = msg;

        layout.enabled = (text.text.Length > characterLimit) ? true : false;
    }
    private void FixedUpdate()
    {
        Vector2 position = Input.mousePosition;

        transform.position = position;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }
}
