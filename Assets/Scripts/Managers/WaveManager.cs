//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

using AIType = AIManager.AIType;
using Random = UnityEngine.Random;

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
    [SerializeField]
    private int _waveHiddenRange = 5;
    [SerializeField]
    private int _bossFrequency = 10;
    [SerializeField, Range(0.05f, 1)]
    private float _chanceOfEnhancedWave = 0.1f;

    private List<AIType> _wavePredictionList = new List<AIType>();
    private List<AIType?> _bossPredictionList = new List<AIType?>();

    private int _totalWavePredictionSize = 10;


    protected override void Awake()
    {
        base.Awake();
        _waveCountTxt.text = _waveCount.ToString();

        _totalWavePredictionSize = _wavePredictionRange + _waveHiddenRange;

        // Create a list of waves up to _wavePredictionRange + 5;
        PopulateWavePredictionList();
        InitialiseBossPredictionList();
        a_OnWaveIncremented?.Invoke();

    }

    private void IncrementWaveCount_Internal()
    {
        _waveCount++;
        _waveCountTxt.text = _waveCount.ToString();
        AISpawnManager.IncreaseWaveDifficulty(_increaseRange.Random);
        UpdateWavePredictions();
        PopulateWavePredictionList();
        PopulateBossPredictionList();
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

    private void InitialiseBossPredictionList()
    {
        for (int i = 0; i < _totalWavePredictionSize; i++)
        {
            if ((i + 1) % _bossFrequency == 0)
            {
                _bossPredictionList.Add(AIManager.GetRandomBossType());
            }
            else
            {
                _bossPredictionList.Add(null);
            }
        }
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

    private Sprite[] GetInitialWavePrediction_BossInternal()
    {
        Sprite[] sprites = new Sprite[_wavePredictionRange];

        for (int i = 0; i < sprites.Length; i++)
        {
            if (_bossPredictionList[i].HasValue)
            {
                sprites[i] = _bossPredictionList[i].Value._aiTypeIcon;
            }
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
        _bossPredictionList.RemoveAt(0);
    }

    private void PopulateWavePredictionList()
    {
        while (_wavePredictionList.Count < _totalWavePredictionSize)
        {
            _wavePredictionList.Add(AIManager.GetRandomAIType());
        }
    }

    private void PopulateBossPredictionList()
    {
        //          2                           20            /         10
        int desiredBossPredictionsInList = _bossPredictionList.Count / _bossFrequency;
        if (_bossPredictionList.Where(x => { return x != null; }).Count() < desiredBossPredictionsInList)
        {
            _bossPredictionList.Add(AIManager.GetRandomBossType());
            return;
        }

        _bossPredictionList.Add(null);
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

    public static AIType? GetCurrentBossWave()
    {
        return Instance._bossPredictionList[0];
    }

    public static AIType GetNextHiddenWave()
    {
        return Instance._wavePredictionList[Instance._wavePredictionRange - 1];
    }

    public static AIType? GetNextHiddenBossWave()
    {
        return Instance._bossPredictionList[Instance._wavePredictionRange - 1];
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

    public static Sprite[] GetInitialWavePrediction_Boss()
    {
        return Instance?.GetInitialWavePrediction_BossInternal();
    }

    public static bool GetChanceOfEnhancedAI()
    {
        return Random.Range(0f, 1f) <= Instance?._chanceOfEnhancedWave;
    }
    #endregion - Public Static Access Methods -


}