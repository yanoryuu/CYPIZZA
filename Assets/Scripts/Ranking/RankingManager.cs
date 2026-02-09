using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    private const int MAX_RANKING_COUNT = 5;
    private const string RANKING_KEY_PREFIX = "Ranking_";

    private List<UserData> rankingList = new List<UserData>();

    protected override void Awake()
    {
        base.Awake();
        LoadRanking();
    }

    /// <summary>
    /// ランキングを読み込む
    /// </summary>
    public void LoadRanking()
    {
        rankingList.Clear();

        for (int i = 0; i < MAX_RANKING_COUNT; i++)
        {
            string name = PlayerPrefs.GetString($"{RANKING_KEY_PREFIX}Name_{i}", "");
            int score = PlayerPrefs.GetInt($"{RANKING_KEY_PREFIX}Score_{i}", 0);

            if (!string.IsNullOrEmpty(name))
            {
                rankingList.Add(new UserData(i + 1, name, score));
            }
        }
    }

    /// <summary>
    /// ランキングを保存する
    /// </summary>
    public void SaveRanking()
    {
        for (int i = 0; i < MAX_RANKING_COUNT; i++)
        {
            if (i < rankingList.Count)
            {
                PlayerPrefs.SetString($"{RANKING_KEY_PREFIX}Name_{i}", rankingList[i].Name);
                PlayerPrefs.SetInt($"{RANKING_KEY_PREFIX}Score_{i}", rankingList[i].Score);
            }
            else
            {
                PlayerPrefs.DeleteKey($"{RANKING_KEY_PREFIX}Name_{i}");
                PlayerPrefs.DeleteKey($"{RANKING_KEY_PREFIX}Score_{i}");
            }
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 新しいスコアを追加する
    /// </summary>
    /// <param name="name">プレイヤー名</param>
    /// <param name="score">スコア</param>
    /// <returns>ランクイン順位（ランク外の場合は-1）</returns>
    public int AddScore(string name, int score)
    {
        // ランクインするかチェック
        if (rankingList.Count >= MAX_RANKING_COUNT && score <= rankingList[rankingList.Count - 1].Score)
        {
            return -1; // ランク外
        }

        // 新しいスコアを追加
        rankingList.Add(new UserData(0, name, score));

        // スコアで降順ソート
        rankingList = rankingList.OrderByDescending(u => u.Score).ToList();

        // 最大件数を超えたら削除
        if (rankingList.Count > MAX_RANKING_COUNT)
        {
            rankingList.RemoveAt(rankingList.Count - 1);
        }

        // ランクを更新
        UpdateRanks();

        // 保存
        SaveRanking();

        // ランクインした順位を返す
        return rankingList.FindIndex(u => u.Name == name && u.Score == score) + 1;
    }

    /// <summary>
    /// ランクを更新する
    /// </summary>
    private void UpdateRanks()
    {
        for (int i = 0; i < rankingList.Count; i++)
        {
            rankingList[i] = new UserData(i + 1, rankingList[i].Name, rankingList[i].Score);
        }
    }

    /// <summary>
    /// ランキングリストを取得する
    /// </summary>
    public List<UserData> GetRankingList()
    {
        return new List<UserData>(rankingList);
    }

    /// <summary>
    /// ランキングをクリアする
    /// </summary>
    public void ClearRanking()
    {
        rankingList.Clear();
        
        for (int i = 0; i < MAX_RANKING_COUNT; i++)
        {
            PlayerPrefs.DeleteKey($"{RANKING_KEY_PREFIX}Name_{i}");
            PlayerPrefs.DeleteKey($"{RANKING_KEY_PREFIX}Score_{i}");
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 指定スコアがランクインするかチェック
    /// </summary>
    public bool IsRankIn(int score)
    {
        if (rankingList.Count < MAX_RANKING_COUNT)
        {
            return true;
        }
        return score > rankingList[rankingList.Count - 1].Score;
    }
}
