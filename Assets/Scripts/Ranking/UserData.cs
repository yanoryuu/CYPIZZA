using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    private int rank;
    private string name;
    private int score;

    public UserData(int rank, string name, int score)
    {
        this.rank = rank;
        this.name = name;
        this.score = score;
    }

    public int Rank { get { return rank; } }
    public string Name { get { return name; } }
    public int Score { get { return score; } }
}
