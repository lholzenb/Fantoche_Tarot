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
    private bool now = false;

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
            spriteRenderer.sortingOrder = 99;
            return;
        }
        HighlightCard(true);
        isHovering = true;
    }

    void OnMouseExit()
    {
        if (deactivateInteractions)
        {
            spriteRenderer.sortingOrder = 99;
            return;
        }
        HighlightCard(false);
        isHovering = false;
    }
    void OnMouseDown()
    {
        if (deactivateInteractions)
        {
            spriteRenderer.sortingOrder = 99;
            return;
        }
        if (isHovering)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            isHolding = true;
        }
        if (cardType.Name == "Joker")
        {
            now = true;
        }
    }
    void OnMouseUp()
    {
        // this is game-breaking if not placed here!
        mechanicsHolder.GetComponent<Main>().LayPrep("lay");
        isHolding = false;
        mechanicsHolder.GetComponent<Main>().StatusUpdate("none", "card");

        if (deactivateInteractions)
        {   
            spriteRenderer.sortingOrder = 99;
            return;
        }

        spriteRenderer.color = Color.white;
        transform.position = originalPosition;
        HighlightCard(false);
    }

    public bool JokerDragged()
    {
        if (now == true)
        {
            return true;
        }
        else 
        {
            return false;
        }
        
    }

    void Update()
    {
        if (mechanicsHolder.GetComponent<Main>().PauseMenuObject.activeSelf == true)
        {
            return;
        }
        if (deactivateInteractions)
        {
            spriteRenderer.sortingOrder = 99;
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

    void HighlightCard(bool highlight)
    {
        if (highlight)
        {
            transform.localScale = new Vector3(baseScale.x + 0.2f, baseScale.y + 0.2f, baseScale.z + 0.2f);
            spriteRenderer.sortingOrder = 99;
        }
        else
        {
            transform.localScale = baseScale;
            spriteRenderer.sortingOrder -= 1;
        }
    }
    public void DeactivateInteractions(bool yes = false, bool activate = false)
    {
        if (activate)
        {
            deactivateInteractions = false;
            StartCoroutine(waitForRenderer("white"));
            return;
        }
        deactivateInteractions = true;
        if (yes)
        {
            StartCoroutine(waitForRenderer("grey"));
        }
    }
    IEnumerator waitForRenderer(string color)
    {
        float duration = 0.33f;
        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.25f);

        Color startColor = spriteRenderer.color;
        Color targetColor;
        if (color == "grey")
        {
            targetColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
        else
        {
            targetColor = new Color(1f, 1f, 1f, 1f);
        }
        
        yield return new WaitForSeconds(0.05f);

        while (elapsedTime < duration)
        {

            spriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = targetColor;
    }
}
