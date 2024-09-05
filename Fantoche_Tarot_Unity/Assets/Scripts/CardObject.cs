using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public CardType cardType;

    // adding a type to an object
    public void SetCardType(CardType newCardType)
    {
        cardType = newCardType;
        // Debug.Log("Assigned card type: " + cardType.Name);
    }
}
