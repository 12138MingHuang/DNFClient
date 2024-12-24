using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class DamageTextItem : MonoBehaviour
{
    [SerializeField]
    private Text DamageText;
    
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public async void ShowDamageText(int damageValue, RenderObject target)
    {
        BattleWindow window = UIModule.Instance.GetWindow<BattleWindow>();
        transform.SetParent(window.transform);
        transform.localScale = Vector3.one;
        transform.position = PosConvertUtility.World3DPosToCanvasWorldPos(target.transform.position, window.transform as RectTransform, UIModule.Instance.UICamera);

        DamageText.text = damageValue.ToString();
        
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 20f, 0f);
        transform.localScale = Vector3.one * 2f;
        transform.DOScale(1, 0.3f);
        await UniTask.Delay(200);
        _canvasGroup.DOFade(0, 0.3f);
        await transform.DOMoveY(transform.position.y + 1f, 0.2f).AsyncWaitForCompletion();
        _canvasGroup.DOKill();
        transform.DOKill();
        Destroy(gameObject);
    }
}
