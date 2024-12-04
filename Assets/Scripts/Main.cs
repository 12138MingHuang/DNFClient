using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // TODO 测试代码
    public static Main Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //初始化UI框架
        UIModule.Instance.Initialize();
        UIModule.Instance.PopUpWindow<CreateRoleWindow>();
        //不允许销毁当前节点
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadSceneAsync()
    {
        StartCoroutine(AsyncLoadScene());
    }

    private IEnumerator AsyncLoadScene()
    {
        //异步加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Battle");
        //默认不允许激活
        asyncOperation.allowSceneActivation = false;
        
        //当前加载进度
        float curProgress = 0;
        //最大加载进度
        float maxProgress = 100;

        while (curProgress < 90f)
        {
            curProgress = asyncOperation.progress * 100.0f;
            yield return null;
        }

        while (curProgress < maxProgress)
        {
            curProgress++;
            //等待一个空帧是为了让UI有渲染过程
            yield return null;
        }
        //激活已经加载完成的场景
        asyncOperation.allowSceneActivation = true;
        
        UIModule.Instance.DestroyAllWindow();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
