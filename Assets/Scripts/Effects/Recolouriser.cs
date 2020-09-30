//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recolouriser : MonoBehaviour
{
    [SerializeField]
    private Color _initialColour = Color.white;
    [SerializeField]
    private float _timeToRecolourise = 1f;

    [SerializeField]
    private Renderer _renderer;

    private Material _mainMaterial;
    private Color _newColour = Color.red;

    private Coroutine _currentCoroutine;

    private void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        if (_renderer == null)
        {
            Debug.LogErrorFormat(this, "Tried to find Renderer on object, but could not!");
            return;
        }

        _mainMaterial = _renderer.material;

        if (_mainMaterial == null)
        {
            Debug.LogErrorFormat(this, "Could not find Material or Renderer!");
            return;
        }

        _mainMaterial = new Material(_mainMaterial);
        _mainMaterial.color = _initialColour;
        _renderer.material = _mainMaterial;
    }

    public void BeginRecolourise(Color newColour, bool forceRecolour = false)
    {
        if (_currentCoroutine != null)
        {
            if (!forceRecolour)
            {
                Debug.LogErrorFormat(this, "We are already colourising this object to colour {0},  trying to recolour to {1}. Will not do it!", _newColour.ToString(), newColour.ToString());
                return;
            }
            StopCoroutine(_currentCoroutine);
        }
        _newColour = newColour;
        _currentCoroutine = StartCoroutine(Recolourise());
    }


    private IEnumerator Recolourise()
    {
        Color fadeCol = _newColour;
        float initA = fadeCol.a;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= Mathf.Clamp((initA / _timeToRecolourise) * Time.deltaTime, 0f, 1f);
            _mainMaterial.color = fadeCol;
            yield return new WaitForEndOfFrame();
        }

        _currentCoroutine = null;
        Destroy(transform.root.gameObject);
    }
}