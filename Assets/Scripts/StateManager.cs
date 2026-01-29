using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public ReactiveProperty<State> currentState{get; private set;} = new ReactiveProperty<State>(State.Title);

    [SerializeField] private GameObject titlePanel;
    public IPresenter titlePresenter;
    // [SerializeField] private GameObject selectStagePanel;
    // public IPresenter selectStagePresenter;
    [SerializeField] private GameObject inGamePanel;
    public IPresenter inGamePresenter;
    [SerializeField] private GameObject resultPanel;
    public IPresenter resultPresenter;
    [SerializeField] private GameObject transitionPanel;
    
    [SerializeField] private Transition transition;
    private bool isTransitioning;
    
    public async UniTask ChangeStateWithTransition(State next)
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (transitionPanel != null) transitionPanel.SetActive(true);

        // ① 入ってくるアニメーション
        await transition.PlayInAnimation();

        // ② ここで State を変更
        ApplyState(next);

        // ③ 出ていくアニメーション
        await transition.PlayOutAnimation();

        if (transitionPanel != null) transitionPanel.SetActive(false);

        isTransitioning = false;
    }
    
    public void ApplyState(State state)
    {
        currentState.Value = state;
        
        switch (currentState.Value)
        {
            case State.Title:
                titlePanel.SetActive(true);
                // selectStagePanel.SetActive(false);
                inGamePanel.SetActive(false);
                resultPanel.SetActive(false);
                titlePresenter.Enter();
                break;
            /*case State.SelectStage:
                titlePanel.SetActive(false);
                selectStagePanel.SetActive(true);
                inGamePanel.SetActive(false);
                resultPanel.SetActive(false);
                transitionPanel.SetActive(false);
                selectStagePresenter.Enter();
                break;*/
            case State.InGame:
                titlePanel.SetActive(false);
                // selectStagePanel.SetActive(false);
                inGamePanel.SetActive(true);
                resultPanel.SetActive(false);
                inGamePresenter.Enter();
                break;
            case State.Result:
                titlePanel.SetActive(false);
                // selectStagePanel.SetActive(false);
                inGamePanel.SetActive(false);
                resultPanel.SetActive(true);
                resultPresenter.Enter();
                break;
        }
    }
}

public enum State
{
    Title,
    /*SelectStage,*/
    InGame,
    Result
}
