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

    public  List<AnchorParent> anchorParent;
    private List<Anchor> anchors;
    private int anchorLength;

    [SerializeField] private RectTransform guideMaxTransform;
    [SerializeField] private RectTransform guideMinTransform;

    public Subject<Unit> OnSwipeUp = new Subject<Unit>();

    private void Start()
    {
        guideLoopImage = guideLoop.GetComponent<Image>();
        GameSceneStart(anchors);
        pizzaStartPos = pizzaTransform.position;
        pizzaStartRotate = pizzaTransform.rotation;
        pizzaStartScale = pizzaTransform.localScale;
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
        pizzaTransform.DOScale(pizzaSize, 0.2f);
        //.SetAutoKill(true);
    }

    public void MakeRotatePizza(float bigspeed)
    {
        // pizzaTransform.rotation = Quaternion.Euler(0, 0, pizzaTransform.rotation.z + bigspeed);
        pizzaTransform.DOLocalRotate(new Vector3(0, 0, bigspeed), 0.02f, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }

    public void OnNearLimitTime()
    {
        
    }
    public void SwipePizza(ProtoGestureData gestureData)
    {
        if(gestureData.type == ProtoGestureType.Swipe)
        {
            if(gestureData.endPosition.y >= ConstVariables.swipeAreaPercent*Screen.height && isPizzaSwiping == false)
            {
                isPizzaSwiping = true;
                guideLoop.SetActive(false);
                //Swipe判定
                OnSwipeUp.OnNext(Unit.Default);
                Debug.Log("GestureType" + gestureData.type);
            }
        }
    }
    
    public void SwipePizzaAnim()
    {
        //Pizzaアニメーション
        pizzaTransform.DOMoveY(3000f, 0.6f).OnComplete(() =>
            {
                pizzaTransform.position = pizzaStartPos + new Vector3(1000f, 0f, 0f);
                pizzaTransform.DOKill();
                pizzaTransform.DOMove(pizzaStartPos, 1f).SetDelay(0.1f).OnComplete(() => 
                {
                    isPizzaSwiping = false;
                    Debug.Log("isPizzaSwiping = false");
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
        pizzaTransform.DOKill();
        pizzaTransform.localScale = pizzaStartScale;
        pizzaTransform.rotation = pizzaStartRotate;
        anchors = anchorParent[parentNum].anchors;
    }
}
