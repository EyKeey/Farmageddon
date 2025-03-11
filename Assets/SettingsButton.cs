using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public Transform settingsPanel;




    public void OnClick()
    {
        UIAnimator.Instance.ShowUI(settingsPanel.gameObject, 0.3f);
        UIAnimator.Instance.ShowBackground();
    }


}
