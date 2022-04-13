using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProgressData
{
    public static int CurrentProgress;
    public static int MaximumProgress = 10;
    public static int CurrentLevel;
    public static int MaximumLevel = 10;

    public static event Action<float> ProgressChanged;
    public static event Action LevelChanged;

    public static void AddProgressPoint(int countAdditiveProgressPoint)
    {
        if (CurrentProgress + countAdditiveProgressPoint >= MaximumProgress)
        {
            if (CurrentLevel < MaximumLevel)
            {
                CurrentLevel++;
                LevelChanged?.Invoke();
                Debug.Log("Уровень повышен");
            }

            CurrentProgress = CurrentProgress + countAdditiveProgressPoint - MaximumProgress;
        }
        else
        {
            CurrentProgress += countAdditiveProgressPoint;
        }
        float currentProgressPercantage = (float) CurrentProgress / MaximumProgress;
        ProgressChanged?.Invoke(currentProgressPercantage);
    }
}
