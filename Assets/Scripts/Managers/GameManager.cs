//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameMode
    {
        BUILD = 0,
        DEFAULT = 1
    }


    public bool hasGameOvered { get; private set; }
    private GameMode _currentCursorGameMode = GameMode.DEFAULT;
    private UIController.BuildObjectPairing? _currentPairing;
    private GameObject _ghostObject;


    [SerializeField]
    private List<MonoBehaviour> managers = new List<MonoBehaviour>();


    private void Update()
    {
        if (_currentCursorGameMode == GameMode.DEFAULT)
        {
            return;
        }
        else if (_currentCursorGameMode == GameMode.BUILD)
        {
            ProcessBuildMode();
        }

    }

    public override void Initialise()
    {

    }

    public override void OnRetryExecuted()
    {
        hasGameOvered = false;
        _currentCursorGameMode = GameMode.DEFAULT;
    }

    // When retry button is pressed BEFORE the scene switch
    public void OnRetry()
    {
        //for (int i = 0; i < managers.Count; i++)
        //{
        //    Singleton<T> x = managers[i] as Singleton<T>;
        //}
    }

    private void SetCursorGameModeToBuildInternal(UIController.BuildObjectPairing ghostPrefab)
    {
        Debug.Log("Set mode to build!");
        _ghostObject = Instantiate(ghostPrefab.ghostObject, Input.mousePosition, Quaternion.identity);
        _currentCursorGameMode = GameMode.BUILD;
        _currentPairing = ghostPrefab;
    }

    private void ChangeGameOverInternal(bool gameOver)
    {
        hasGameOvered = gameOver;
    }

    private void ProcessBuildMode()
    {
        Vector3 newPos = MoveGhostObject();
        BuildObject(newPos);
    }

    private Vector3 MoveGhostObject()
    {
        float snapValue = 1f / 1f;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0;
        newPos = new Vector3(Mathf.Round(newPos.x / snapValue) * snapValue, Mathf.Round(newPos.y / snapValue) * snapValue, 0);
        _ghostObject.transform.position = newPos + Vector3.one * 0.25f;
        return newPos;

    }

    private void BuildObject(Vector3 newPos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(PlayerManager.GetCurrentMoney() < _currentPairing.Value.buildCost)
            {
                Debug.LogFormat("Insufficient Funds to build Tower: {0}", _currentPairing.Value.toSpawnObject.name);
                return;
            }

            Debug.Log("NewPos: " + newPos);
            if (MapManager.GetCellState((int)newPos.x, (int)newPos.y))
            {
                Debug.Log("This cell is occupied!");
                return;
            }

            PlayerManager.DecreaseCurrency(_currentPairing.Value.buildCost);
            Instantiate(_currentPairing.Value.toSpawnObject, newPos + Vector3.one * 0.25f, Quaternion.identity);
            MapManager.UpdateCellState((int)newPos.x, (int)newPos.y);
        }
    }


    public static void SetCursorGameModeToBuild(UIController.BuildObjectPairing ghostPrefab)
    {
        Instance?.SetCursorGameModeToBuildInternal(ghostPrefab);
    }

    public static void SetHasGameOverToTrue()
    {
        Instance?.ChangeGameOverInternal(true);
    }

    public static void ResetHasGameOver()
    {
        Instance?.ChangeGameOverInternal(false);
    }
}