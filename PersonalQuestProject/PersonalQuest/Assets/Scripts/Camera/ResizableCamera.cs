using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizableCamera : MonoBehaviour
{

    private const float optimalRatio = 0.565f; //Ratio 1080p

    void Awake()
    {
        //Sacamos el ratio de la resolución actual
        float ratio = (float)Screen.width / (float)Screen.height;
        float zoomRatio = ratio / optimalRatio;

        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = cam.orthographicSize / zoomRatio;
    }

}