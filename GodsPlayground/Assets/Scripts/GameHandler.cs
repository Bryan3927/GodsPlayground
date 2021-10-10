using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{

    public GameObject UI;
    public Environment environment;

    private List<GameObject> cards = new List<GameObject>();

    float gameStartTime;
    float waitTime = 50.0f;
    float lastSimSpeed;

    private int activeTimeStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(UI.transform.GetChild(0).gameObject);
        cards.Add(UI.transform.GetChild(1).gameObject);
        cards.Add(UI.transform.GetChild(2).gameObject);

        UI.SetActive(false);
        gameStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - gameStartTime > waitTime && !UI.activeInHierarchy)
        {
            activeTimeStep++;
            lastSimSpeed = Environment.GetSimSpeed();
            Environment.SetSimSpeed(0);

            List<Trait> nextTraits = DecideNextTraits();
            FormatUI(nextTraits);

            UI.SetActive(true);
        }
    }

    public void StartNextRound()
    {
        Debug.Log("Starting next round. Previous sim speed: " + lastSimSpeed);
        UI.SetActive(false);
        Environment.SetSimSpeed(lastSimSpeed);
        gameStartTime = Time.time;
    }

    private List<Trait> DecideNextTraits()
    {
        // TODO: Logic to select traits based on the round
        Transform card1 = UI.transform.GetChild(0);
        Transform card2 = UI.transform.GetChild(1);
        Transform card3 = UI.transform.GetChild(2);

        ShortenGestationPeriod sgp = card1.gameObject.AddComponent<ShortenGestationPeriod>() as ShortenGestationPeriod;
        SpeedBoost1 sp1 = card2.gameObject.AddComponent<SpeedBoost1>() as SpeedBoost1;
        ShortenMateTime smt = card3.gameObject.AddComponent<ShortenMateTime>() as ShortenMateTime;

        return new List<Trait>() { sgp, sp1, smt };
    }

    private void FormatUI(List<Trait> roundTraits)
    {
        if (roundTraits.Count != 3)
        {
            throw new System.ArgumentException("Expected exactly three traits");
        }

        // Formats each card of the UI
        for (int i = 0; i < roundTraits.Count; i++)
        {
            Trait trait = roundTraits[i];
            Text traitName = cards[i].transform.GetChild(0).GetComponentInChildren<Text>();
            traitName.text = trait.Name;

            Button chooseButton = cards[i].transform.GetChild(1).GetComponent<Button>();
            // TODO: Currently we're only modifying bunnies. This needs to change to upgrade each species alternating between
            chooseButton.onClick.AddListener(delegate { environment.UpgradeBunnies(trait); });
        }
    } 
}
