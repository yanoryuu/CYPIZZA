using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDifficulty", menuName = "ScriptableObjects/StageDifficulty", order = 1)]
[Serializable]
public class StageDifficulty : ScriptableObject
{
    public float ScaleThresholdForScale;
}
