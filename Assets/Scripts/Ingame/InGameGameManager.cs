using System;
using UnityEngine;

public class InGameGameManager : MonoBehaviour
{
    public InGamePresenter inGamePresenter { get;private set; }
    public InGameModel inGameModel { get;private set; }

    [SerializeField] private InGameView inGameView;
    
    [SerializeField] private StateManager stateManager;

    private void Awake()
    {
        inGameModel = new InGameModel();
        inGamePresenter = new InGamePresenter(inGameModel, inGameView,stateManager);
        inGamePresenter.Bind();
        inGamePresenter.Initialize();
    }
}
