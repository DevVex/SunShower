using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Round
{
    [SerializeField]
    public float spawnFrequency;
    [SerializeField]
    public GameObject plant;
    [SerializeField]
    public GameObject[] dropOrder;
    [SerializeField]
    public int starsAchieved;
}

public class MechanicsManager : MonoBehaviour
{

    public Round[] rounds;

    public LightColorController lightColorController;
    private float localLightColor;

    public GameObject sliderUi;
    public Slider slider;

    private float timeToDrop = 0f;
    private bool roundStarted = false;
    private bool roundOver = false;

    private int sunCollected;
    private int waterCollected;
    private int waterSunBalance;
    public int starsCollected;
    private int moneyCollected;
    private int totalSuns;
    private float sunLightValue;

    public int currentRound = 0;
    private int currentRoundDrop = 0;
    private int moneyPerSun = 100;

    public int money;
    public int stars;
    public int deaths = 0;

    public GameObject roundsUi;
    public GameObject roundCompleteUi;
    public GameObject upgradesUi;
    public GameObject finalScreenUi;

    public Upgrades upgrades;

    public AudioSource soundEffects;

    public AudioClip getWater;
    public AudioClip getSun;
    public AudioClip lostRound;
    public AudioClip transition;

    public ParticleSystem sweat;

    public GameObject plantPosition;
    private GameObject plant;
    private BoxCollider2D plantCollider;

    

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        GoToRoundsUi();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentRound < rounds.Length)
        {
            if (currentRoundDrop >= rounds[currentRound].dropOrder.Length && roundStarted && !roundOver)
            {
                StartCoroutine(RoundOver(4f));
            }

            waterSunBalance = sunCollected - waterCollected;
            slider.value = Mathf.Lerp(slider.value, 0.5f + (waterSunBalance * 0.1667f), Time.deltaTime * 2);

            if (!roundOver && roundStarted)
            {
                timeToDrop += Time.deltaTime;
                if (timeToDrop > rounds[currentRound].spawnFrequency)
                {
                    SpawnGameObject(rounds[currentRound].dropOrder[currentRoundDrop]);
                    currentRoundDrop++;
                    timeToDrop = 0f;
                }

                if (waterSunBalance >= 3 || waterSunBalance <= -3)
                {
                    StartCoroutine(RoundLost(0.1f));
                }
            }


            if (waterSunBalance == 2 || waterSunBalance == -2)
            {
                sweat.Play();
            }
            else
            {
                sweat.Pause();
                sweat.Clear();
            }

            lightColorController.time = Mathf.Lerp(lightColorController.time, localLightColor, Time.deltaTime * 2);
        }
        
    }

    public IEnumerator RoundLost(float waitTime)
    {
        Debug.Log("Round Lost");
        roundOver = true;
        yield return new WaitForSeconds(waitTime);
        sliderUi.SetActive(false);
        StartCoroutine(WaitAndShowRoundLost(1f));
    }

    private IEnumerator RoundOver(float waitTime)
    {
        Debug.Log("Round Over");
        roundOver = true;
        yield return new WaitForSeconds(waitTime);
        sliderUi.SetActive(false);
        StartCoroutine(WaitAndShowRoundOver(4f));
    }

    public void GotSun()
    {
        sunCollected += 1;
        soundEffects.clip = getSun;
        soundEffects.Play();

        if (sunCollected >= totalSuns - 6)
        {
            starsCollected = 1;
        }
        if (sunCollected >= totalSuns - 3)
        {
            starsCollected = 2;
        }
        if (sunCollected >= totalSuns)
        {
            starsCollected = 3;
        }
    }

    public void GotWater()
    {
        waterCollected += 1;
        soundEffects.clip = getWater;
        soundEffects.Play();
    }

    public void StartRound()
    {
        roundOver = false;
        roundStarted = true;
        roundsUi.SetActive(false);
        roundCompleteUi.SetActive(false);
        upgradesUi.SetActive(false);
        sliderUi.SetActive(true);
        currentRoundDrop = 0;
        sunCollected = 0;
        waterCollected = 0;
        totalSuns = GetSunsForRound();
        sunLightValue = 0.75f / totalSuns;
        localLightColor = 0f;

        foreach (Transform child in plantPosition.transform)
        {
            Destroy(child.gameObject);
        }

        if (rounds[currentRound].plant)
        {
            plant = Instantiate(rounds[currentRound].plant, plantPosition.transform);
            plantCollider = plant.GetComponentsInChildren<BoxCollider2D>()[0];
            plantCollider.size = new Vector2(plantCollider.size.x + (upgrades.balanceLevel / 4f), plantCollider.size.y);
        }
    }

    public void GoToRoundsUi()
    {
        soundEffects.clip = transition;
        soundEffects.Play();
        roundsUi.SetActive(true);
        roundCompleteUi.SetActive(false);
        upgradesUi.SetActive(false);
    }

    public void CompletedRound()
    {
        currentRound++;
        GoToRoundsUi();
    }

    public void FailedRound()
    {
        money -= 100;
        deaths++;
        GoToRoundsUi();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToRoundCompleteUi()
    {
        roundsUi.SetActive(false);
        roundCompleteUi.SetActive(true);
        upgradesUi.SetActive(false);
    }

    public void GoToUpgradesUi()
    {
        if (currentRound == 0 && deaths == 0)
        {
            StartRound();
            return;
        }
        else if (currentRound == 8)
        {
            GoToFinalScreen();
            return;
        }
        soundEffects.clip = transition;
        soundEffects.Play();
        roundsUi.SetActive(false);
        roundCompleteUi.SetActive(false);
        upgradesUi.SetActive(true);
    }

    public void GoToFinalScreen()
    {
        roundsUi.SetActive(false);
        roundCompleteUi.SetActive(false);
        upgradesUi.SetActive(false);
        finalScreenUi.SetActive(true);
    }

    private int GetSunsForRound()
    {
        int suns = 0;
        for(int i = 0; i < rounds[currentRound].dropOrder.Length; i++)
        {
            if (rounds[currentRound].dropOrder[i].name == "Sun")
            {
                suns++;
            }
        }
        return suns;
    }

    private IEnumerator WaitAndShowRoundLost(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        moneyCollected = sunCollected * moneyPerSun;
        starsCollected = 0;
        GoToRoundCompleteUi();
        soundEffects.clip = lostRound;
        soundEffects.Play();
        money += moneyCollected;
    }

    private IEnumerator WaitAndShowRoundOver(float waitTime)
    {
        localLightColor = 1f;
        yield return new WaitForSeconds(waitTime);
        moneyCollected = sunCollected * moneyPerSun;
        rounds[currentRound].starsAchieved = starsCollected;
        GoToRoundCompleteUi();
        stars += starsCollected;
        money += moneyCollected;
    }


    private void SpawnGameObject(GameObject gameObject)
    {
        Vector3 randomLocation = GetRandomLocation();
        Instantiate(gameObject, randomLocation, Quaternion.identity);
        if(gameObject.name == "Sun")
        {
            localLightColor += sunLightValue;
        }
    }

    private Vector3 GetRandomLocation()
    {
        Vector3 screenDataStart = Camera.main.ScreenToWorldPoint(new Vector3(20f, Screen.height, 0));
        Vector3 screenDataEnd = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20f, Screen.height, 0));
        float xPosition = Random.Range(screenDataStart.x, screenDataEnd.x);
        return new Vector3(xPosition, screenDataStart.y + 1f, 0f);
    }
}