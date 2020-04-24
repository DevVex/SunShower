using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{

    public MechanicsManager mechanicsManager;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI plantsKilledText;

    public GameObject potPosition;
    public GameObject finalPlant;

    // Start is called before the first frame update
    private void OnEnable()
    {
        int finalScore = mechanicsManager.money + (mechanicsManager.stars * 1000);
        finalScoreText.text = finalScore.ToString();
        plantsKilledText.text = mechanicsManager.deaths.ToString();

        foreach (Transform child in potPosition.transform)
        {
            Destroy(child.gameObject);
        }

        finalPlant.SetActive(true);
    }
}
