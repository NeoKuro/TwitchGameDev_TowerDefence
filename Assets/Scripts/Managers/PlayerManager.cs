//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private int _currentMoney = 30;
    [SerializeField]
    private int _currentPlayerHealth = 100;

    [SerializeField]
    private Text _currencyDisplay;
    [SerializeField]
    private Text _healthDisplay;
    [SerializeField]
    private GameObject _gameOverPanel;

    private void Start()
    {
        _currencyDisplay.text = _currentMoney.ToString();
        _healthDisplay.text = _currentPlayerHealth.ToString();
    }

    private void IncrementCurrency_Internal(int amountToIncreaseBy)
    {
        _currentMoney += amountToIncreaseBy;
        _currencyDisplay.text = _currentMoney.ToString();
    }

    private void DecrementCurrency_Internal(int amountToDecreaseBy)
    {
        _currentMoney -= amountToDecreaseBy;
        _currencyDisplay.text = _currentMoney.ToString();
    }

    private void IncreaseHealth_Internal(int amountToIncreaseBy)
    {
        _currentPlayerHealth += amountToIncreaseBy;
        _healthDisplay.text = _currentPlayerHealth.ToString();
    }

    private void DecreaseHealth_Internal(int amountToDecreaseBy)
    {
        _currentPlayerHealth -= amountToDecreaseBy;

        if (_currentPlayerHealth <= 0)
        {
            _currentPlayerHealth = 0;
            _gameOverPanel.SetActive(true);
            GameManager.SetHasGameOverToTrue();
        }

        _healthDisplay.text = _currentPlayerHealth.ToString();
    }

    public static int GetCurrentMoney()
    {
        if (Instance != null)
        {
            return Instance._currentMoney;
        }

        return -1;
    }

    public static int GetCurrentHealth()
    {
        if (Instance != null)
        {
            return Instance._currentPlayerHealth;
        }

        return -1;
    }

    public static void IncrementCurrency(int amountToIncreaseBy)
    {
        Instance?.IncrementCurrency_Internal(amountToIncreaseBy);
    }


    public static void DecreaseCurrency(int amountToDecreaseBy)
    {
        Instance?.DecrementCurrency_Internal(amountToDecreaseBy);
    }

    public static void IncreaseHealth(int amountToIncreaseBy)
    {
        Instance?.IncreaseHealth_Internal(amountToIncreaseBy);
    }

    public static void DecreaseHealth(int amountToDecreaseBy)
    {
        Instance?.DecreaseHealth_Internal(amountToDecreaseBy);
    }
}