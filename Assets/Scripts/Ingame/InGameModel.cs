using System.Collections.Generic;
using R3;
using UnityEngine;

public class InGameModel
{
    // アンカーたち
    public List<Anchor> anchors { get; private set; }

    // 現在のアンカーナンバー
    public ReactiveProperty<int> currentAncorIndex { get; private set; }

    // 残り時間
    public ReactiveProperty<float> currentRestTime { get; private set; }

    // 今回のラップタイム
    public ReactiveProperty<float> oneLapTime { get; private set; }

    // 回転速度
    public ReactiveProperty<float> pizzaRotateSpeed { get; private set; }

    // 現在の大きさ
    public ReactiveProperty<float> pizzaScale { get; private set; }

    // 一周しました。
    public Subject<Unit> OnFinishedOneLap  { get; private set; }
    
    public ReactiveProperty<float> limitScale { get; private set; }   // 最大拡大率
    
    public ReactiveProperty<int> pizzaCount { get; private set; } // ピザの数

    public ReactiveProperty<int> currentPizzaType { get; private set; }

    public InGameModel()
    {
        OnFinishedOneLap   = new Subject<Unit>();
        anchors            = new List<Anchor>();
        currentRestTime        = new ReactiveProperty<float>();
        currentAncorIndex  = new ReactiveProperty<int>();
        oneLapTime         = new ReactiveProperty<float>();
        pizzaRotateSpeed   = new ReactiveProperty<float>();
        pizzaScale         = new ReactiveProperty<float>();
        limitScale         = new ReactiveProperty<float>();
        pizzaCount         = new ReactiveProperty<int>();
    }

    public bool HasAnchors => anchors != null && anchors.Count > 0;

    // アンカーリストをセット
    public void SetAncor(List<Anchor> anchorList)
    {
        if (anchorList == null || anchorList.Count == 0)
        {
            Debug.LogWarning("[InGameModel] SetAncor called with null or empty list.");
            anchors = new List<Anchor>();
            currentAncorIndex.Value = 0;
            return;
        }

        anchors = anchorList;
        currentAncorIndex.Value = Mathf.Clamp(currentAncorIndex.Value, 0, anchors.Count - 1);
        Debug.Log($"[InGameModel] anchors set: {anchors.Count}");
    }
    
    // 手の位置(スクリーン座標) と 現在のアンカー(Transform) との距離をピクセル単位で返す
    public float HandDistanceToCurrentAnchor_ScreenSpace(Vector2 handScreenPos, Camera uiCamera /* null */)
    {
        if (!HasAnchors) return float.PositiveInfinity;

        int idx = Mathf.Clamp(currentAncorIndex.Value, 0, anchors.Count - 1);

        // アンカー(Transform) のワールド位置 → スクリーン座標へ
        Vector2 anchorScreenPos = RectTransformUtility.WorldToScreenPoint(
            uiCamera, // Overlay は null
            anchors[idx].anchorTransform.position
        );

        float distancePx = Vector2.Distance(handScreenPos, anchorScreenPos);
        return distancePx;
    }

    // 次のアンカーへ
    public void AddCurrentAnchorIndex()
    {
        if (!HasAnchors) return;

        currentAncorIndex.Value++;
        if (currentAncorIndex.Value >= anchors.Count)
        {
            OnFinishedOneLap.OnNext(Unit.Default);
            currentAncorIndex.Value = 0;
        }
        Debug.Log($"[InGameModel] next anchor: {currentAncorIndex.Value}/{anchors.Count}");
    }

    // 一周
    public void finishedOneLap()
    {
        pizzaRotateSpeed.Value = UpdateSpeedByLap(oneLapTime.Value, pizzaRotateSpeed.Value);
        Debug.Log(pizzaRotateSpeed.Value);
        oneLapTime.Value = 0;
        // Debug.Log("[InGameModel] finished one lap");
    }
    

    // 時間増加（ラップタイム加算もここで）
    public void ChangeTime(float time)
    {
        currentRestTime.Value -= time;
        oneLapTime.Value  += time;
    }
    
    // ピザ拡大
    public void TickScalingByFrame()
    {
        // pizzaRotateSpeed をそのまま拡大速度に利用
        float currSpeed = pizzaRotateSpeed.Value;

        // スピードに比例した増分
        float delta = currSpeed * Time.deltaTime * 0.15f;
        
        if (delta > 0f)
        {
            pizzaScale.Value += delta;
        }
    }

    //　速度遅くする。
    public void DelayRotatePizzaSpeed(float bigspeed)
    {
        pizzaRotateSpeed.Value -= bigspeed;
        pizzaRotateSpeed.Value = Mathf.Clamp(pizzaRotateSpeed.Value, 1, 100f);
    }

    public float UpdateSpeedByLap(float currLapTime, float currentSpeed)
    {
        if (currLapTime <= 0f) return currentSpeed;

        // ---- 基準との比較 ----
        // targetLapTime より速い（= lapTime が短い）なら加速
        bool isFast = currLapTime < ConstVariables.targetLapTime;

        // ---- 感度 ----
        const float k = 1f;   // 加速/減速の強さ

        float targetSpeed;
        if (isFast)
        {
            // 加速
            targetSpeed = currentSpeed * Mathf.Pow(1.2f, k); // 倍率は調整可
        }
        else
        {
            // 減速
            targetSpeed = currentSpeed * Mathf.Pow(0.8f, k); // 倍率は調整可
        }

        // ---- スムージング ----
        float alpha = 0.25f;
        float newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, alpha);

        // ---- 範囲制限 ----
        newSpeed = Mathf.Clamp(newSpeed, 1, 100);

        return newSpeed;
    }

    public bool shippingPizza()
    {
        if (pizzaScale.Value >=limitScale.Value*ConstVariables.minScaleThresholdForScale && pizzaScale.Value<=limitScale.Value*ConstVariables.maxScaleThresholdForScale) 
        {
            // ピザ完成
            Debug.Log("Pizza Shipped!");
            InitializePizza();
            pizzaCount.Value++;
            return true;
        }
        else
        {
            // ピザ未完成
            Debug.Log("Pizza Not Ready Yet!");
            InitializePizza();
            return false;
        }
    }
    
    // ピザ完成時の初期化
    public void InitializePizza()
    {
        //回転速度初期化
        pizzaRotateSpeed.Value = 0;
        
        //サイズ初期化
        pizzaScale.Value = 1f;

        //最大拡大率再設定
        limitScale.Value = Random.Range(1.5f, 3);
    }
    
    // 初期化
    public void Initialize()
    {
        pizzaCount.Value      = 0;
        currentRestTime.Value       = 0;
        oneLapTime.Value        = 0;
        currentAncorIndex.Value = HasAnchors ? Mathf.Clamp(currentAncorIndex.Value, 0, anchors.Count - 1) : 0;
        pizzaRotateSpeed.Value  = 1f; // 初期速度
        pizzaScale.Value       = 1f; // 初期サイズ
        currentRestTime.Value = ConstVariables.timeLimit; // 制限時間セット
        InitializePizza();
        Debug.Log("[InGameModel] Initialize done");
    }
}