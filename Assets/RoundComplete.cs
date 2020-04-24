using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundComplete : MonoBehaviour
{

    public MechanicsManager mechanicsManager;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    public GameObject nextRound;
    public GameObject replayRound;

    public TextMeshProUGUI title;
    public TextMeshProUGUI money;
    public TextMeshProUGUI stars;
   
    private float updateSpeedMoney = 0.001f;
    private float updateSpeedStars = 0.5f;

    public AudioSource soundEffects;
    public AudioClip star1Audio;
    public AudioClip star2Audio;
    public AudioClip star3Audio;

    private void OnEnable()
    {
        nextRound.SetActive(false);
        replayRound.SetActive(false);
        if (mechanicsManager.starsCollected > 0)
        {
            title.text = "Round " + (mechanicsManager.currentRound + 1) + " Complete";
            nextRound.SetActive(true);
        }
        else
        {
            title.text = "Round " + (mechanicsManager.currentRound + 1) + " Failed";
            replayRound.SetActive(true);
        }

        money.text = mechanicsManager.money.ToString();
        stars.text = mechanicsManager.stars.ToString();

        StartCoroutine(MoneyUpdater());
        StartCoroutine(StarsUpdater());
        StartCoroutine(ShowStars(mechanicsManager.starsCollected));
    }


    private IEnumerator ShowStars(int stars)
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        if (mechanicsManager.starsCollected >= 1)
        {
            star1.SetActive(true);
            soundEffects.clip = star1Audio;
            soundEffects.Play();
            yield return new WaitForSeconds(soundEffects.clip.length - 2f);
        }
        
        if (mechanicsManager.starsCollected >= 2)
        {
            star2.SetActive(true);
            soundEffects.clip = star2Audio;
            soundEffects.Play();
            yield return new WaitForSeconds(soundEffects.clip.length - 2f);
        }
       
        if(mechanicsManager.starsCollected >= 3)
        {
            star3.SetActive(true);
            soundEffects.clip = star3Audio;
            soundEffects.Play();
        }
    }


    private IEnumerator MoneyUpdater()
    {
        while (true)
        {
            if (int.Parse(money.text) < mechanicsManager.money)
            {
                if (mechanicsManager.money - int.Parse(money.text) > 1000)
                {
                    money.text = (int.Parse(money.text) + 100).ToString();
                }
                else if (mechanicsManager.money - int.Parse(money.text) > 100)
                {
                    money.text = (int.Parse(money.text) + 10).ToString();
                }
                else
                {
                    money.text = (int.Parse(money.text) + 1).ToString();
                }
            }
            yield return new WaitForSeconds(updateSpeedMoney); // I used .2 secs but you can update it as fast as you want
        }
    }


    private IEnumerator StarsUpdater()
    {
        while (true)
        {
            if (int.Parse(stars.text) < mechanicsManager.stars)
            {
                stars.text = (int.Parse(stars.text) + 1).ToString();
            }
            yield return new WaitForSeconds(updateSpeedStars); // I used .2 secs but you can update it as fast as you want
        }
    }
}
