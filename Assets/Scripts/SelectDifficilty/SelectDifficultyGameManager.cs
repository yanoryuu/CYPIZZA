using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDifficultyGameManager : MonoBehaviour
{
    [SerializeField] private SelectView selectView;
    private SelectModel selectModel;
    private SelectPresenter selectPresenter;

    [SerializeField] private StateManager stateManager;
    void Start()
    {
        selectModel = new SelectModel();
        selectPresenter = new SelectPresenter(selectModel, selectView, stateManager);
        selectPresenter.Bind();
        selectView.StartAnim();
    }
}
