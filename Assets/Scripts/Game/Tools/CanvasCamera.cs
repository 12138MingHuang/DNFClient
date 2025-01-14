using System;
using UnityEngine;

public class CanvasCamera : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = UIModule.Instance.UICamera;
    }
}