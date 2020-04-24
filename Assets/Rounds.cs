using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rounds : MonoBehaviour
{
    public MechanicsManager mechanicsManager;
    public GameObject roundPrefab;

    public Sprite completed;
    public Sprite current;
    public Sprite locked;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < mechanicsManager.rounds.Length; i++)
        {
            GameObject round = Instantiate(roundPrefab, transform);
            SetupRound(i, round);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject round = this.transform.GetChild(i).gameObject;
            SetupRound(i, round);
        }
    }

    private void SetupRound(int i, GameObject round)
    {
        round.transform.Find("RoundNumber").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
        if (i == mechanicsManager.currentRound)
        {
            round.transform.Find("RoundNumber").gameObject.SetActive(true);
            round.GetComponent<Image>().sprite = current;
            round.transform.Find("Stars").gameObject.SetActive(false);
        }
        else if (i < mechanicsManager.currentRound)
        {
            round.GetComponent<Image>().sprite = completed;
            Transform stars = round.transform.Find("Stars");
            stars.gameObject.SetActive(true);
            int starsAchieved = mechanicsManager.rounds[i].starsAchieved;
            if (starsAchieved < 2)
            {
                Color starColor = stars.GetChild(1).GetComponent<Image>().color;
                starColor.a = 0.5f;
                stars.GetChild(1).GetComponent<Image>().color = starColor;
            }
            if (starsAchieved < 3)
            {
                Color starColor = stars.GetChild(2).GetComponent<Image>().color;
                starColor.a = 0.5f;
                stars.GetChild(2).GetComponent<Image>().color = starColor;
            }
        }
        else
        {
            round.GetComponent<Image>().sprite = locked;
            round.transform.Find("RoundNumber").gameObject.SetActive(false);
            round.transform.Find("Stars").gameObject.SetActive(false);
        }
    }
}
