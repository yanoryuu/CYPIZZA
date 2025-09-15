using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : MonoBehaviour
{
    [SerializeField] private DifficultyCardController[] difficultyCards;
    [SerializeField] private Button exitButton;

    public Subject<Mode>[] OnDifficultyButtons;
    public Subject<Unit> OnExitButton;

    private void Awake()
    {
        OnDifficultyButtons = new Subject<Mode>[difficultyCards.Length];
        for (int i = 0; i < difficultyCards.Length; i++)
        {
            OnDifficultyButtons[i] = new Subject<Mode>();
            int index = i; // クロージャ対策
            difficultyCards[i].Button.onClick.AddListener(() => {
                OnDifficultyButtons[index].OnNext(difficultyCards[index].GetMode());
                });
        }

        OnExitButton = new Subject<Unit>();
        exitButton.onClick.AddListener(() => { OnExitButton.OnNext(Unit.Default); });
    }

    public void StartAnim()
    {
        for (int i = 0; i < difficultyCards.Length; i++)
        {
            float delay = i * 0.1f;
            difficultyCards[i].PlayInAnimation(delay).Forget();
        }
    }

    public async UniTask SelectedAnim(Mode mode)
    {
        foreach(var card in difficultyCards)
        {
            if (card.GetMode() == mode)
            {
                await card.PlayOutAnimation();
            }
        }
    }
}
