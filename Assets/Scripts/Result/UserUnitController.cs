using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUnitController : MonoBehaviour
{
    static private readonly Color[] CrownColors = new Color[]
    {
        new Color(1f, 0.95f, 0f), // Gold
        new Color(0.75f, 0.75f, 0.75f), // Silver
        new Color(0.8f, 0.5f, 0.2f) // Bronze
    };
    [SerializeField] Image CrownSprite;
    [SerializeField] TMPro.TextMeshProUGUI UserNameText;
    [SerializeField] TMPro.TextMeshProUGUI UserScoreText;

    public void setDate(UserData userData)
    {
        this.UserNameText.text = userData.Name;
        this.UserScoreText.text = userData.Score.ToString();
        if (userData.Rank < 3)
        {
            CrownSprite.color = CrownColors[userData.Rank];
        }
        else
        {
            Destroy(CrownSprite.gameObject);
        }
    }
}
