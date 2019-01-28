﻿using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class MoneyCounterUI : MonoBehaviour
{
    private Text moneyText;
    // Start is called before the first frame update

    private void Awake()
    {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "MONEY: " + GameMaster.Money;
    }
}

