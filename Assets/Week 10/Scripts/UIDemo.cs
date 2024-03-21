using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDemo : MonoBehaviour
{
    public Color colour1;
    public Color colour2;

    public TMP_Dropdown dropdown;

    private float interpolation;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRenderer.color = Color.Lerp(colour1, colour2, interpolation);
    }

    public void SliderHasChangedValue(float value)
    {
        interpolation = value;
    }

    public void DropdownHasChangedValue(int value)
    {
        Debug.Log(dropdown.options[value].text);
    }
}
