using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class RankingLoader : MonoBehaviour
{
    public ScoreNetworking _scoreNetWorking;
    public static int MAX_DATA_NUM = 5; // Å‘åæ“¾”
    
    public async UniTask<UserData[]> ResultScore(int score,string playerName)
    {
        bool success = await _scoreNetWorking.SendScoreAsync(playerName, score);
        UserData[] users = null;
        if (success)
        {
            users = await FetchAndDisplayLeaderboard();
        }

        return users;
    }
    
    private async UniTask<UserData[]> FetchAndDisplayLeaderboard()
    {
        LeaderboardData leaderboard = await _scoreNetWorking.GetLeaderboardAsync();
            
        if (leaderboard != null && leaderboard.leaderboard.Count > 0)
        {
            UserData[] users = new UserData[Math.Min(leaderboard.leaderboard.Count, MAX_DATA_NUM)];

            int index = 0;
            foreach (var entry in leaderboard.leaderboard)
            {
                if (index >= MAX_DATA_NUM) break;
                UserData userData = new UserData(index, entry.username, entry.score);
                users[index++] = userData;
            }
            return users;
        }
        else
        {
            return null;
        }
    }
}
