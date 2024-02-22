using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycleTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // AddLife
    // RemoveLife
    // Rotate
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LifeCycleTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private GUIMenu x;

    // Update is called once per frame
    void Update()
    {
        x = GetComponentInChildren<GUIMenu>(true);

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (x.paused)
            {
                x.UnPause();
                x.paused = false;
            }

            else
            {
                x.paused = true;
                x.Pause();
            }
        }
    }
}

/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    /// <summary>
    /// Allows child images rotation.
    /// </summary>
    public class ImageRotator : MonoBehaviour
    {
        private Queue<Image> images = new Queue<Image>() ;

        private void Awake()
        {
            images = new Queue<Image>(GetComponentsInChildren<Image>());

        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="turns"></param>
        public void Rotate(uint turns = 1)
        {
            for (var i = 0; i < turns; i++)
            {
                var image = images.Dequeue();

                image.transform.SetAsLastSibling();

                images.Enqueue(image);
            }
        }
    }
}*/