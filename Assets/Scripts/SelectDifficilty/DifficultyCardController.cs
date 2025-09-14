using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyCardController : MonoBehaviour
{
    [SerializeField] private Mode mode;
    [SerializeField] private RectTransform contentTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    public Button Button;

    // アニメーション前の位置
    private Vector3 beforePosition;

    // アニメーション後の位置
    private Vector3 afterPosition;

    public Mode GetMode()
    {
        return mode;
    }

    private void Awake()
    {
        afterPosition = contentTransform.localPosition;

        beforePosition = new Vector3(afterPosition.x + 1500f, afterPosition.y, afterPosition.z);
    }

    // 入るアニメーション
    public async UniTask PlayInAnimation(float delaySecond = 0)
    {
        // 初期位置をリセット
        contentTransform.localPosition = beforePosition;
        canvasGroup.alpha = 0f;

        contentTransform.DOKill();
        canvasGroup.DOKill();

        var seqIn = DOTween.Sequence();
        seqIn.PrependInterval(delaySecond);
        seqIn.Append(contentTransform.DOLocalMove(afterPosition, 0.5f).SetEase(Ease.OutCubic));
        seqIn.Join(canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear));
        await seqIn.Play().AsyncWaitForCompletion();

        // 無限に揺らすアニメーション
        contentTransform.DOLocalMoveX(afterPosition.x + 10f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
    }

    // 出るアニメーション
    public async UniTask PlayOutAnimation()
    {
        contentTransform.DOKill();

        var seqOut = DOTween.Sequence();
        seqOut.Append(contentTransform.DOLocalMove(beforePosition, 0.5f).SetEase(Ease.OutCubic));
        await seqOut.Play().AsyncWaitForCompletion();
    }
}
