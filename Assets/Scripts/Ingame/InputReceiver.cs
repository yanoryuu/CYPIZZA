using R3;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public Subject<Unit> OnSwipe;

    public void OnSwipeUp()
    {
        OnSwipe.OnNext(Unit.Default);
    }
}
