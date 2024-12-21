using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZMAssetFrameWork;
using ZMGC.Battle;
using ZMGC.Hall;

public class Main : MonoBehaviour
{
    private void Start()
    {
        //初始化资源加载框架
        ZMAssetsFrame.Instance.InitFrameWork();
        //初始化UI框架
        UIModule.Instance.Initialize();
        //初始化大厅世界
        WorldManager.CreateWorld<HallWorld>();
        //不允许销毁当前节点
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        
    }

    private void Update()
    {
        WorldManager.OnUpdate();
    }
}
