using DG.Tweening;
using R3;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartView : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button OptionButton;
    [SerializeField] private TMP_InputField InputField;
    [SerializeField] private GameObject PizzaObject;
    [SerializeField] private Image TitleImg;

    // �ŏI�I�ɌŒ肳���ʒu
    private Vector3 fixedPlayButtonTransform;
    private Vector3 fixedOptionButtonTransform;
    private Vector3 fixedInputFieldTransform;
    private Vector3 fixedPizzaTransform;
    private Vector3 fixedPizzaScale;
    private Vector3 fixedTitleImgScale;
    private Vector3 fixedTitleImgRotate;

    // �A�j���[�V�����̏����ʒu
    private Vector3 initPlayButtonTransform;
    private Vector3 initOptionButtonTransform;
    private Vector3 initInputFieldTransform;
    private Vector3 initPizzaTransform;
    private Vector3 initPizzaScale;
    private Vector3 initTitleImgScale;
    private Vector3 initTitleImgRotate;

    public Subject<Unit> OnStartButton = new Subject<Unit>();
    public Subject<Unit> OnOptionButton = new Subject<Unit>();
    public Subject<string> OnInputField = new Subject<string>();

    // Start is called before the first frame update
    private void Awake()
    {
        PlayButton.onClick.AddListener(() => OnStartButton.OnNext(Unit.Default));
        OptionButton.onClick.AddListener(() => OnOptionButton.OnNext(Unit.Default));
        InputField.onEndEdit.AddListener((string text) => OnInputField.OnNext(text));

        fixedPlayButtonTransform = PlayButton.GetComponent<RectTransform>().localPosition;
        fixedOptionButtonTransform = OptionButton.GetComponent<RectTransform>().localPosition;
        fixedInputFieldTransform = InputField.GetComponent<RectTransform>().localPosition;
        fixedPizzaTransform = PizzaObject.GetComponent<Transform>().localPosition;
        fixedPizzaScale = PizzaObject.GetComponent<Transform>().localScale;
        fixedTitleImgScale = TitleImg.GetComponent<RectTransform>().localScale;
        fixedTitleImgRotate = TitleImg.GetComponent<RectTransform>().localEulerAngles;

        initPlayButtonTransform = fixedPlayButtonTransform + Vector3.right * 1500;
        initOptionButtonTransform = fixedOptionButtonTransform + Vector3.right * 1500;
        initInputFieldTransform = fixedInputFieldTransform + Vector3.right * 1300;
        initPizzaTransform = fixedPizzaTransform + Vector3.up * 1800;
        initPizzaScale = Vector3.zero;
        initTitleImgScale = Vector3.zero;
        initTitleImgRotate = fixedTitleImgRotate + new Vector3(0, 0, -180);

        StartAnim();
    }

    // UI�o��A�j���[�V����
    public void StartAnim()
    {
        var sequence = DOTween.Sequence();
        
        var startButtonRect = PlayButton.GetComponent<RectTransform>();
        var optionButtonRect = OptionButton.GetComponent<RectTransform>();
        var inputFieldRect = InputField.GetComponent<RectTransform>();
        var titleImgRect = TitleImg.GetComponent<RectTransform>();

        startButtonRect.DOKill();
        optionButtonRect.DOKill();
        inputFieldRect.DOKill();
        titleImgRect.DOKill();
        PizzaObject.transform.DOKill();

        startButtonRect.localPosition = initPlayButtonTransform;
        optionButtonRect.localPosition = initOptionButtonTransform;
        inputFieldRect.localPosition = initInputFieldTransform;
        //PizzaObject.transform.localPosition = initPizzaTransform;
        PizzaObject.transform.localScale = initPizzaScale;
        titleImgRect.transform.localScale = initTitleImgScale;
        titleImgRect.transform.localEulerAngles = initTitleImgRotate;

        float delaySecond = 0.5f;
        //sequence.Insert(delaySecond, PizzaObject.transform.DOLocalMove(fixedPizzaTransform, .5f));
        sequence.Insert(delaySecond, PizzaObject.transform.DOScale(fixedPizzaScale, .5f).SetEase(Ease.OutCubic));
        delaySecond += 0.1f;
        sequence.Insert(delaySecond, titleImgRect.transform.DOScale(fixedTitleImgScale, .5f).SetEase(Ease.OutCubic));
        sequence.Insert(delaySecond, titleImgRect.transform.DORotate(fixedTitleImgRotate, .7f).SetEase(Ease.OutElastic));
        // UI�̃A�j���[�V����
        delaySecond += 0.5f;
        sequence.Insert(delaySecond, inputFieldRect.DOLocalMove(fixedInputFieldTransform, .5f).SetEase(Ease.OutCubic));
        delaySecond += 0.1f;
        sequence.Insert(delaySecond, startButtonRect.DOLocalMove(fixedPlayButtonTransform, .5f).SetEase(Ease.OutCubic));
        delaySecond += 0.1f;
        sequence.Insert(delaySecond, optionButtonRect.DOLocalMove(fixedOptionButtonTransform, .5f).SetEase(Ease.OutCubic));
        sequence.Append(titleImgRect.DOScale(fixedPizzaScale * 1.2f, 0.5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo));

        sequence.Play();
    }

    private async void PlayButonAnim(GameObject obj)
    {
        Debug.Log("�A�j���[�V�����Đ�");
    }

}
