using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    
    #region Unitask插件异步加载写法
    public async void LoadSceneAsync()
    {
        UIModule.Instance.PopUpWindow<LoadingWindow>();
        await AsyncLoadScene();
    }

    private async UniTask AsyncLoadScene()
    {
        // 异步加载场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Battle");
        // 默认不允许激活
        asyncOperation.allowSceneActivation = false;

        // 当前加载进度
        float curProgress = 0f;
        // 最大加载进度
        float maxProgress = 100f;

        // 等待加载进度达到90%,Unity加载只会从0-0.9，所以这里等待进度达到90%，剩余的0.1需要自己使用代码进行过渡
        while (asyncOperation.progress < 0.9f)
        {
            curProgress = asyncOperation.progress * 100.0f;
            UIEventControl.DispensEvent(UIEventEnum.OnLoadingProgressUpdate, curProgress); // 响应进度条更新事件
            // 等待下一帧
            await UniTask.Yield();
        }

        // 模拟进度条从90%增长到100%
        while (curProgress < maxProgress)
        {
            curProgress++;
            UIEventControl.DispensEvent(UIEventEnum.OnLoadingProgressUpdate, curProgress); // 响应进度条更新事件
            // 等待下一帧
            await UniTask.Yield();
        }

        // 激活已经加载完成的场景
        asyncOperation.allowSceneActivation = true;

        // 清理所有UI窗口
        UIModule.Instance.DestroyAllWindow();
    }

    #endregion
    
    #region 协程写法
    
    public void LoadSceneAsyncIEnumerator()
    {
        StartCoroutine(AsyncLoadSceneIEnumerator());
    }

    private IEnumerator AsyncLoadSceneIEnumerator()
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
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
