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
using UnityEngine.UI;

public class WavePredictionController : MonoBehaviour
{
    public GameObject wavePredictionPanel;

    private List<Image> _wavePredictionImages = new List<Image>();

    private void Awake()
    {
        _wavePredictionImages = wavePredictionPanel.GetComponentsInChildren<Image>().ToList();
    }

    private void Start()
    {
        InitialisePredictionSprites();
    }

    private void OnEnable()
    {
        Debug.Log("ENABLE CALLED");
        WaveManager.AddOnWaveIncrementedAction(UpdatePredictionSpritesInternal);
    }

    private void OnDisable()
    {
        WaveManager.RemoveOnWaveIncrementedAction(UpdatePredictionSpritesInternal);
    }

    private void InitialisePredictionSprites()
    {
        Sprite[] initialWavePrediction = WaveManager.GetInitialWavePrediction();
        for (int i = 0; i < initialWavePrediction.Length; i++)
        {
            if(i >= _wavePredictionImages.Count)
            {
                Debug.LogError("You forgot to increase the number of sprites when you increased the prediction range");
                break;
            }

            _wavePredictionImages[i].sprite = initialWavePrediction[i];
        }
    }

    private void UpdatePredictionSpritesInternal()
    {
        for (int i = 0; i < _wavePredictionImages.Count; i++)
        {
            if (i == _wavePredictionImages.Count - 1)
            {
                _wavePredictionImages[i].sprite = WaveManager.GetNextHiddenWave()._aiTypeIcon;
                return;
            }

            _wavePredictionImages[i].sprite = _wavePredictionImages[i + 1].sprite;
        }
    }
}