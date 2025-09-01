using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    [SerializeField] private ImageList imageList;
    [SerializeField] private Image image;

    void Start()
    {
        // image = GetComponent<Image>();
        setRandomImage();
    }

    public int setRandomImage()
    {
        Debug.Log(imageList.getImageList().Length);
        int index = Random.Range(0, imageList.getImageList().Length);
        image.sprite = imageList.getImageList()[index];
        return index;
    }
}
