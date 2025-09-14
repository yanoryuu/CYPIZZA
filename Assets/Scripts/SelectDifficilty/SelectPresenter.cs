using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPresenter : IPresenter
{
    private SelectModel selectModel;
    private SelectView selectView;
    private CompositeDisposable disposables;
    private StateManager stateManager;

    public SelectPresenter(SelectModel selectModel, SelectView selectView, StateManager stateManager)
    {
        this.selectModel = selectModel;
        this.selectView = selectView;
        this.stateManager = stateManager;
        disposables = new CompositeDisposable();
    }

    public void Bind()
    {
        foreach (var OnButton in selectView.OnDifficultyButtons)
        {

            OnButton.Subscribe(async mode =>
            {
                Debug.Log($"click {mode}");
                SoundManager.I.PlaySE(SoundManager.I.swipeClip);
                await selectView.SelectedAnim(mode);
                //stateManager.ApplyState(State.InGame);
            })
                .AddTo(disposables);
        }

        selectView.OnExitButton.Subscribe(_ =>
        {
            Debug.Log("click exit");
            SoundManager.I.PlaySE(SoundManager.I.buttonClickClip);
            //stateManager.ApplyState(State.Title);
        });
    }

    public void Enter()
    {
        selectView.StartAnim();
    }

    public void Initialize()
    {
        
    }
    
}
