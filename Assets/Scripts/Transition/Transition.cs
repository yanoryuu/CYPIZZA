using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private float transitionTime = 20f;
    [SerializeField] private Image Panel;
    [SerializeField] private Image Peel;
    [SerializeField] private Image Pizza;
    [SerializeField] private RandomImage PizzaImage;

    // 最終的に移動する位置
    private Vector3 fixedPanelPosition;
    private Vector3 fixedPeelPosition;
    private Vector3 fixedPizzaPosition;

    // アニメーションの初期位置
    private Vector3 initPanelPosition;
    private Vector3 initPeelPosition;
    private Vector3 initPizzaPositon;

    // Start is called before the first frame update
    void Start()
    {
        fixedPanelPosition = Panel.GetComponent<RectTransform>().localPosition;
        fixedPeelPosition = Peel.GetComponent<RectTransform>().localPosition;
        fixedPizzaPosition = Pizza.GetComponent<RectTransform>().localPosition;
        PizzaImage.setRandomImage();

        var panelRect = Panel.GetComponent<RectTransform>();
        initPanelPosition = new Vector3(panelRect.localPosition.x, -panelRect.rect.height, panelRect.localPosition.z);
        initPeelPosition = fixedPeelPosition + Vector3.down * 1500;
        initPizzaPositon = fixedPizzaPosition + Vector3.down * 2000;

        getUpPanel();
    }

    public async UniTask PlayInAnimation()
    {
        var panelRect = Panel.GetComponent<RectTransform>();
        var peelRect  = Peel.GetComponent<RectTransform>();
        var pizzaRect = Pizza.GetComponent<RectTransform>();

        // 初期位置リセット
        panelRect.localPosition = initPanelPosition;
        peelRect.localPosition  = initPeelPosition;
        pizzaRect.localPosition = initPizzaPositon;

        panelRect.DOKill();
        peelRect.DOKill();
        pizzaRect.DOKill();

        // 無限回転
        pizzaRect.DOLocalRotate(new Vector3(0, 0, -360), 10f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        // 前半（入ってくる）
        var seqIn = DOTween.Sequence();
        seqIn.Append(panelRect.DOLocalMove(fixedPanelPosition, transitionTime).SetEase(Ease.OutCubic));
        seqIn.Join(  peelRect.DOLocalMove(fixedPeelPosition,  transitionTime).SetEase(Ease.OutCubic));
        seqIn.Join( pizzaRect.DOLocalMove(fixedPizzaPosition, transitionTime * 2).SetEase(Ease.OutCubic));

        seqIn.Play();

        await seqIn.AsyncWaitForCompletion(); // ? 入ってくる完了を待つ
    }

    public async UniTask PlayOutAnimation()
    {
        var panelRect = Panel.GetComponent<RectTransform>();
        var peelRect  = Peel.GetComponent<RectTransform>();
        var pizzaRect = Pizza.GetComponent<RectTransform>();

        var seqOut = DOTween.Sequence();
        seqOut.AppendInterval(0.3f);
        seqOut.Append(panelRect.DOLocalMove(initPanelPosition, transitionTime).SetEase(Ease.InCubic));
        seqOut.Join(  peelRect.DOLocalMove(initPeelPosition,  transitionTime).SetEase(Ease.InCubic));
        seqOut.Join( pizzaRect.DOLocalMove(initPizzaPositon,  transitionTime * 2).SetEase(Ease.InCubic));

        seqOut.Play();

        await seqOut.AsyncWaitForCompletion();

        pizzaRect.DOKill(); // 回転止める
    }

    // パネルを画面外に追い出す
    private void getUpPanel()
    {
        var panelRect = Panel.GetComponent<RectTransform>();
        panelRect.localPosition = initPanelPosition;
    }

}
