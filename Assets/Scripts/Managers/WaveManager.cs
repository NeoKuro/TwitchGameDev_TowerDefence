//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

using AIType = AIManager.AIType;

public class WaveManager : Singleton<WaveManager>
{
    private Action a_OnWaveIncremented; 

    [SerializeField]
    private int _waveCount;
    [SerializeField]
    private RangeInt _increaseRange = new RangeInt(2, 5);
    [SerializeField]
    private Text _waveCountTxt;

    [SerializeField]
    private int _wavePredictionRange = 5;

    private List<AIType> _wavePredictionList = new List<AIType>();


    protected override void Awake()
    {
        base.Awake();
        _waveCountTxt.text = _waveCount.ToString();

        // Create a list of waves up to _wavePredictionRange + 5;
        PopulateWavePredictionList();
        a_OnWaveIncremented?.Invoke();
        Debug.Log("Awake DONE DID IT!");
    }

    private void IncrementWaveCount_Internal()
    {
        _waveCount++;
        _waveCountTxt.text = _waveCount.ToString();
        AISpawnManager.IncreaseWaveDifficulty(_increaseRange.Random);
        UpdateWavePredictions();
        PopulateWavePredictionList();

        a_OnWaveIncremented?.Invoke();

        // Go to next wave
        // Generate new hidden wave

        Debug.LogFormat("The NEW wave is: {0}. Good Luck", _wavePredictionList[0].Name);
    }

    private void RemoveOnWaveIncrementedActionInternal(Action toRemove)
    {
        Debug.Log("REMOVE called!");
        a_OnWaveIncremented -= toRemove;
    }

    private void AddOnWaveIncrementedActionInternal(Action toAdd)
    {
        a_OnWaveIncremented += toAdd;

        Debug.Log("Added to Actions List. Count: " + a_OnWaveIncremented);
    }

    private Sprite[] GetInitialWavePredictionInternal()
    {
        Sprite[] sprites = new Sprite[_wavePredictionRange];

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = _wavePredictionList[i]._aiTypeIcon;
        }

        return sprites;
    }

    private void UpdateWavePredictions()
    {
        if (_wavePredictionList.Count <= 0)
        {
            Debug.LogErrorFormat("Attempting to update wave predictions list but the wave is empty question mark?");
            return;
        }
        _wavePredictionList.RemoveAt(0);
    }

    private void PopulateWavePredictionList()
    {
        while(_wavePredictionList.Count < _wavePredictionRange + 5)
        {
            _wavePredictionList.Add(AIManager.GetRandomAIType());
        }
    }


    #region - Public Static Access Methods -
    public static void IncrementWaveCount()
    {
        Instance?.IncrementWaveCount_Internal();
    }

    public static AIType GetCurrentWave()
    {
        return Instance._wavePredictionList[0];
    }

    public static AIType GetNextHiddenWave()
    {
        return Instance._wavePredictionList[Instance._wavePredictionRange - 1];
    }

    public static void AddOnWaveIncrementedAction(Action newAction)
    {
        Instance?.AddOnWaveIncrementedActionInternal(newAction);
    }

    public static void RemoveOnWaveIncrementedAction(Action newAction)
    {
        Instance?.RemoveOnWaveIncrementedActionInternal(newAction);
    }

    public static Sprite[] GetInitialWavePrediction()
    {
        return Instance?.GetInitialWavePredictionInternal();
    }
    #endregion - Public Static Access Methods -
}