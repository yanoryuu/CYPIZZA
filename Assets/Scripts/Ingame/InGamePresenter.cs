using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Proto.Core;
using R3;
using UnityEngine;

public class InGamePresenter : IPresenter
{
    private readonly InGameModel inGameModel;
    private readonly InGameView inGameView;
    private readonly StateManager stateManager;

    //全体の購読解除用
    private CompositeDisposable disposables;
    
    //ゲーム中の購読解除用
    private CompositeDisposable ingameDisposables;

    private readonly Camera uiCamera = null;
    
    public InGamePresenter(InGameModel inGameModel, InGameView inGameView ,StateManager stateManager)
    {
        this.inGameModel = inGameModel;
        this.inGameView = inGameView;
        this.stateManager = stateManager;
        disposables = new CompositeDisposable();
        inGameModel.SetAncor(inGameView.anchorParent[0].anchors);
    }

    public void Bind()
    {
        inGameModel.pizzaCount
            .Subscribe(count => inGameView.SetScore(count))
            .AddTo(disposables);

        inGameModel.currentRestTime
            .Subscribe(time => inGameView.SetTime(time))
            .AddTo(disposables);

        inGameModel.pizzaScale
            .Subscribe(scale =>
            {
                Debug.Log(scale);
                inGameView.MakeBigPizza(scale);
            })
            .AddTo(disposables);

        Observable.EveryUpdate()
            .Select(_ =>
            {
                Vector2 p;
                var ok = inGameView.TryGetPointerPos(out p); // p はスクリーン座標(px)
                return (ok, p);
            })
            .Where(t => t.ok)
            .Where(_ => inGameModel.HasAnchors) 
            .Where(t =>
            {
                float d = inGameModel.HandDistanceToCurrentAnchor_ScreenSpace(t.p, uiCamera);
                return d <= ConstVariables.anchorHitThresholdPx;
            })
            .Subscribe(_ => inGameModel.AddCurrentAnchorIndex())
            .AddTo(disposables);
        
        
        
        Debug.Log($"[Presenter] start, rotateSpeed={inGameModel.pizzaRotateSpeed.Value}");
    }

    public void Initialize()
    {
        inGameModel.Initialize();
        inGameView.Initialize(0);
    }
    
    public void StartMakePizzaCountdown(float delaySeconds)
    {
        inGameView.Initialize(0);
        
        DOVirtual.Float(delaySeconds, 0f, delaySeconds, t =>
            {
                inGameView.ShowCountDown();
                inGameView.SetCount(t);
                // 0.1秒刻みでキレよく
            })
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                inGameView.HideCountDown();
                // ここで元の処理を実行
                StartMakePizza();
            });
    }
    
    //3秒カウント後のゲーム開始、
    public void StartMakePizza()
    {
        ingameDisposables = new CompositeDisposable();
        
        Observable.EveryUpdate()
            .Subscribe(_ => inGameModel.ChangeTime(Time.deltaTime))
            .AddTo(ingameDisposables);
        
        Observable.EveryUpdate()
            .Subscribe(_=>inGameModel.DelayRotatePizzaSpeed(0.0003f))
            .AddTo(ingameDisposables);
        
        Observable.EveryUpdate()
            .Subscribe(_=>inGameModel.TickScalingByFrame())
            .AddTo(ingameDisposables);
        
        inGameModel.currentAncorIndex.Subscribe(index => inGameView.MoveAnchor(index))
            .AddTo(ingameDisposables);
        
        inGameModel.OnFinishedOneLap
            .Subscribe(_ =>
            {
                inGameModel.finishedOneLap();
                inGameView.MakeRotatePizza(inGameModel.pizzaRotateSpeed.Value);
            })
            .AddTo(ingameDisposables);

        inGameModel.currentRestTime
            .Where(time => time <= 0f)
            .Subscribe(_ => StopMakePizza())
            .AddTo(ingameDisposables);
        
        inGameView.OnSwipeUp.Subscribe(_ =>
            {
                inGameModel.shippingPizza();
                inGameView.SwipePizzaAnim();
                SoundManager.I.PlaySE(SoundManager.I.swipeClip);
            })
            .AddTo(ingameDisposables);

        inGameModel.limitScale.Subscribe(limit =>
            {   
                inGameView.PizzaScaleGuideLine(limit);
            })
            .AddTo(ingameDisposables);
    }
    
    //ゲーム終了
    public void StopMakePizza()
    {
        ingameDisposables.Dispose();
        stateManager.ChangeStateWithTransition(State.Result);
    }
    
    public void Enter()
    {
        StartMakePizzaCountdown(3.0f);
        SoundManager.I.PlayBGM(SoundManager.I.inGameBgmClip);
        Initialize();
    }
}