using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstVariables
{
    // スクリーン座標でのヒット判定ピクセル閾値
    public const float anchorHitThresholdPx = 200f;
    public const float targetLapTime = 1f;
    
    public const float minScaleThresholdForScale = 0.9f; // ピザの有効幅
    public const float maxScaleThresholdForScale = 1.1f;
    
    public const float timeLimit = 30f; // 制限時間
    
    public const float swipeAreaPercent = 0.75f;
}

