using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultGameManager : MonoBehaviour
{
    
    private StartPresenter startPresenter;
    private StartModel startModel;
    
    private InGameModel ingameModel;
    private InGamePresenter ingamePresenter;
    
    [SerializeField] private ResultView resultView;
    private ResultModel resultModel;
    private ResultPresenter resultPresenter;
    [SerializeField] private RankingLoader rankingLoader;

    private void Awake()
    {
        startModel = new StartModel();
        ingameModel = new InGameModel();
        resultModel = new ResultModel();
        
        // resultPresenter = new ResultPresenter(resultModel, resultView, rankingLoader, ingameModel, startModel);
        
        resultPresenter.Enter();
    }
}
