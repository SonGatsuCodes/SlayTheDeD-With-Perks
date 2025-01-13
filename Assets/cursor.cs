using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class cursor : MonoBehaviour
{
    
    Ray ray;
    bool mobile;
    /*void Start ()
    {
        mobile=UnityEngine.SystemInfo.deviceType == DeviceType.Desktop;
        print(mobile);
    }*/
    void Update()
    {
        if(mobile){}else{ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ray.origin = new Vector3(ray.origin.x, ray.origin.y,-500/* offsetDrawline*/);        
        transform.position=ray.origin;}
    }
    /*
    public float magnification = 2.0f;
    public Camera mainCamera,camera2;
    public RawImage magnifyingGlassImage;
    public RawImage magnifiedImage;

    private RenderTexture capturedTexture;

    void Start()
    {
        capturedTexture = new RenderTexture(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2, 16); // Adjust size as needed
        magnifiedImage.enabled = false; // Initially hide magnified image
    }
    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world coordinates (adjust for UI scaling if necessary)
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Define capture area based on mouse position and magnifying glass size
        float captureX = worldPosition.x - magnifyingGlassImage.rectTransform.rect.width / 2;
        float captureY = worldPosition.y - magnifyingGlassImage.rectTransform.rect.height / 2;
        float captureWidth = magnifyingGlassImage.rectTransform.rect.width;
        float captureHeight = magnifyingGlassImage.rectTransform.rect.height;

        // Set camera render target and render the captured area
        capturedTexture=camera2.targetTexture  ;
        camera2.RenderWithShader(Shader.Find("RenderTexture/Sampling"), null); // Use appropriate shader for rendering
        camera2.targetTexture = null;

        magnifiedImage.texture = capturedTexture;
        magnifiedImage.rectTransform.localScale = new Vector3(magnification, magnification, 1f);
        magnifiedImage.transform.position = worldPosition; // Position magnified image at mouse position

        magnifiedImage.enabled = true; // Show magnified image after update
    }

*/
}
