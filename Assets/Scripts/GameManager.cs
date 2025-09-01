using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;
    
    [SerializeField] private StartView startView;
    private StartPresenter startPresenter;
    private StartModel startModel;
    
    [SerializeField] private InGameView ingameView;
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
        
        startPresenter = new StartPresenter(startModel, startView,stateManager);
        ingamePresenter = new InGamePresenter(ingameModel, ingameView,stateManager);
        resultPresenter = new ResultPresenter(resultModel, resultView, rankingLoader, ingameModel, startModel, stateManager);
        
        // バインド
        startPresenter.Bind();
        ingamePresenter.Bind();
        resultPresenter.Bind();
        
        stateManager.titlePresenter = startPresenter;
        stateManager.resultPresenter = resultPresenter;
        stateManager.inGamePresenter = ingamePresenter;
    }
}
