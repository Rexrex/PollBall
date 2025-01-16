using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sprite MuteTexture;
    public Sprite SoundTexture;

    private bool muted = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MuteUnmuteAudio()
    {
        if (muted)
        {
            GameObject.FindFirstObjectByType<AudioListener>().enabled = true;
            muted = false;
        }

        else
        {
            muted = true;
            GameObject.FindFirstObjectByType<AudioListener>().enabled = false;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (muted)
        {
            this.GetComponent<Image>().sprite = MuteTexture;
        }

        else
        {
            this.GetComponent<Image>().sprite = SoundTexture;
        }
    }
}
