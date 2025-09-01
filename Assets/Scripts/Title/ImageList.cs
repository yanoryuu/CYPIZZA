using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ImageList", menuName = "ScriptableObjects/ImageList", order = 1)]
public class ImageList : ScriptableObject
{
    [SerializeField] private Sprite[] images;

    public Sprite[] getImageList()
    {
        return images;
    }
}
