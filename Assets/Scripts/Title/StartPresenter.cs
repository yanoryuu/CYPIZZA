using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPresenter : IPresenter
{
    private StartModel startModel;
    private StartView startView;
    private CompositeDisposable disposables;
    private StateManager stateManager;


    public StartPresenter(StartModel startModel, StartView startView,StateManager stateManager)
    {
        this.startModel = startModel;
        this.startView = startView;
        this.stateManager = stateManager;
        disposables = new CompositeDisposable();
    }

    public void Bind()
    {
        startView.OnStartButton.Subscribe(_ =>
            {
                Debug.Log("click start");
                stateManager.ChangeStateWithTransition(State.InGame);
                SoundManager.I.PlaySE(SoundManager.I.buttonClickClip);
            })
            .AddTo(disposables);
        
        //設定
        
        // startView.OnOptionButton.Subscribe(_ =>
        //     {
        //         Debug.Log("click option");
        //     })
        //     .AddTo(disposables);
        startView.OnInputField.Subscribe(name =>
        {
            Debug.Log($"input {name}");
            startModel.SetUserName(name);
        });
    }
    
    public void Enter()
    {
        SoundManager.I.PlayBGM(SoundManager.I.titleBgmClip);
    }
}
