using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {


    [Header("Main Configuration")]

    [Tooltip("The main camera (default = MainCamera)")]
    public Camera Camera;

    [Tooltip("The camera that will be used to render the outline")]
    public Camera OutlineCamera;

    public CanvasManager CanvasManager;

    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers = true;

    [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
    public int RequiredFingerCount;

    [Header("Movement Configuration")]
    [Tooltip("The distance from the camera the world drag delta will be calculated from (this only matters for perspective cameras)")]
    public float Distance = 1.0f;

    [Header("Zoom Configuration")]

    [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
    [Range(-1.0f, 1.0f)]
    public float WheelSensitivity;

    [Tooltip("The target FOV/Size")]
    public float Target = 10.0f;

    [Tooltip("The minimum FOV/Size we want to zoom to")]
    public float Minimum = 10.0f;

    [Tooltip("The maximum FOV/Size we want to zoom to")]
    public float Maximum = 60.0f;

    [Tooltip("How quickly the zoom reaches the target value")]
    public float Dampening = 10.0f;

    [Header("Scale Configuration")]

    [Tooltip("Orthographic Size of the Camera")]
    public float orthographicSize = 5;

    [Tooltip("The aspect ratio of the camera")]
    public float aspect = 1.6f;

    [HideInInspector]
    public Vector3 RemainingDelta;

    

    public delegate void ActiveGUI(); // declare delegate type

    protected ActiveGUI callbackFct;

    protected virtual void Start()
    {
        if (Camera != null && OutlineCamera != null)
        {
            Target = GetCurrent();
        }
    }

    protected virtual void LateUpdate()
    {
        if (!CanvasManager.IsGuiOpened)
        {
            // Get the fingers we want to use
            List<LeanFinger> fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

            //MOVEMENT
            // Get the world delta of all the fingers
            Vector3 worldDelta = LeanGesture.GetWorldDelta(fingers, Distance, Camera);

            // Pan the camera based on the world delta
            RemainingDelta -= worldDelta;

            // The framerate independent damping factor
            System.Single factor = Mathf.Exp(-Dampening * Time.deltaTime);

            // Dampen remainingDelta
            Vector3 newDelta = RemainingDelta * factor;

            // Shift this transform by the change in delta
            transform.position += RemainingDelta - newDelta;

            // Update remainingDelta with the dampened value
            RemainingDelta = newDelta;
            //MOVEMENT

            //ZOOM
            // Scale the current value based on the pinch ratio
            Target *= LeanGesture.GetPinchRatio(fingers, WheelSensitivity);

            // Clamp the current value to min/max values
            Target = Mathf.Clamp(Target, Minimum, Maximum);

            // The framerate independent damping factor
            System.Single factorZoom = 1.0f - Mathf.Exp(-Dampening * Time.deltaTime);

            // Store the current size/fov in a temp variable
            var current = GetCurrent();

            current = Mathf.Lerp(current, Target, factorZoom);

            SetCurrent(current);
            //ZOOM
        }
    }

    private float GetCurrent()
    {
        if (Camera.orthographic == true)
        {
            return Camera.orthographicSize;
        }
        else
        {
            return Camera.fieldOfView;
        }
    }

    private void SetCurrent(float current)
    {
        if (Camera.orthographic == true)
        {
            Camera.orthographicSize = current;
            OutlineCamera.orthographicSize = current;
        }
        else
        {
            Camera.fieldOfView = current;
        }
    }

    public void MoveCameraToTarget(Transform target, ActiveGUI callback)
    {
        Target = 70;
        Camera.transform.DOMove(new Vector3(target.localPosition.x + 80f, target.localPosition.y, Camera.transform.localPosition.z), 0.5f).OnComplete(() => callback());
    }

    public void ReturnCameraToWorld(Transform target, ActiveGUI callback)
    {
        callback();
        Camera.transform.DOMove(new Vector3(target.localPosition.x, target.localPosition.y, Camera.transform.localPosition.z), 0.5f);
        Target = 100;
    }

}

