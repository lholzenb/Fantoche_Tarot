using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Numerics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("[Debug]")]
    public int pairNumber;
    private bool continuePairs = false;
    private bool startNextRound = false;
     private bool finalPhaseNotYetRun;
    public int currentOutcomePosNeg;
    public int finalScore;
    public int allowedAmountOfHandcards;

    [Space(20)]
    public string requestedPlacement = "none";
    public bool isThisSpotFree = false;
    public bool holdingCardRightNow = false;
    public string theCardIamHolding = "none";
    private CardType theCardTypeIamHolding;
    private bool mouseWasReleased = false;

    string backup1;
    bool backup2;
    bool backup3;
    CardType backup4;

    [Space(20)]
    public DialogueManager dialogueManager;

    [Space(20)]
    public GameObject Current_CardLeft;
    public GameObject Current_CardRight;
    public GameObject Current_CardTop;
    public GameObject Current_CardBottom;
    
    [Space(20)]
    public GameObject Current_Handcard1;
    public GameObject Current_Handcard2;
    public GameObject Current_Handcard3;
    
    [Space(20)]
    [Header("Setup & General")]
    public List<GameObject> Pairs = new List<GameObject>();
    public float TimePassedUntilHandcardsAreRevealed = 1.5f;

    [Space(20)]
    public GameObject cardPrefab_HandcardTypeBlue;
    public GameObject cardPrefab_HandcardTypeGreen;
    public GameObject cardPrefab_HandcardTypeRed;   
    public GameObject cardPrefab_HandcardTypeYellow;

    [Space(20)]
    public GameObject CardLeftHolder;
    private UnityEngine.Vector2 cardLeftPos;
    public GameObject CardRightHolder;
    private UnityEngine.Vector2 cardRightPos;
    public GameObject CardTopHolder;
    private UnityEngine.Vector2 cardTopPos;
    public GameObject CardBottomHolder;
    private UnityEngine.Vector2 cardBottomPos;

    [Space(20)]
    public GameObject HandcardHolder1;
    private UnityEngine.Vector2 handcard1Pos;
    public GameObject HandcardHolder2;
    private UnityEngine.Vector2 handcard2Pos;
    public GameObject HandcardHolder3;
    private UnityEngine.Vector2 handcard3Pos;

    [Space(20)]
    public GameObject JokerHolder;

    [Space(20)]
    [Header("Card Types")]
    private CardType red_is_fire;
    private CardType yellow_is_earth;
    private CardType green_is_air;
    private CardType blue_is_water;
    public CardType joker_card;

    [Space(20)]
    [Header("Reactions")]
    public GameObject reaction_hearts_left;
    public GameObject reaction_hearts_right;
    public GameObject reaction_hearts_top;
    public GameObject reaction_hearts_bottom;

    [Space(20)]
    public GameObject reaction_thunder_left;
    public GameObject reaction_thunder_right;
    public GameObject reaction_thunder_top;
    public GameObject reaction_thunder_bottom;

    void Start()
    {
        Pairs[1].SetActive(true);

        // game starts & checking if setup correct & loads first pair
        allowedAmountOfHandcards = 3;
        startNextRound = true;
        finalPhaseNotYetRun = true;
        JokerHolder.GetComponent<CardObject>().DeactivateInteractions(true, false);
        if (CardLeftHolder == null || CardRightHolder == null || CardTopHolder == null || CardBottomHolder == null)
        {
            Debug.LogError("CARD SPOTS NOT ASSIGNED!");
            return;
        }

        // read positions of cards in graphical layout
        cardLeftPos = CardLeftHolder.transform.position;
        cardRightPos = CardRightHolder.transform.position;
        cardTopPos = CardTopHolder.transform.position;
        cardBottomPos = CardBottomHolder.transform.position;
        handcard1Pos = HandcardHolder1.transform.position;
        handcard2Pos = HandcardHolder2.transform.position;
        handcard3Pos = HandcardHolder3.transform.position;

        // deactivate holders after initialization of positions -> No, since we use them as placement ui, but we deactivate them when a card is placed
        //CardLeftHolder.SetActive(false);
        //CardRightHolder.SetActive(false);
        //CardTopHolder.SetActive(false);
        //CardBottomHolder.SetActive(false);

        HandcardHolder1.SetActive(false);
        HandcardHolder2.SetActive(false);
        HandcardHolder3.SetActive(false);

        // current cards are set to null at start
        Current_CardLeft = null;
        Current_CardRight = null;
        Current_CardTop = null;
        Current_CardBottom = null;

        Current_Handcard1 = null;
        Current_Handcard2 = null;
        Current_Handcard3 = null;

        // defining card types - main color cards
        Debug.Log("All defined card types in-game ->");
        red_is_fire = new CardType("Red","Fire beats air.", "blue_is_water", "green_is_air");
        Debug.Log(red_is_fire.Name + ": " + red_is_fire.Description);
        yellow_is_earth = new CardType("Yellow","Earth beats water.", "green_is_air", "blue_is_water");
        Debug.Log(yellow_is_earth.Name + ": " + yellow_is_earth.Description);
        green_is_air = new CardType("Green","Air beats earth.", "red_is_fire", "yellow_is_earth");
        Debug.Log(green_is_air.Name + ": " + green_is_air.Description);
        blue_is_water = new CardType("Blue","Water beats fire.","yellow_is_earth", "red_is_fire");
        Debug.Log(blue_is_water.Name + ": " + blue_is_water.Description);

        // defining card types - other cards
        // joker_card = new CardType("Joker","Joker card resolves a truce by being laid upside down (negative) or in the correct position (positive).","none","none");
    }

    // tracking card movements
    public bool StatusUpdate(string nameOfObject, string objectType, bool hoveringAbove = false, bool cardOnCursor = false, CardType cardType = null)
    {   
        // checking if the holder registers the mouse above and if a card is held right now
        if (objectType == "holder")
        {
            if (hoveringAbove)
            {
                requestedPlacement = nameOfObject;

                 if (nameOfObject == "Card_Holder_Left")
                {
                    if (Current_CardLeft == null)
                    {
                        isThisSpotFree = true;
                    }
                    else
                    {
                        isThisSpotFree = false;
                    }
                }
                else if (nameOfObject == "Card_Holder_Right")
                {
                    if (Current_CardRight == null)
                    {
                        isThisSpotFree = true;
                    }
                    else
                    {
                        isThisSpotFree = false;
                    }
                }
                else if (nameOfObject == "Card_Holder_Top")
                {
                    if (Current_CardTop == null)
                    {
                        isThisSpotFree = true;
                    }
                    else
                    {
                        isThisSpotFree = false;
                    }
                }
                else if (nameOfObject == "Card_Holder_Bottom")
                {
                    if (Current_CardBottom == null)
                    {
                        isThisSpotFree = true;
                    }
                    else
                    {
                        isThisSpotFree = false;
                    }
                }
                else
                {
                    Debug.LogWarning("NOPE BRO, THIS AIN'T IT");
                }
            }
            else 
            {
                requestedPlacement = "none";
                isThisSpotFree = false;
            }

        }
        else if (objectType == "card")
        {
            if (cardOnCursor)
            {
                holdingCardRightNow = true;
            }
            else
            {
                holdingCardRightNow = false;
            }

            if (nameOfObject != "none")
            {
                theCardIamHolding = cardType.Name;
                theCardTypeIamHolding = cardType;
            }
            else 
            {
                theCardIamHolding = "none";
            }
        }
        return holdingCardRightNow;
    }
    public void LayPrep(string action)
    {
        var temp = "none";
        switch (requestedPlacement)
        {
            case "Card_Holder_Left":
                temp = "left";
                break;
            case "Card_Holder_Right":
                temp = "right";
                break;
            case "Card_Holder_Top":
                temp = "top";
                break;
            case "Card_Holder_Bottom":
                temp = "bottom";
                break;
            default:
                // Debug.LogError("Card placement failed: A critical error occurred.");
                break;
        }

        if (action == "prep")
        {
            if (!mouseWasReleased)
            {
                backup1 = requestedPlacement;
                backup2 = isThisSpotFree;
                backup3 = holdingCardRightNow;
                backup4 = theCardTypeIamHolding;
            }
            
        }
        if (action == "lay")
        {
            mouseWasReleased = true;
            if (backup1 != "none" && backup2 == true && backup3 == true)
            {

                LayCards(theCardTypeIamHolding, temp);

                // delete the card in hand by checking if it matches with the laid one
                if (Current_Handcard1 != null && Current_Handcard1.GetComponent<CardObject>().cardType.Name == theCardTypeIamHolding.Name)
                {
                    Destroy(Current_Handcard1);
                    Current_Handcard1 = null; // emty reference
                }
                else if (Current_Handcard2 != null && Current_Handcard2.GetComponent<CardObject>().cardType.Name == theCardTypeIamHolding.Name)
                {
                    Destroy(Current_Handcard2);
                    Current_Handcard2 = null; // emty reference
                }
                else if (Current_Handcard3 != null && Current_Handcard3.GetComponent<CardObject>().cardType.Name == theCardTypeIamHolding.Name)
                {
                    Destroy(Current_Handcard3);
                    Current_Handcard3 = null; // emty reference
                }
            }
            else if (requestedPlacement != null && isThisSpotFree == true && holdingCardRightNow == false)
            {
                
                if (Current_Handcard1 != null)
                {
                    Current_Handcard1.GetComponent<CardObject>().DeactivateInteractions(false, true);
                }
                if (Current_Handcard2 != null)
                {
                    Current_Handcard2.GetComponent<CardObject>().DeactivateInteractions(false, true);
                }
                if (Current_Handcard3 != null)
                {
                    Current_Handcard3.GetComponent<CardObject>().DeactivateInteractions(false, true);
                }
                RandomizeCard(temp, true, 1);
                Debug.Log("requested random card on: " + temp);
            }
            mouseWasReleased = false;
        }
    }
    void Update()
    {
        // 1.: We start the round by displaying or starting an animation for the according pairs.
        //DisplayPairs();
        // 2.: Making selection of hand-cards avaiable.
        DisplayHandCards();
        // 3.: LayCards will be triggered as soon as we have everything prepared.
        LayPrep("prep");
        // 4.: We calculate the outcome for the cards present on the board.
        CardRelationshipLogic();
        // 5.: We check, if the final phase may begin and trigger it.
        FinalPhase();
    }
    
    //void DisplayPairs()
    //{
    //    if (continuePairs == true)
    //    {
    //        continuePairs = false;
    //        pairNumber += 1;

    //        // this only runs once for each according pairs
    //        // you can trigger an animation and/or couroutine for displaying a pair down below (fancy slide-in perhaps?)

    //        if (Pairs.Count > 0 && pairNumber < Pairs.Count) // checking if not emty or out of range
    //        {
    //            /*
    //            // For animators & in-engine polishing:
    //            // Displaying the correct pair, arrays start at 0.
    //            -> Or do this in a coroutine and make a new array for animations, it's up to you!
    //            */
    //            Pairs[(pairNumber-1)].SetActive(true);
    //            Debug.Log("Pair activated: " + Pairs[(pairNumber-1)].name);
    //        }
    //    }
    //}
    void DisplayHandCards()
    {
        // every round we display max. 3 cards - every round we get +1 card
        if (startNextRound == true)
        {
            startNextRound = false;
            continuePairs = true;
            allowedAmountOfHandcards += 1;
        }
        else
        {
            return;
        }

        if (allowedAmountOfHandcards > 3)
        {
            allowedAmountOfHandcards = 3;

            // give a random selection of cards but no +1
            StartCoroutine(NewCards(allowedAmountOfHandcards));
        }
        else
        {
            // give a random selection of cards and add the +1 a moment later
            StartCoroutine(NewCards(allowedAmountOfHandcards-1));
            StartCoroutine(ExtraCards(1));
        }
    }
    private IEnumerator NewCards(int amount)
    {
        yield return new WaitForSeconds(TimePassedUntilHandcardsAreRevealed);
        RandomizeCard("void", false, amount);

    }
    private IEnumerator ExtraCards(int amount)
    {
        yield return new WaitForSeconds(TimePassedUntilHandcardsAreRevealed + 1f);
        RandomizeCard("void", false, amount);
    }

    // more specific card logic ---> Lay, Randomize, RelationshipLogic
    public void RandomizeCard(string requestedSpot, bool isSuposedToLay, int amount = 1)
    {
        List<CardType> possibleCards = new List<CardType>()
        {
        red_is_fire,
        yellow_is_earth,
        green_is_air,
        blue_is_water,
        };

        if (isSuposedToLay == true)
        {
            CardType randomCard = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];
            LayCards(randomCard, requestedSpot);
            backup4 = null;
            theCardTypeIamHolding = null;
        }
        else
        {
            // giving a selection for the amount specified
            List<CardType> randomSelection = new List<CardType>();
            for (int i = 0; i < amount; i++)
            {
                CardType randomCard = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];

                // remove doubles (diverse selection)
                while (randomSelection.Contains(randomCard))
                {
                    randomCard = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];
                }
                randomSelection.Add(randomCard);
            }

            // using the output
            StartCoroutine(CardInstanceDelay(randomSelection));
        }
    }
    IEnumerator CardInstanceDelay(List<CardType> randomSelection)
    {
        float baseDelay = 0f;
        foreach (CardType cardType in randomSelection)
        {
            Debug.Log("Handcard: " + cardType.Name);
            GameObject cardPrefab = SelectCardPrefab(cardType);
            yield return new WaitForSeconds(baseDelay);
            baseDelay = 0.5f;

            // handcard slot checking
            if (Current_Handcard1 == null)
            {
                Current_Handcard1 = Instantiate(cardPrefab);
                Current_Handcard1.AddComponent<CardObject>().SetCardType(cardType);
                Current_Handcard1.GetComponent<CardObject>().DeactivateInteractions(true);
                Current_Handcard1.transform.position = new UnityEngine.Vector2(handcard1Pos.x, handcard1Pos.y);
            }
            else if (Current_Handcard2 == null)
            {
                Current_Handcard2 = Instantiate(cardPrefab);
                Current_Handcard2.AddComponent<CardObject>().SetCardType(cardType);
                Current_Handcard2.GetComponent<CardObject>().DeactivateInteractions(true);
                Current_Handcard2.transform.position = new UnityEngine.Vector2(handcard2Pos.x, handcard2Pos.y);
            }
            else if (Current_Handcard3 == null)
            {
                Current_Handcard3 = Instantiate(cardPrefab);
                Current_Handcard3.AddComponent<CardObject>().SetCardType(cardType);
                Current_Handcard3.GetComponent<CardObject>().DeactivateInteractions(true);
                Current_Handcard3.transform.position = new UnityEngine.Vector2(handcard3Pos.x, handcard3Pos.y);
            }
        }
    }
    // we use an extra method to return the correct card prefab for the cardType input
    GameObject SelectCardPrefab(CardType cardType)
    {
        switch (cardType.Name)
        {
            case "Blue":
                return cardPrefab_HandcardTypeBlue;
            case "Green":
                return cardPrefab_HandcardTypeGreen;
            case "Red":
                return cardPrefab_HandcardTypeRed;
            case "Yellow":
                return cardPrefab_HandcardTypeYellow;
            default:
                Debug.LogWarning("Unknown card type: " + cardType.Name);
                return null;
        }
    }

    void LayCards(CardType cardType, string requestedSpot)
    {
        // at the beginning all spots should be open (interaction true)
        // -> set all interactibles to "active"
        GameObject cardPrefab = SelectCardPrefab(cardType);

        // assigning card type to slots
        if (Current_CardLeft == null && requestedSpot == "left")
        {
            // we assign the type of the card chosen
            Current_CardLeft = Instantiate(cardPrefab);
            Current_CardLeft.AddComponent<CardObject>().SetCardType(cardType);
            Current_CardLeft.GetComponent<CardObject>().DeactivateInteractions();
            CardLeftHolder.GetComponent<CardHolder>().DeactivateInteractions();
            Current_CardLeft.transform.position = new UnityEngine.Vector2(cardLeftPos.x, cardLeftPos.y);
            Debug.Log("Card placed on the left: " + Current_CardLeft.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardRight == null && requestedSpot == "right")
        {
            // we assign the type of the card chosen
            Current_CardRight = Instantiate(cardPrefab);
            Current_CardRight.AddComponent<CardObject>().SetCardType(cardType);
            Current_CardRight.GetComponent<CardObject>().DeactivateInteractions();
            CardRightHolder.GetComponent<CardHolder>().DeactivateInteractions();
            Current_CardRight.transform.position = new UnityEngine.Vector2(cardRightPos.x, cardRightPos.y);
            Debug.Log("Card placed on the right: " + Current_CardRight.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardTop == null && requestedSpot == "top")
        {
            // we assign the type of the card chosen
            Current_CardTop = Instantiate(cardPrefab);
            Current_CardTop.AddComponent<CardObject>().SetCardType(cardType);
            Current_CardTop.GetComponent<CardObject>().DeactivateInteractions();
            CardTopHolder.GetComponent<CardHolder>().DeactivateInteractions();
            Current_CardTop.transform.position = new UnityEngine.Vector2(cardTopPos.x, cardTopPos.y);
            Debug.Log("Card placed on the top: " + Current_CardTop.GetComponent<CardObject>().cardType.Name);
        }
        else if (Current_CardBottom == null && requestedSpot == "bottom")
        {
            // we assign the type of the card chosen
            Current_CardBottom = Instantiate(cardPrefab);
            Current_CardBottom.AddComponent<CardObject>().SetCardType(cardType);
            Current_CardBottom.GetComponent<CardObject>().DeactivateInteractions();
            CardBottomHolder.GetComponent<CardHolder>().DeactivateInteractions();
            Current_CardBottom.transform.position = new UnityEngine.Vector2(cardBottomPos.x, cardBottomPos.y);
            Debug.Log("Card placed on the bottom: " + Current_CardBottom.GetComponent<CardObject>().cardType.Name);
        }
        else
        {
            // safe is safe - even tho we control it over deactivating interactibles
            Debug.LogWarning("This slot is already assigned! How did you even trigger this interaction?");
        }
    }

    void CardRelationshipLogic()
    {
        // here we determine the outcome for each new card presented - basically changing value when something changes
        // establishing value during this operation which resets every time it has gone through
        var currentOutcomeTemp = 0;

        if (Current_CardLeft != null)
        {
            if (CardMathematicalSummary("left") >= 1)
            {
                Debug.Log("hearts");
                currentOutcomeTemp += 1;
                reaction_hearts_left.SetActive(true);
                reaction_thunder_left.SetActive(false);
            }
            else if (CardMathematicalSummary("left") <= -1)
            {
                currentOutcomeTemp -= 1;
                reaction_hearts_left.SetActive(false);
                reaction_thunder_left.SetActive(true);
            }
            else 
            {
                // needing some sort of visual representation of neutral value
                reaction_hearts_left.SetActive(false);
                reaction_thunder_left.SetActive(false);
            }
        }
        if (Current_CardRight != null)
        {
            if (CardMathematicalSummary("right") >= 1)
            {
                Debug.Log("hearts");
                currentOutcomeTemp += 1;
                reaction_hearts_right.SetActive(true);
                reaction_thunder_right.SetActive(false);
            }
            else if (CardMathematicalSummary("right") <= -1)
            {
                currentOutcomeTemp -= 1;
                reaction_hearts_right.SetActive(false);
                reaction_thunder_right.SetActive(true);
            }
            else 
            {
                // needing some sort of visual representation of neutral value
                reaction_hearts_right.SetActive(false);
                reaction_thunder_right.SetActive(false);
            }
        }
        if (Current_CardTop != null)
        {
            if (CardMathematicalSummary("top") >= 1)
            {
                Debug.Log("hearts");
                currentOutcomeTemp += 1;
                reaction_hearts_top.SetActive(true);
                reaction_thunder_top.SetActive(false);
            }
            else if (CardMathematicalSummary("top") <= -1)
            {
                currentOutcomeTemp -= 1;
                reaction_hearts_top.SetActive(false);
                reaction_thunder_top.SetActive(true);
            }
            else 
            {
                // needing some sort of visual representation of neutral value
                reaction_hearts_top.SetActive(false);
                reaction_thunder_top.SetActive(false);
            }
        }
        if (Current_CardBottom != null)
        {
            if (CardMathematicalSummary("bottom") >= 1)
            {
                Debug.Log("hearts");
                currentOutcomeTemp += 1;
                reaction_hearts_bottom.SetActive(true);
                reaction_thunder_bottom.SetActive(false);
            }
            else if (CardMathematicalSummary("bottom") <= -1)
            {
                currentOutcomeTemp -= 1;
                reaction_hearts_bottom.SetActive(false);
                reaction_thunder_bottom.SetActive(true);
            }
            else 
            {
                // needing some sort of visual representation of neutral value
                reaction_hearts_bottom.SetActive(false);
                reaction_thunder_bottom.SetActive(false);
            }
        }      
        currentOutcomePosNeg = currentOutcomeTemp;
    }
    int CardMathematicalSummary(string position)
    {
        if (position == "left")
        {   
            var temp = 0;
            // top, bottom and right influence
            temp += CardCompareWith(Current_CardTop, Current_CardLeft);
            temp += CardCompareWith(Current_CardBottom, Current_CardLeft);
            temp += CardCompareWith(Current_CardRight, Current_CardLeft);
            Debug.Log("CardMathematicalSummary (left): " + temp);
            return temp;
        }
        else if (position == "right")
        {   
            var temp = 0;
            // top, bottom and left influence this
            temp += CardCompareWith(Current_CardTop, Current_CardRight);
            temp += CardCompareWith(Current_CardBottom, Current_CardRight);
            temp += CardCompareWith(Current_CardLeft, Current_CardRight);
             Debug.Log("CardMathematicalSummary (right): " + temp);
            return temp;
        }
        else if (position == "top")
        {
            var temp = 0;
            // bottom, left and right influence this
            temp += CardCompareWith(Current_CardBottom, Current_CardTop);
            temp += CardCompareWith(Current_CardLeft, Current_CardTop);
            temp += CardCompareWith(Current_CardRight, Current_CardTop);
            Debug.Log("CardMathematicalSummary (top): " + temp);
            return temp;
        }
        else if (position == "bottom")
        {
            var temp = 0;
            // top, left and right influence this
            temp += CardCompareWith(Current_CardTop, Current_CardBottom);
            temp += CardCompareWith(Current_CardLeft, Current_CardBottom);
            temp += CardCompareWith(Current_CardRight, Current_CardBottom);
            Debug.Log("CardMathematicalSummary (bottom): " + temp);
            return temp;
        }
        else 
        {
            Debug.LogError("Something in MATHEMATICAL SUMMARY went horribly wrong!!!");
            return 0;
        }
    }
    int CardCompareWith(GameObject ToCompareWith, GameObject myCard)
    {
        var myCardObject = myCard.GetComponent<CardObject>();
        string myWeakness = myCardObject.cardType.WeaknessAgainst;
        string myStrenght = myCardObject.cardType.StrenghtAgainst;

        Debug.Log(ToCompareWith + "&");
        Debug.Log(myCardObject + " " + myWeakness + " " + myStrenght + " &");
        if (ToCompareWith == null)
        {
            return 0;
        }
        
        Debug.Log("***** This card: " + myCardObject + " is weak against " + myWeakness + " and strong against " + myStrenght + "\n" +
                  "***** Other card: " + ToCompareWith + " is weak against " + ToCompareWith.GetComponent<CardObject>().cardType.WeaknessAgainst + " and strong against " + ToCompareWith.GetComponent<CardObject>().cardType.StrenghtAgainst + "\n" +
                  "***** Result: " + "This card is strong against other card: " + (myStrenght == ToCompareWith.GetComponent<CardObject>().cardType.WeaknessAgainst) + "\n" +
                  "***** Result: " + "This card is weak against other card: " + (myWeakness == ToCompareWith.GetComponent<CardObject>().cardType.StrenghtAgainst) + "\n");

        if (myStrenght == ToCompareWith.GetComponent<CardObject>().cardType.Type)
        {
            Debug.Log("Positive Return " + myCardObject + " strenght: " + myStrenght + " weakness: " + myWeakness);
            return 1;
        }
        else if (myWeakness == ToCompareWith.GetComponent<CardObject>().cardType.Type)
        {
            Debug.Log("Negative Return " + myCardObject + " strenght: " + myStrenght + " weakness: " + myWeakness);
            return -1;
        }
        else
        {
            return 0;
        }
    }

    void FinalPhase()
    {
        if 
        (
        Current_CardLeft != null &&
        Current_CardRight != null &&
        Current_CardTop != null &&
        Current_CardBottom != null
        )
        {
            // making sure this does not run more than once per couple
            if (finalPhaseNotYetRun)
            {
                finalPhaseNotYetRun = false;
            }
            else
            {
                return;
            }

            // deactivate further interactions with the game (except for joker if neutral outcome)
            if (Current_Handcard1 != null)
            {
                Current_Handcard1.GetComponent<CardObject>().DeactivateInteractions(false, false);
            }
            if (Current_Handcard2 != null)
            {
                Current_Handcard2.GetComponent<CardObject>().DeactivateInteractions(false, false);
            }
            if (Current_Handcard3 != null)
            {
                Current_Handcard3.GetComponent<CardObject>().DeactivateInteractions(false, false);
            }
        }
        else
        {
            return;
        }

        // joker time for neutral results -> WE NEED A ILLUSTRATION!
        if (currentOutcomePosNeg == 0)
        {
            Debug.Log("time for joker");
            JokerHolder.GetComponent<CardObject>().DeactivateInteractions(false, true);
        }
        
        // for presentation!! so that we can click the joker
        StartCoroutine(ExecuteAfterDelay(3.0f));
        
        // coroutine to start animations and finish the round & prepare the next one
            // startNextRound = true; NOT YET
            // show a banner for the results?
            // nullify everything that has to be empty for the next round!
            // disable interaction locks
            // close curtains and reopen?

            // wieder deaktivieren?
            // JokerHolder.GetComponent<CardObject>().DeactivateInteractions(true, false);
    }
    IEnumerator ExecuteAfterDelay(float delayInSeconds)
    {
        Debug.Log("Coroutine started. Waiting for " + delayInSeconds + " seconds...");
        
        // Wartet die angegebene Zeit
        yield return new WaitForSeconds(delayInSeconds);
        
        // Code, der nach dem Delay ausgeführt wird
        Debug.Log("3 seconds passed, now executing the rest of the code.");
        
        // Füge hier deinen Code hinzu, der nach der Verzögerung ausgeführt werden soll
        finalScore = currentOutcomePosNeg;
        dialogueManager.UpdateScore();

        // Update Couples Sprite
        if (currentOutcomePosNeg >= 0)
        {
            Pairs[1].SetActive(false);
            Pairs[2].SetActive(true);
        }
        else
        {
            Pairs[1].SetActive(false);
            Pairs[0].SetActive(true);
        }

    }
}
    
        
// second class to easify card types
public class CardType
{
    public string Name {get; set;}
    public string Description {get; set;}
    public string WeaknessAgainst {get; set;}
    public string StrenghtAgainst {get; set;}
    public string Type {get; set;}

    // setting values and getting values (constructor)
    public CardType(string name, string description, string weaknessAgainst, string strenghAgainst)
    {
        Name = name;
        Description = description;
        WeaknessAgainst = weaknessAgainst;
        StrenghtAgainst = strenghAgainst;
        switch (name)
        {
            case "Blue":
                Type = "blue_is_water";
                break;
            case "Green":
                 Type = "green_is_air";
                break;
            case "Red":
                Type = "red_is_fire";
                break;
            case "Yellow":
                Type = "yellow_is_earth";
                break;
        }
    }
}
