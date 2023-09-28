using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    [SerializeField] private Vector2 baseResolution = new Vector2(720, 1280);
    [SerializeField] private bool matchWidth = true;
    [SerializeField] private bool matchHeight = true;

    private float baseAspect;
    private float currentAspect;

    private void Awake()
    {


    }

    private void ApplyScale()
    {
        if (Screen.width <= 1080f)
        {
            float scale = 1f;

            if (matchWidth && matchHeight)
            {
                scale = currentAspect / baseAspect;
            }
            else if (matchWidth)
            {
                scale = (float)Screen.width / baseResolution.x;
            }
            else if (matchHeight)
            {
                scale = (float)Screen.height / baseResolution.y;
            }

            transform.localScale = new Vector3(scale, scale, 1f);
        }

    }


    private void Start()
    {
        baseAspect = baseResolution.x / baseResolution.y;
        currentAspect = (float)Screen.width / Screen.height;

        ApplyScale();


    }
    private void Update()
    {
        float newAspect = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(newAspect, currentAspect))
        {
            return;
        }

        currentAspect = newAspect;

        ApplyScale();

    }

}
