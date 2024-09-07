using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public CardType cardType;
    UnityEngine.Vector3 baseScale;
    private SpriteRenderer spriteRenderer;

    private Vector2 originalPosition;
    private bool isHovering = false;
    private bool isMoving = false;

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
    }
    void OnMouseEnter()
    {
        HighlightCard(true);
        isHovering = true;
    }

    void OnMouseExit()
    {
        HighlightCard(false);
        isHovering = false;
    }
    void OnMouseDown()
    {
        if (isHovering)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            isMoving = true;
        }
        else
        {
        }
    }
    void OnMouseUp()
    {
        spriteRenderer.color = Color.white;
        isMoving = false;
        transform.position = originalPosition;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        if (isMoving)
        {
            transform.position = new Vector2(objectPosition.x, objectPosition.y);
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
}
