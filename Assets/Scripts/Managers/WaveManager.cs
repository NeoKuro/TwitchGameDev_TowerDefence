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
    public struct WavePredictionAI
    {
        public AIType _aitype;
        public CombatTypeBase _combatType;

        public WavePredictionAI(AIType ai, CombatTypeBase combatType)
        {
            _aitype = ai;
            _combatType = combatType;
        }
    }

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
    [SerializeField]
    private int _waveCombatTypePersistanceBase = 2;
    [SerializeField, Range(0.05f, 1)]
    private float _chanceOfEnhancedWave = 0.1f;

    private List<WavePredictionAI> _wavePredictionList = new List<WavePredictionAI>();
    private List<WavePredictionAI?> _bossPredictionList = new List<WavePredictionAI?>();

    private int _currentWavePrediction = 0;
    private int _currentBossesDefeatedPrediction = 0;
    private int _currentCombatTypePersistance = 2;
    private int _totalWavePredictionSize = 10;


    protected override void Awake()
    {
        base.Awake();
        Initialise();
    }

    public override void Initialise()
    {
        _waveCountTxt.text = _waveCount.ToString();
        _totalWavePredictionSize = _wavePredictionRange + _waveHiddenRange;
        _currentCombatTypePersistance = _waveCombatTypePersistanceBase;
        // Create a list of waves up to _wavePredictionRange + 5;
        PopulateWavePredictionList();
        InitialiseBossPredictionList();
        a_OnWaveIncremented?.Invoke();
    }

    public override void OnRetryExecuted()
    {
        _waveCount = 0;
        _currentWavePrediction = 0;
        _currentBossesDefeatedPrediction = 0;
        _currentCombatTypePersistance = _waveCombatTypePersistanceBase;
        _wavePredictionList.Clear();
        _bossPredictionList.Clear();
        Initialise();
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

        Debug.LogFormat("The NEW wave is: {0}. Good Luck", _wavePredictionList[0]._aitype.Name);
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
                AIType ai = AIManager.GetRandomBossType();
                WavePredictionAI newBoss = new WavePredictionAI(ai, ai._aiTypeReference.AICombatType);

                _bossPredictionList.Add(newBoss);
            }
            else
            {
                _bossPredictionList.Add(null);
            }
        }
    }

    private WavePredictionAI[] GetInitialWavePredictionInternal()
    {
        WavePredictionAI[] wavePrediction = new WavePredictionAI[_wavePredictionRange];

        for (int i = 0; i < _wavePredictionRange; i++)
        {
            wavePrediction[i] = _wavePredictionList[i];
        }

        return wavePrediction;
    }

    private WavePredictionAI?[] GetInitialWavePrediction_BossInternal()
    {
        WavePredictionAI?[] wavePrediction = new WavePredictionAI?[_wavePredictionRange];

        for (int i = 0; i < _wavePredictionRange; i++)
        {
            if (_bossPredictionList[i].HasValue)
            {
                wavePrediction[i] = _bossPredictionList[i].Value;
            }
            else
            {
                wavePrediction[i] = null;
            }
        }

        return wavePrediction;
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
        // Maintain the same Combat TYpe for X rounds (see difficulty later on?)
        CombatTypeBase _chosenType = _wavePredictionList.Count > 0 ? _wavePredictionList[_wavePredictionList.Count - 1]._combatType : null;
        while (_wavePredictionList.Count < _totalWavePredictionSize)
        {
            _currentWavePrediction++;

            if (_chosenType == null || (_currentWavePrediction - 1) % _currentCombatTypePersistance == 0)
            {
                if (_chosenType != null)
                {
                    _currentCombatTypePersistance = _currentCombatTypePersistance <= 1 ? 1 : _currentCombatTypePersistance - 1;
                }

                _chosenType = AIManager.Instance.AvailableCombatTypes[Random.Range(0, AIManager.Instance.AvailableCombatTypes.Count)];
            }

            AIType ai = AIManager.GetRandomAIType();
            WavePredictionAI newAI = new WavePredictionAI(ai, _chosenType);
            _wavePredictionList.Add(newAI);

            if (_currentWavePrediction % _bossFrequency == 0)
            {
                _currentBossesDefeatedPrediction++;
                AIManager.BossDefeatPredicted(_currentBossesDefeatedPrediction);
            }
        }
    }

    private void PopulateBossPredictionList()
    {
        //          2                           20            /         10
        int desiredBossPredictionsInList = _bossPredictionList.Count / _bossFrequency;
        if (_bossPredictionList.Where(x => { return x != null; }).Count() < desiredBossPredictionsInList)
        {
            AIType ai = AIManager.GetRandomBossType();
            WavePredictionAI newBoss = new WavePredictionAI(ai, ai._aiTypeReference.AICombatType);

            _bossPredictionList.Add(newBoss);
            return;
        }

        _bossPredictionList.Add(null);
    }


    #region - Public Static Access Methods -
    public static void IncrementWaveCount()
    {
        Instance?.IncrementWaveCount_Internal();
    }

    public static WavePredictionAI GetCurrentWave()
    {
        return Instance._wavePredictionList[0];
    }

    public static WavePredictionAI? GetCurrentBossWave()
    {
        return Instance._bossPredictionList[0];
    }

    public static WavePredictionAI GetNextHiddenWave()
    {
        return Instance._wavePredictionList[Instance._wavePredictionRange - 1];
    }

    public static WavePredictionAI? GetNextHiddenBossWave()
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

    public static WavePredictionAI[] GetInitialWavePrediction()
    {
        return Instance?.GetInitialWavePredictionInternal();
    }

    public static WavePredictionAI?[] GetInitialWavePrediction_Boss()
    {
        return Instance?.GetInitialWavePrediction_BossInternal();
    }

    public static bool GetChanceOfEnhancedAI()
    {
        return Random.Range(0f, 1f) <= Instance?._chanceOfEnhancedWave;
    }
    #endregion - Public Static Access Methods -


}