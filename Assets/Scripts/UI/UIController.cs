//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Serializable]
    public struct BuildObjectPairing
    {
        public int buildCost;
        public GameObject ghostObject;
        public GameObject toSpawnObject;
    }

    public List<BuildObjectPairing> buildObjectPairingList = new List<BuildObjectPairing>();

    public void OnBuildButtonPressed()
    {
        GameManager.SetCursorGameModeToBuild(buildObjectPairingList[0]);
    }

    public void OnRetryButtonpressed()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnQuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}