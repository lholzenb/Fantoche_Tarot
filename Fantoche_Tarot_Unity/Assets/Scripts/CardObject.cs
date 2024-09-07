using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public CardType cardType;
    UnityEngine.Vector3 baseScale;
    private SpriteRenderer spriteRenderer;
    Collider2D collider;
    private GameObject mechanicsHolder;
    private bool deactivateInteractions = false;
    private Vector2 originalPosition;
    private bool isHovering = false;
    private bool isHolding = false;

    // adding a type to an object
    public void SetCardType(CardType newCardType)
    {
        cardType = newCardType;
    }

    //******************************************************************************
    // BEHAVIOUR AS A GAMEOBJECT
    void Start()
    {
        baseScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.localPosition;
        collider = GetComponent<Collider2D>();
        mechanicsHolder = GameObject.Find("Mechanics_Holder");
        spriteRenderer.sortingOrder = 99;
    }
    void OnMouseEnter()
    {
        if (deactivateInteractions)
        {
            return;
        }
        HighlightCard(true);
        isHovering = true;
    }

    void OnMouseExit()
    {
        if (deactivateInteractions)
        {
            return;
        }
        HighlightCard(false);
        isHovering = false;
    }
    void OnMouseDown()
    {
        if (deactivateInteractions)
        {
            return;
        }
        if (isHovering)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            isHolding = true;
        }
    }
    void OnMouseUp()
    {
        if (deactivateInteractions)
        {
            return;
        }
        mechanicsHolder.GetComponent<Main>().LayPrep("lay");
        spriteRenderer.color = Color.white;
        isHolding = false;
        mechanicsHolder.GetComponent<Main>().StatusUpdate("none", "card");
        transform.position = originalPosition;
        HighlightCard(false);
    }

    void Update()
    {
        if (deactivateInteractions)
        {
            return;
        }
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (isHolding)
        {
            collider.enabled = false;
            transform.position = new Vector2(objectPosition.x, objectPosition.y);
            HighlightCard(true);
            mechanicsHolder.GetComponent<Main>().StatusUpdate(gameObject.name, "card", false, true, cardType);
        }
        else 
        {
            collider.enabled = true;
        }
    }

    // Beispielmethode zum Hervorheben der Karte (z.B. durch Skalierung oder Farbe)
    void HighlightCard(bool highlight)
    {
        if (highlight)
        {
            // Zum Beispiel die Karte etwas vergrößern
            transform.localScale = new Vector3(baseScale.x + 0.2f, baseScale.y + 0.2f, baseScale.z + 0.2f);
            spriteRenderer.sortingOrder = 99;
        }
        else
        {
            transform.localScale = baseScale;
            spriteRenderer.sortingOrder -= 1;
        }
    }

    public void DeactivateInteractions()
    {
        deactivateInteractions = true;
    }
}
