using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuManager : MonoBehaviour
{
    public bool toggle;

    public GameObject settingButton;
    public GameObject settingPanel;

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
        settingButton.GetComponent<Image>().DOFade(toggle ? 0 : 1, 0.25f);

        settingPanel.GetComponent<CanvasGroup>().DOFade(toggle ? 1 : 0, 2f);
    }
}
