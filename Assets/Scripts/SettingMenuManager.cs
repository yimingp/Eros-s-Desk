using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuManager : MonoBehaviour
{

    [Title("Setting")]
    public float settingPanelFadeTime;
    public float settingButtonFadeTime;
    
    
    [Title("Data")]
    public bool toggle;

    [Title("Reference")]
    public GameObject settingButton;
    public GameObject settingPanel;

    private Image _settingButtonImage;
    private CanvasGroup _settingPanelGroup;
    
    

    private void Awake()
    {
        _settingButtonImage = settingButton.GetComponent<Image>();
        _settingPanelGroup = settingPanel.GetComponent<CanvasGroup>();
    }

    public void ChangeNumSpawns(float val)
    {
        TitleManager.Instance.numSpawns = Mathf.RoundToInt(val);
    }
    
    public void Toggle()
    {
        toggle = !toggle;
        Toggle(toggle);
    }
    
    private void Toggle(bool status)
    {
        if (status)
        {
            settingButton.transform.DORotate(new Vector3(0, 0, 90), 0.25f);
            _settingButtonImage.DOFade(0, settingButtonFadeTime).OnComplete(() =>
            {
                settingButton.transform.DOKill();
            });


            _settingPanelGroup.DOFade(1, settingPanelFadeTime);
        }
        else
        {
            _settingButtonImage.DOFade(1, settingButtonFadeTime);
            _settingPanelGroup.DOFade(0, settingPanelFadeTime);
        }
            
    }
}
