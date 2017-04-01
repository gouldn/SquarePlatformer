using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public static float pixelsToUnits = 1f;
    public static float scale = 1f;

    public static float rightSideOfScreen;
    public static float leftSideOfScreen;

    public Camera cam;

    private Vector2 nativeResolution = new Vector2(640, 480);

    void Awake()
    {
        if (cam.orthographic)
        {
            scale = Screen.height / nativeResolution.y;
            pixelsToUnits *= scale;
        }
    }

    void Update()
    {
        rightSideOfScreen = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, cam.nearClipPlane)).x;
        leftSideOfScreen = cam.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, cam.nearClipPlane)).x;
    }

}

