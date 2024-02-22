using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Menus excluded from ResetOptions.")]
    private Transform[] exclude;

    private Transform[] options;

    private void Awake()
    {
        options = transform.Cast<Transform>().Except(exclude).ToArray();
    }


    /// <summary>
    /// Disable all options.
    /// </summary>
    public void OptionsReset()
    {
        // Resets options menu
        foreach (var option in options)
        {
            option.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Pick option.
    /// </summary>
    /// <param name="selectedOption"></param>
    public void SelectOption(Transform selectedOption)
    {
        foreach (var option in options)
        {
            if (selectedOption == option)
            {
                selectedOption.gameObject.SetActive(true);
            }
            else
            {
                option.gameObject.SetActive(false);
            }
        }
    }
}
