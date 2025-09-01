using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotate : MonoBehaviour
{
    [SerializeField] float second = 30f;
    [SerializeField] float direction = -1;
    // Start is called before the first frame update
    void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.DOLocalRotate(new Vector3(0, 0, 360 * direction), second, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

}
