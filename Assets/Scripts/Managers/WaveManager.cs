//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField]
    private int _waveCount;
    [SerializeField]
    private RangeInt _increaseRange = new RangeInt(2, 5);
    [SerializeField]
    private Text _waveCountTxt;

    private void Start()
    {
        _waveCountTxt.text = _waveCount.ToString();
    }

    private void IncrementWaveCount_Internal()
    {
        _waveCount++;
        _waveCountTxt.text = _waveCount.ToString();
        AISpawnManager.IncreaseWaveDifficulty(_increaseRange.Random);
    }

    public static void IncrementWaveCount()
    {
        Instance?.IncrementWaveCount_Internal();
    }
}