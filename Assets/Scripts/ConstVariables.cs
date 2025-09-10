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
    
    public const float swipeAreaPercent = 0.9f;
    
    public static ModeParameter easyModeParameter = new ModeParameter(30, 0.9f, 1.1f);
    public static ModeParameter normalModeParameter = new ModeParameter(30, 0.9f, 1.1f);
    public static ModeParameter hardModeParameter = new ModeParameter(30, 0.9f, 1.1f);
}

public class ModeParameter
{
    public ModeParameter(int time, float minScaleThresholdForScale, float maxScaleThresholdForScale)
    {
        this.time = time;
        this.minScaleThresholdForScale = minScaleThresholdForScale;
        this.maxScaleThresholdForScale = maxScaleThresholdForScale;
    }
    
    public int time;
    public float minScaleThresholdForScale;
    public float maxScaleThresholdForScale;
}

