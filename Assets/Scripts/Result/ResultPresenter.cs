using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class ResultPresenter : IPresenter
{
    private ResultModel resultModel;
    private ResultView resultView;
    private RankingLoader rankingLoader;
    private InGameModel inGameModel;
    private StartModel startModel;
    private StateManager stateManager;
    
    private CompositeDisposable  disposables;
    
    public ResultPresenter(ResultModel resultModel, ResultView resultView,RankingLoader rankingLoader, InGameModel inGameModel,StartModel startModel,StateManager stateManager)
    {
        this.resultModel = resultModel;
        this.resultView = resultView;
        this.rankingLoader = rankingLoader;
        this.inGameModel = inGameModel;
        this.startModel = startModel;
        this.stateManager = stateManager;
        
        disposables = new CompositeDisposable();
    }

    public async void Bind()
    {
        resultView.onRetryButtonClicked.Subscribe(_ =>
        {
            stateManager.ChangeStateWithTransition(State.InGame);
            Debug.Log("click retry");
            SoundManager.I.PlaySE(SoundManager.I.swipeClip);
        }).AddTo(disposables);
        
        resultView.onExitButtonClicked.Subscribe(_ =>
        {
            stateManager.ChangeStateWithTransition(State.Title);
            SoundManager.I.PlaySE(SoundManager.I.buttonClickClip);
        }).AddTo(disposables);
    }
    
    public async void Enter()
    {
        Debug.Log(inGameModel.pizzaCount.Value);
        var usersData = await rankingLoader.ResultScore(inGameModel.pizzaCount.Value, startModel.userName);
        resultView.SetScore(inGameModel.pizzaCount.Value);
        resultView.SetUsersData(usersData);
        resultView.StartAnim();
        SoundManager.I.PlaySE(SoundManager.I.resultClip);
    }
}
