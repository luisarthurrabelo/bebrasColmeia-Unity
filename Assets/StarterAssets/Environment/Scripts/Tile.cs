using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    private Renderer _renderer;

    public void init(bool isOffset)
    {
        SetColor(isOffset ? _offsetColor : _baseColor);
    }

    private void SetColor(Color color)
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer != null && _renderer.material.color != null)
        {
            _renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("Material ou Renderer não encontrado!");
        }
    }
}
