using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<Button>(out var button))
        {
            button.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        Debug.Log(gameObject.name);
    }
}
