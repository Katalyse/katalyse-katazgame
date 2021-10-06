using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraCapture : MonoBehaviour
{
    public Camera _camera;
    public TextMesh TextTest;

    public byte[] Capture()
    {
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = _camera.targetTexture;

        _camera.Render();

        Texture2D image = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;
        return image.EncodeToJPG();
    }
}
