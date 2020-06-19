//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public int mapXSize = 50;
    public int mapYSize = 50;

    private bool[][] _gridCellStatus;       // True means this cell is occupied

    protected override void Awake()
    {
        base.Awake();
        _gridCellStatus = new bool[mapYSize][];
        for (int i = 0; i < mapYSize; i++)
        {
            _gridCellStatus[i] = new bool[mapXSize];
        }
    }

    private bool GetCellStateInternal(int cellX, int cellY)
    {
        Debug.LogError("Cell X : " + cellX + "    cellY : " + cellY);
        return _gridCellStatus[cellY][cellX];
    }

    private void UpdateCellStateInternal(int cellX, int cellY)
    {
        _gridCellStatus[cellY][cellX] = true;
    }

    public static void UpdateCellState(int cellX, int cellY)
    {
        Instance?.UpdateCellStateInternal(cellX, cellY);
    }

    public static bool GetCellState(int cellX, int cellY)
    {
        if(Instance == null)
        {
            Debug.LogError("Trying to get cell state when Instance of MapManager is Null!");
            return false;
        }
        return Instance.GetCellStateInternal(cellX, cellY);
    }
}