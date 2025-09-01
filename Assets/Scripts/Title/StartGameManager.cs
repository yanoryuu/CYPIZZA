using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [SerializeField] private StartView startView;
    private StartModel startModel;
    private StartPresenter startPresenter;
    // Start is called before the first frame update
     
    [SerializeField] private StateManager stateManager;
    void Start()
    {
        startModel = new StartModel();
        startPresenter = new StartPresenter(startModel, startView,stateManager);
        startPresenter.Bind();
    }
}
