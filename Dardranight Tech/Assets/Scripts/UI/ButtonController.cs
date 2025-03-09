using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Sprite hoverButtonImage;
    [SerializeField] private Sprite normalButtonImage;
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private GameObject rankPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private TMP_InputField input;

    public void OnHoverEnter()
    {
        if(gameObject.TryGetComponent<Image>(out Image image))
        {
            image.sprite = hoverButtonImage;
            sfxSource.resource = hoverAudioClip;
            sfxSource.Play();
        }
    }

    public void OnHoverExit()
    {
        if (gameObject.TryGetComponent<Image>(out Image image))
        {
            image.sprite = normalButtonImage;
        }
    }

    public void OnClickPlay()
    {
        sfxSource.resource = clickAudioClip;
        sfxSource.Play();
        Invoke("LoadScene", sfxSource.clip.length);
    }
    public void OnClickExit()
    {
        sfxSource.resource = clickAudioClip;
        sfxSource.Play();
        Invoke("QuitGame", sfxSource.clip.length);
    }

    public void OnClickRank()
    {
        sfxSource.resource = clickAudioClip;
        sfxSource.Play();
        menuPanel.SetActive(false);
        rankPanel.SetActive(true);
    }

    public void OnClickBack()
    {
        sfxSource.resource = clickAudioClip;
        sfxSource.Play();
        menuPanel.SetActive(true);
        rankPanel.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }


    private void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartGame()
    {
        sfxSource.resource = clickAudioClip;
        sfxSource.Play();
        namePanel.SetActive(false);
        gameCanvas.SetActive(true);
        GameObject.Find("Player").GetComponent<PlayerController>().NamePlayer = input.text;
        Time.timeScale = 1f;
    }
   
}
