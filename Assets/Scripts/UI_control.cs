using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

// Author Arely M.
public class UI_control : MonoBehaviour
{
    public Dropdown resolutions;
    public Button exit;
    public Button Sound;
    public AudioMixer audio_mixer;
    public Sprite Sound_on, Sound_off;
    public AudioSource audioSource;
    private bool mute;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        exit.gameObject.SetActive(true);
        resolutions.value = resolutions.options.Count - 1;
        audio_mixer.SetFloat("volume", 0);
        Head.SetMute(mute);
    }

    public void PlayGame() //  I charge the game scene
    {
        SceneManager.LoadScene(1);
    }

    public  void SetFullscreen(bool state) // set game in fullscren
    {
        Screen.fullScreen = state;
        if(state)
        {
            exit.gameObject.SetActive(true);
            resolutions.options.Add(new Dropdown.OptionData("1920 x 1080"));
        }
        else
        {
            exit.gameObject.SetActive(false);
            resolutions.options.RemoveAt(resolutions.options.Count - 1);
        }
    }

    public void SetResolution(int resolution_index)
    {
        switch(resolution_index)
        {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;

            case 1:
                Screen.SetResolution(1360, 768, Screen.fullScreen);
                break;

            case 2:
                Screen.SetResolution(1440, 900, Screen.fullScreen);
                break;

            case 3:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;

            case 4:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetVolume(float volume) // change the volume 
    {
        audio_mixer.SetFloat("volume", volume); // i use an audio mixer to change volumen because i believe is better
    }

    public void SetMute()
    {
        mute = !mute;
        if(mute)
        {
            Sound.GetComponent<Image>().sprite = Sound_off;
            audioSource.mute = mute; // silence this scene (main menu)
        }
        else
        {
            Sound.GetComponent<Image>().sprite = Sound_on;
            audioSource.mute = mute;
        }
        Head.SetMute(mute); // silence the game scene 
    }
}
