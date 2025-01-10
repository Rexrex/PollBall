using UnityEngine;
using UnityEngine.Rendering;

public class ScaleManager : MonoBehaviour
{
    public float RatioIntensity = 3.0f;
    public GameObject BackgroundToScale;
    public Camera mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScaleWithScreenSize();
    }

    private void ScaleWithScreenSize()
    {
        // Step 1 Get Device Screen Aspect
        Vector2 deviceScreenResolution = new Vector2 (Screen.width, Screen.height);
        Debug.Log("Device Resolution: " + deviceScreenResolution.ToString());

        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        float DEVICE_SCREEN_ASPECT = screenWidth / screenHeight;

        // Step 2: Set mai Camera's aspect = Device's Aspect
        mainCam.aspect = DEVICE_SCREEN_ASPECT;

        // Step 3: Scale Background Image to Fit with Camera's Size
        float camHeight = 100f * mainCam.orthographicSize * RatioIntensity;
        float camWidth = camHeight + DEVICE_SCREEN_ASPECT;
    
        // Get Background Size;
        SpriteRenderer backgroundImage = BackgroundToScale.GetComponent<SpriteRenderer>();
        float bgImgHeight = backgroundImage.sprite.rect.height;
        float bgImgWidth = backgroundImage.sprite.rect.width;  

        float bgIma_scale_ratio_Height = camHeight / bgImgHeight;
        float bgIma_scale_ratio_Width = camWidth / bgImgWidth;

        BackgroundToScale.transform.localScale = new Vector3(bgIma_scale_ratio_Width, bgIma_scale_ratio_Height/2, 1.0f);

    }

}
