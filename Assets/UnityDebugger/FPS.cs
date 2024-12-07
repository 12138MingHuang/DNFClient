using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityDebugger
{
    public class FPS : MonoBehaviour
    {
        private float _deltaTime = 0.0f;

        private GUIStyle _style;
        
        private void Awake()
        {
            _style = new GUIStyle();
            _style.alignment = TextAnchor.UpperLeft;
            _style.normal.background = null;
            _style.fontSize = 20;
            _style.normal.textColor = Color.white;
        }

        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, 500, 300);
            float fps = 1.0f / _deltaTime;
            string text = $"FPS:{fps:N0}";
            GUI.Label(rect, text, _style);
            
            Rect appInfoRect = new Rect(Screen.width - 400, Screen.height - 30, 500, 300);
        }
    }
}

