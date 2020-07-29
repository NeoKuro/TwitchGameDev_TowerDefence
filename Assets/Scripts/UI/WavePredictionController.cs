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
            if(i >= _wavePredictionImages.Count )
            {
                Debug.LogError("You forgot to increase the number of sprites when you increased the prediction range");
                break;
            }

            _wavePredictionImages[i].sprite = initialWavePrediction[i];
            
        }

        initialWavePrediction = WaveManager.GetInitialWavePrediction_Boss();
        for (int i = 0; i < initialWavePrediction.Length; i++)
        {
            if (i >= _bossPredictionImages.Count)
            {
                Debug.LogError("You forgot to increase the number of sprites when you increased the prediction range");
                break;
            }

            if(initialWavePrediction[i] == null)
            {
                _bossPredictionImages[i].enabled = false;
            }
            else
            {
                _bossPredictionImages[i].sprite = initialWavePrediction[i];
                _bossPredictionImages[i].enabled = true;
                _bossPredictionImages[i].color = Color.white;
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
                _wavePredictionImages[i].sprite = WaveManager.GetNextHiddenWave()._aiTypeIcon;
                return;
            }

            _wavePredictionImages[i].sprite = _wavePredictionImages[i + 1].sprite;
        }
    }

    private void UpdateBossPredictionSprite()
    {
        AIType? temp = WaveManager.GetNextHiddenBossWave();
        for (int i = 0; i < _bossPredictionImages.Count; i++)
        {
            if (i == _bossPredictionImages.Count - 1)
            {
                _bossPredictionImages[i].sprite = temp.HasValue ? temp.Value._aiTypeIcon : null;
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