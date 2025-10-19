using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue; // |EN| The base value of the stat |TR| Stat'ın temel değeri

    public float GetValue()
    {
        return baseValue;
    }
}
