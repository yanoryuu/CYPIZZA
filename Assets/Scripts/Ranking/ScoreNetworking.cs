using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreNetworking : MonoBehaviour
{
    private string serverUrl = "https://cypizza-score-server-1.onrender.com";

    public async UniTask<bool> SendScoreAsync(string username, int score)
    {
        Debug.Log("Sending score to server...");
        string url = $"{serverUrl}/submit_score/";

        // JSON文字列に変換
        string json = JsonConvert.SerializeObject(new ScoreData(username, score));
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"SendScore failed: {request.error}, Response: {request.downloadHandler.text}");
                return false;
            }

            return true;
        }
    }


    public async UniTask<LeaderboardData> GetLeaderboardAsync()
    {
        string url = $"{serverUrl}/leaderboard/";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest().ToUniTask();
            return JsonConvert.DeserializeObject<LeaderboardData>(request.downloadHandler.text);
        }
    }
}

public class ScoreData
{
    public string username;
    public int score;

    public ScoreData(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}

// ランキングデータの構造体
[Serializable]
public class LeaderboardEntry
{
    public string username;
    public int score;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> leaderboard;
}