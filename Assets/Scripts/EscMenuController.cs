using System;
using Sirenix.OdinInspector;
using Unit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscMenuController : MonoBehaviour
{
    [Title("Reference")]
    public GameObject escMenu;
    public GameObject musicButton;

    public AudioSource backgroundAudio;
    public AudioSource definedAudio;
    
    public UnitRelationshipGUIDrawer relationshipDrawer;
    public PusherGUIController pusherController;

    private bool _toggle = false;
    private bool _musicToggle = true;
    private Color _musicButtonColor;
    
    public void OpenMenu()
    {
        escMenu.SetActive(true);

        relationshipDrawer.canDraw = false;
        pusherController.canUse = false;
    }

    public void CloseMenu()
    {
        escMenu.SetActive(false);

        relationshipDrawer.canDraw = true;
        pusherController.canUse = true;
    }

    public void ToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }

    public void ToggleMusic()
    {
        _musicToggle = !_musicToggle;
        if (!_musicToggle)
        {
            _musicButtonColor = musicButton.GetComponent<Image>().color;
            musicButton.GetComponent<Image>().color = _musicButtonColor.ModifiedAlpha(_musicButtonColor.a / 2);

            backgroundAudio.enabled = false;
            definedAudio.enabled = false;
        }
        else
        {
            musicButton.GetComponent<Image>().color = _musicButtonColor;
            
            backgroundAudio.enabled = true;
            definedAudio.enabled = true;
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _toggle = !_toggle;
            if(_toggle)
                OpenMenu();
            else
                CloseMenu();
        }
    }
}