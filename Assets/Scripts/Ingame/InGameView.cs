using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using R3;
using Proto.Core;
using Proto.Input;
using Proto.Input.Gestures;

public class InGameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject pizzaImage;
    [SerializeField] private GameObject guideLoop;
    [SerializeField] private RectTransform blinkAnchorTransform;
    [SerializeField] private Image blinkAnchorImage;
    [SerializeField] private GameObject FullScreenCoverImage;
    private Image guideLoopImage;
    
    [SerializeField] private RectTransform pizzaTransform;
    private Vector3 pizzaStartPos;
    private Quaternion pizzaStartRotate;
    private Vector3 pizzaStartScale;

    private Vector2 touchPos;
    private bool isTouched;
    private bool isPizzaSwiping = false;

    // 自前スワイプ検出用
    private bool isTrackingSwipe = false;
    private Vector2 swipeStartPos;
    private float swipeStartTime;

    public  List<AnchorParent> anchorParent;
    private List<Anchor> anchors;
    private int anchorLength;

    [SerializeField] private RectTransform guideMaxTransform;
    [SerializeField] private RectTransform guideMinTransform;

    private static readonly int PizzaRotateTweenId = "PizzaRotate".GetHashCode();
    private static readonly int PizzaMoveTweenId = "PizzaMove".GetHashCode();

    public Subject<Unit> OnSwipeUp = new Subject<Unit>();

    private void Awake()
    {
        pizzaStartPos = pizzaTransform.position;
        pizzaStartRotate = pizzaTransform.rotation;
        pizzaStartScale = pizzaTransform.localScale;
    }

    private void Start()
    {
        guideLoopImage = guideLoop.GetComponent<Image>();
        GameSceneStart(anchors);
    }

    public void GameSceneStart(List<Anchor> anchorsTemp)
    {
        //Viewの初期化
        anchors = anchorsTemp;
        // anchorLength = anchors.Count;
        guideLoop.SetActive(true);
        guideLoopImage.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InSine);
        PlayStart();
    }
    
    public void PlayStart()
    {
        MoveAnchor(0);
        guideLoop.SetActive(true);
    }
    
    public void SetTime(float time)
    {
        timeText.text = time.ToString("f2");
    }

    public void ShowCountDown()
    {
        countText.gameObject.SetActive(true);
    }

    public void HideCountDown()
    {
        countText.gameObject.SetActive(false);
    }
    public void SetCount(float time)
    {
        countText.text = time.ToString("f0");
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetDifficulty(int difficulty)
    {
        difficultyText.text = difficulty.ToString();
    }

    public void MakeBigPizza(float pizzaSize)
    {
        // 直接スケール設定（毎フレーム呼ばれるためTween不要）
        pizzaTransform.localScale = Vector3.one * pizzaSize;
    }

    public void MakeRotatePizza(float bigspeed)
    {
        // 回転Tweenだけ停止して再作成（移動Tweenには影響しない）
        DOTween.Kill(PizzaRotateTweenId);
        pizzaTransform.DOLocalRotate(new Vector3(0, 0, bigspeed), 0.02f, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental)
        .SetId(PizzaRotateTweenId);
    }

    public void OnNearLimitTime()
    {
        
    }

    private void Update()
    {
        DetectSwipeUp();
    }

    private void DetectSwipeUp()
    {
        if (isPizzaSwiping) return;

        // --- タッチ入力 ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isTrackingSwipe = true;
                    swipeStartPos = touch.position;
                    swipeStartTime = Time.time;
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Ended:
                    if (isTrackingSwipe)
                    {
                        TryFireSwipe(touch.position);
                        if (touch.phase == TouchPhase.Ended)
                            isTrackingSwipe = false;
                    }
                    break;

                case TouchPhase.Canceled:
                    isTrackingSwipe = false;
                    break;
            }
            return;
        }

        // --- マウス入力（エディタ/PC用） ---
        if (Input.GetMouseButtonDown(0))
        {
            isTrackingSwipe = true;
            swipeStartPos = Input.mousePosition;
            swipeStartTime = Time.time;
        }
        else if (Input.GetMouseButton(0) && isTrackingSwipe)
        {
            TryFireSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTrackingSwipe = false;
        }
    }

    private void TryFireSwipe(Vector2 currentPos)
    {
        float deltaY = currentPos.y - swipeStartPos.y;
        float elapsed = Time.time - swipeStartTime;

        // 上方向に一定距離 & 速度を超えたら即発火（指を離す前でもOK）
        if (deltaY >= ConstVariables.swipeMinDistancePx
            && elapsed > 0f
            && (deltaY / elapsed) >= ConstVariables.swipeMinSpeedPxPerSec)
        {
            isPizzaSwiping = true;
            isTrackingSwipe = false;
            guideLoop.SetActive(false);
            OnSwipeUp.OnNext(Unit.Default);
        }
    }

    // 旧メソッド（UnityEvent参照が残っている場合の互換用）
    public void SwipePizza(ProtoGestureData gestureData)
    {
        // 新しいUpdate検出に移行したため、何もしない
    }
    
    public void SwipePizzaAnim()
    {
        // 回転Tweenだけ停止（移動Tweenはそのまま）
        DOTween.Kill(PizzaRotateTweenId);
        DOTween.Kill(PizzaMoveTweenId);
        pizzaTransform.DOMoveY(3000f, 0.6f)
            .SetId(PizzaMoveTweenId)
            .OnComplete(() =>
            {
                // 回転・スケールをリセットしてから戻す
                pizzaTransform.rotation = pizzaStartRotate;
                pizzaTransform.localScale = pizzaStartScale;
                pizzaTransform.position = pizzaStartPos + new Vector3(1000f, 0f, 0f);
                pizzaTransform.DOMove(pizzaStartPos, 1f)
                    .SetDelay(0.1f)
                    .SetId(PizzaMoveTweenId)
                    .OnComplete(() => 
                    {
                        isPizzaSwiping = false;
                    });
            });
    }

    public void MoveAnchor(int anchorNum)
    {
        Anchor anchorPrev = anchors[(anchorNum == 0) ? anchors.Count -1 : anchorNum - 1];
        Anchor anchorNow = anchors[anchorNum];
        Anchor anchorNext = anchors[(anchorNum + 1 >= anchors.Count) ? 0 : anchorNum + 1];
        anchorPrev.anchorTransform.gameObject.SetActive(false);
        anchorNow.anchorTransform.gameObject.SetActive(true);
        anchorNext.anchorTransform.gameObject.SetActive(true);

        blinkAnchorTransform.position = anchorNext.anchorTransform.position;
        // 前回のTweenを停止してから再生成
        blinkAnchorTransform.DOKill();
        blinkAnchorImage.DOKill();
        blinkAnchorTransform.localScale = Vector3.one;
        blinkAnchorTransform.DOScale(2f, 0.5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.InSine);
        blinkAnchorImage.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.InSine);
    }

    public void PizzaScaleGuideLine(float size)
    {
        guideMaxTransform.localScale = new Vector3(size * ConstVariables.maxScaleThresholdForScale, size * ConstVariables.maxScaleThresholdForScale, 0);
        guideMinTransform.localScale = new Vector3(size * ConstVariables.minScaleThresholdForScale, size * ConstVariables.minScaleThresholdForScale, 0);
    }

    public bool TryGetPointerPos(out Vector2 pos)
    {
        // 実機・タッチ対応端末
        if (Input.touchCount > 0)
        {
            pos = Input.GetTouch(0).position;
            return true;
        }

        // エディタ/PCテスト用（左クリック押下中のみ）
        if (Input.GetMouseButton(0))
        {
            pos = Input.mousePosition;
            return true;
        }

        pos = default;
        return false;
    }

    public void FinishOneLap()
    {
        difficultyText.text = "1周しました";
    }

    public void Initialize(int parentNum)
    {
        DOTween.Kill(PizzaRotateTweenId);
        DOTween.Kill(PizzaMoveTweenId);
        pizzaTransform.localScale = pizzaStartScale;
        pizzaTransform.rotation = pizzaStartRotate;
        anchors = anchorParent[parentNum].anchors;
    }
}
