using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField] private UserUnitManager UserUnitManager;
    
    [SerializeField] private Button retryButton;
    [SerializeField] private Button[] exitButton;
    public Subject<Unit> onRetryButtonClicked { get; private set; } = new Subject<Unit>();
    public Subject<Unit> onExitButtonClicked{get; private set;} = new Subject<Unit>();

    [SerializeField] private RandomImage BackPizza;
    [SerializeField] private TextMeshProUGUI scoreText;

    private Vector3 fixedScoreScale;
    private Vector3 fixedScoreRotate;

    private Vector3 initScoreScale;
    private Vector3 initScoreRotate;

    private void Awake()
    {
        retryButton.onClick.AddListener(() => { onRetryButtonClicked.OnNext(Unit.Default); });
        foreach(var exitButton in exitButton)
        {
            exitButton.onClick.AddListener(() => { onExitButtonClicked.OnNext(Unit.Default); });
        }
    }

    private void Start()
    {
        fixedScoreScale = scoreText.transform.localScale;
        fixedScoreRotate = scoreText.transform.localEulerAngles;

        initScoreScale = Vector3.zero;
        initScoreRotate = fixedScoreRotate + new Vector3(0, 0, -90);
    }

    public void SetUsersData(List<UserData> usersData)
    {
        UserUnitManager.SetUserUnit(usersData);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void StartAnim()
    {
        RectTransform scoreRect = scoreText.GetComponent<RectTransform>();
        scoreRect.DOKill();
        BackPizza.setRandomImage();

        scoreRect.localScale = initScoreScale;
        var sequence = DOTween.Sequence();
        sequence.Append(scoreRect.DOScale(fixedScoreScale, .5f).SetEase(Ease.OutBack));
        sequence.Join(scoreRect.DORotate(fixedScoreRotate, .5f).SetEase(Ease.OutBounce));

        sequence.Play();
    }
}
