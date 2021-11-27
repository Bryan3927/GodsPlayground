using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{

    public GameObject UI;
    public Environment environment;
    public Timer timer;
    public TraitHandler traitHandler;
    public Text totalRounds;
    public Text currentRound;

    private List<GameObject> cards = new List<GameObject>();

    float gameStartTime;
    float waitTime = 50.0f;
    float lastSimSpeed;
    int currentRoundCounter = 1;
    int totalRoundsCounter = 5;

    private int activeTimeStep = 0;
    private int numTraitsPerRound = 3;
    private Species animalTurn = Species.Rabbit; //default Rabbit

    // Start is called before the first frame update
    void Start()
    {
        cards.Add(UI.transform.GetChild(0).gameObject);
        cards.Add(UI.transform.GetChild(1).gameObject);
        cards.Add(UI.transform.GetChild(2).gameObject);

        traitHandler = new GameObject().AddComponent<TraitHandler>();

        UI.SetActive(false);
        gameStartTime = Time.time;
        timer.SetTimer(waitTime);
        currentRound.text = "" + currentRoundCounter + "/";
        totalRounds.text = "" + totalRoundsCounter;
    }

    // Update is called once per frame
    void Update()
    {
        timer.DisplayTime();
        if (Time.time - gameStartTime > waitTime && !UI.activeInHierarchy)
        {
            activeTimeStep++;
            lastSimSpeed = Environment.GetSimSpeed();
            Environment.SetSimSpeed(0);

            //Debug.Log("Num of Bunnies: " + Environment.allEntities[Species.Rabbit].Count + " and Num of Foxes: " + Environment.allEntities[Species.Fox].Count);

            List<Trait> nextTraits = DecideNextTraits();
            FormatUI(nextTraits);

            Text animalText = UI.transform.GetChild(3).GetComponent<Text>();
            if (animalTurn == Species.Rabbit) {
                animalText.text = "Bunnies";
            } else if (animalTurn == Species.Fox) {
                animalText.text = "Foxes";
            } 
            UI.SetActive(true);

            Species mutateTurn;

            //switching animals
            if (animalTurn == Species.Rabbit)
            {
                mutateTurn = Species.Fox;
            }
            else
            {
                mutateTurn = Species.Rabbit;
            }


            //mutations (here because its slow)
            foreach (LivingEntity livingEntity in Environment.allEntities[mutateTurn])
            {
                Animal animal = ((Animal)livingEntity);
                foreach (Trait t in animal.traits)
                {
                    t.Mutate();
                }
            }

        }
    }

    public void StartNextRound()
    {
        // Debug.Log("Starting next round. Previous sim speed: " + lastSimSpeed);

        //switching animals
        if (animalTurn == Species.Rabbit)
        {
            animalTurn = Species.Fox;
        }
        else
        {
            animalTurn = Species.Rabbit;
        }


        foreach (GameObject card in cards)
        {
            Button chooseButton = card.transform.GetChild(1).GetComponentInChildren<Button>();
            chooseButton.onClick.RemoveAllListeners();
        }
        UI.SetActive(false);
        Environment.SetSimSpeed(lastSimSpeed);
        gameStartTime = Time.time;
        timer.SetTimer(waitTime);
        timer.StartTimer();
        currentRoundCounter += 1;
        currentRound.text = "" + currentRoundCounter + "/";
    }

    private List<Trait> DecideNextTraits()
    {
        // TODO: Logic to select traits based on the round
        // Transform card1 = UI.transform.GetChild(0);
        // Transform card2 = UI.transform.GetChild(1);
        // Transform card3 = UI.transform.GetChild(2);


        return traitHandler.GetRandomTraits(animalTurn, numTraitsPerRound);
        // ShortenGestationPeriod sgp = card1.gameObject.AddComponent<ShortenGestationPeriod>() as ShortenGestationPeriod;
        // SpeedBoost2 sp2 = card2.gameObject.AddComponent<SpeedBoost2>() as SpeedBoost2;
        // ShortenMateTime smt = card3.gameObject.AddComponent<ShortenMateTime>() as ShortenMateTime;

        // return new List<Trait>() { sgp, sp2, smt };
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
            Text traitName = cards[i].transform.GetChild(0).GetComponentInChildren<Text>(); //test ui obj
            Text traitDescription = cards[i].transform.GetChild(2).GetComponentInChildren<Text>();
            traitName.text = trait.Name;
            traitDescription.text = trait.Description;

            Button chooseButton = cards[i].transform.GetChild(1).GetComponent<Button>();
            chooseButton.onClick.AddListener(delegate { environment.Upgrade(animalTurn, trait); });
            chooseButton.onClick.AddListener(delegate { this.StartNextRound(); });
        }
    } 
}
