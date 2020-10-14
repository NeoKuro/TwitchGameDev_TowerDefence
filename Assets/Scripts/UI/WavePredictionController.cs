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
using static AIManager;

public class WavePredictionController : MonoBehaviour
{
    public GameObject wavePredictionPanel;
    public GameObject bossPredictionPanel;

    private List<Image> _wavePredictionImages = new List<Image>();
    private List<Image> _bossPredictionImages = new List<Image>();

    private void Awake()
    {
        _wavePredictionImages = wavePredictionPanel.GetComponentsInChildren<Image>().ToList();
        _bossPredictionImages = bossPredictionPanel.GetComponentsInChildren<Image>().ToList();
    }

    private void Start()
    {
        InitialisePredictionSprites();
    }

    private void OnEnable()
    {
        WaveManager.AddOnWaveIncrementedAction(UpdatePredictionSpritesInternal);
    }

    private void OnDisable()
    {
        WaveManager.RemoveOnWaveIncrementedAction(UpdatePredictionSpritesInternal);
    }

    private void InitialisePredictionSprites()
    {
        WaveManager.WavePredictionAI[] initialWavePrediction = WaveManager.GetInitialWavePrediction();
        for (int i = 0; i < initialWavePrediction.Length; i++)
        {
            if(i >= _wavePredictionImages.Count )
            {
                Debug.LogError("You forgot to increase the number of sprites when you increased the prediction range");
                break;
            }

            _wavePredictionImages[i].sprite = initialWavePrediction[i]._aitype._aiTypeIcon;
            _wavePredictionImages[i].color = initialWavePrediction[i]._combatType.RepresentativeTypeColour;

        }

        WaveManager.WavePredictionAI?[] initialBossWavePrediction = WaveManager.GetInitialWavePrediction_Boss();
        for (int i = 0; i < initialBossWavePrediction.Length; i++)
        {
            if (i >= _bossPredictionImages.Count)
            {
                Debug.LogError("You forgot to increase the number of sprites when you increased the prediction range");
                break;
            }

            if(initialBossWavePrediction[i] == null)
            {
                _bossPredictionImages[i].enabled = false;
            }
            else
            {
                _bossPredictionImages[i].sprite = initialBossWavePrediction[i].Value._aitype._aiTypeIcon;
                _bossPredictionImages[i].enabled = true;
                _bossPredictionImages[i].color = initialBossWavePrediction[i].Value._combatType.RepresentativeTypeColour;
            }
        }
    }

    private void UpdatePredictionSpritesInternal()
    {
        UpdateWavePredictionSprites();
        UpdateBossPredictionSprite();
    }

    private void UpdateWavePredictionSprites()
    {
        for (int i = 0; i < _wavePredictionImages.Count; i++)
        {
            if (i == _wavePredictionImages.Count - 1)
            {
                WaveManager.WavePredictionAI waveAI = WaveManager.GetNextHiddenWave();
                _wavePredictionImages[i].sprite = waveAI._aitype._aiTypeIcon;
                _wavePredictionImages[i].color = waveAI._combatType.RepresentativeTypeColour;
                return;
            }

            _wavePredictionImages[i].sprite = _wavePredictionImages[i + 1].sprite;
        }
    }

    private void UpdateBossPredictionSprite()
    {
        WaveManager.WavePredictionAI? temp = WaveManager.GetNextHiddenBossWave();
        for (int i = 0; i < _bossPredictionImages.Count; i++)
        {
            if (i == _bossPredictionImages.Count - 1)
            {
                _bossPredictionImages[i].sprite = temp.HasValue ? temp.Value._aitype._aiTypeIcon : null;
                _bossPredictionImages[i].color = temp.HasValue ? temp.Value._combatType.RepresentativeTypeColour : Color.white;
                _bossPredictionImages[i].enabled = temp.HasValue;
                return;
            }

            _bossPredictionImages[i].sprite = _bossPredictionImages[i + 1].sprite;
            if (_bossPredictionImages[i].sprite == null)
            {
                _bossPredictionImages[i].enabled = false;
            }
            else
            {
                _bossPredictionImages[i].enabled = true;
            }
        }
    }
}