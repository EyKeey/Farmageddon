using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnergy : MonoBehaviour
{
    public Transform energyPanel;


    public void BuyEnergy()
    {
        EnergyManager.instance.currentEnergy = EnergyManager.instance.maxEnergy;
        EnergyManager.instance.UpdateEnergyUI();
        UIAnimator.Instance.HideUI(energyPanel.gameObject, 0.3f);
        UIAnimator.Instance.HideBackground();
    
    }

    public void OpenEnergyPanel()
    {
        UIAnimator.Instance.ShowUI(energyPanel.gameObject, 0.3f);
        UIAnimator.Instance.ShowBackground();
    }
}
