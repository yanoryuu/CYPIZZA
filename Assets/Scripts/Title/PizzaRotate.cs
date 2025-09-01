using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaRotate : MonoBehaviour
{
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOLocalRotate(new Vector3(0, 0, -360), 30f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}
