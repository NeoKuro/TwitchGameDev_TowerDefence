//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObstruction : MonoBehaviour
{
    [SerializeField]
    private string _customMessage = "Cannot place here. Cell is obstructed!";

   private void Start()
   {
        ObstructCell();
   }

    private void ObstructCell()
    {
        MapManager.UpdateCellState((int)transform.position.x, (int)transform.position.y);
    }
}