//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Serializable]
    public struct BuildObjectPairing
    {
        public GameObject ghostObject;
        public GameObject toSpawnObject;
    }

    public List<BuildObjectPairing> buildObjectPairingList = new List<BuildObjectPairing>();

    public void OnBuildButtonPressed()
    {
        GameManager.SetCursorGameModeToBuild(buildObjectPairingList[0]);
    }
}