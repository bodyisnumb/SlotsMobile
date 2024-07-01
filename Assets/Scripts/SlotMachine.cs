using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    public TMP_Text moneyText;
    private int price = 1;
    public Slot[] slots;
    public Combinations[] combinations;
    public float timeInterval = 0.025f;
    private int stoppedSlots = 3;
    private bool isSpin = false;
    public bool isAuto;
    public Animator btnAnim;
    public TMP_Text betAmountText;
    private int betAmount = 1;
    private int money;
    private int consecutiveLosses = 0;
    public GameObject freeSpinPanel;


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    private void Start()
    {
        btnAnim = GameObject.FindGameObjectWithTag("AutoButton").GetComponent<Animator>();
        money = PlayerPrefs.GetInt("Money", 1000);
        UpdateMoneyText();
        UpdateBetAmountText();
        freeSpinPanel.SetActive(false);
    }

    public void Spin()
    {
        if (!isSpin && (money - price * betAmount >= 0 || consecutiveLosses >= 3))
        {
            if (consecutiveLosses >= 3)
            {
                freeSpinPanel.SetActive(false);
                consecutiveLosses = 0;
            }
            else
            {
                ChangeMoney(-price * betAmount);
            }

            isSpin = true;
            foreach (Slot i in slots)
            {
                i.StartCoroutine("Spin");
            }
        }
    }

    public void WaitResults()
    {
        stoppedSlots -= 1;
        if (stoppedSlots <= 0)
        {
            stoppedSlots = 3;
            CheckResults();
        }
    }

    public void CheckResults()
    {
        isSpin = false;
        bool win = false;

        foreach (Combinations i in combinations)
        {
            if (slots[0].gameObject.GetComponent<Slot>().stoppedSlot.ToString() == i.FirstValue.ToString()
                && slots[1].gameObject.GetComponent<Slot>().stoppedSlot.ToString() == i.SecondValue.ToString()
                && slots[2].gameObject.GetComponent<Slot>().stoppedSlot.ToString() == i.ThirdValue.ToString())
            {
                ChangeMoney(i.prize * betAmount);
                win = true;
            }
        }

        if (!win)
        {
            consecutiveLosses++;
            if (consecutiveLosses >= 3)
            {
                freeSpinPanel.SetActive(true);
            }
        }
        else
        {
            consecutiveLosses = 0;
        }

        if (isAuto)
        {
            Invoke("Spin", 0.4f);
        }
    }

    private void ChangeMoney(int count)
    {
        money += count;
        PlayerPrefs.SetInt("Money", money);
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "MONEY: " + money.ToString();
    }

    public void SetAuto()
    {
        if (!isAuto)
        {
            timeInterval = timeInterval / 10;
            isAuto = true;
            btnAnim.SetBool("isAuto", true);
            Spin();
        }
        else
        {
            timeInterval = timeInterval * 10;
            isAuto = false;
            btnAnim.SetBool("isAuto", false);
        }
    }

    public void IncreaseBetAmount()
    {
        if (betAmount < 5)
        {
            betAmount++;
            UpdateBetAmountText();
        }
    }

    public void DecreaseBetAmount()
    {
        if (betAmount > 1)
        {
            betAmount--;
            UpdateBetAmountText();
        }
    }

    private void UpdateBetAmountText()
    {
        betAmountText.text = "Bet: " + betAmount.ToString();
    }
}

[System.Serializable]
public class Combinations
{
    public enum SlotValue
    {
        Crown,
        Diamond,
        Seven,
        Cherry,
        Bar
    }

    public SlotValue FirstValue;
    public SlotValue SecondValue;
    public SlotValue ThirdValue;
    public int prize;
}