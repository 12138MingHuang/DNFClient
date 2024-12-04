using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初始化UI框架
        UIModule.Instance.Initialize();
        UIModule.Instance.PopUpWindow<CreateRoleWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
