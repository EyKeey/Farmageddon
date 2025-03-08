using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class moneyText : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
         m_TextMeshPro.text = MoneyManager.instance.currentMoney.ToString();
    }
}
