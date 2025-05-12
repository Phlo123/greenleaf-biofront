using UnityEngine;

[System.Serializable]
public struct ScaledStat
{
    public float baseValue;
    public float currentPercent;
    public float maxPercent;

    public float Value => baseValue * Mathf.Min(currentPercent, maxPercent) / 100f;
}
