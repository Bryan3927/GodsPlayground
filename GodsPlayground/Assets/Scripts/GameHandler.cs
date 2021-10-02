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
    float waitTime = 10.0f;
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
        return new List<Trait>() { new ShortenGestationPeriod(), new SpeedBoost1(), new ShortenMateTime() };
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
            chooseButton.onClick.AddListener(delegate { environment.UpgradeBunnies(trait); });
        }
    } 
}
