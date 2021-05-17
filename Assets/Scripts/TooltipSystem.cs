using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    public Tooltip tooltip;

    public void Awake()
    {
        instance = this;
    }
    public static void Show(string msg)
    {
        instance.tooltip.GetComponent<Animation>().Play();
        instance.tooltip.SetText(msg);
        instance.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        instance.tooltip.gameObject.SetActive(false);
    }
}
