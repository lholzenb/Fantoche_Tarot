using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public Main main;

    public int scoreThreshold = 0;
    private int playerScore;
    public Animator animatorDialogue;


    public Animator animatorCurtain;

    // List to hold good responses
    public List<string> goodResponses = new List<string>
    {
        "I see a lifetime of love... and shared pizza slices in your future!",
        "You’ll grow old together—like a fine wine, or a well-aged cheese!",
        "The crystal ball shows endless date nights… mostly arguing over where to eat!",
        "Your love is written in the stars—right next to 'must do the dishes together'!",
        "In your future, I see… matching outfits. Whether you like it or not!",
        "True love? Definitely! You’ll even finish each other’s… laundry!",
        "The universe predicts lots of laughter, and the occasional 'You’re right, dear.'",
        "You're destined for many romantic getaways… to IKEA!",
        "Ah, the cards tell me you're perfect for each other… especially when ordering takeout!",
        "A long, happy future awaits… full of shared Wi-Fi passwords and TV show spoilers!",
        "The stars align for you… just as long as you share the remote!",
        "You’re meant to be… just like coffee and Sunday mornings!",
        "I see decades of happiness… mostly laughing at inside jokes no one else gets!",
        "The cards say you're inseparable—like peanut butter and jelly, or socks after laundry!",
        "In the distant future, I see… many selfies and arguments over filters!",
        "I sense endless adventures… to find the perfect Instagram-worthy brunch spot!",
        "The cosmos show a love so strong… even your GPS will stop recalculating!",
        "You two are destined to complete each other… mostly in group texts and emojis!",
        "I foresee a lifetime of love, laughter, and never remembering your Netflix password!"
    };

    // List to hold bad responses
    public List<string> badResponses = new List<string>
    {
        "The cards say your relationship is like a Wi-Fi connection—unreliable and full of dead spots. Time to disconnect?",
        "The stars suggest that your love life is like a bad haircut—maybe it’s time for a trim... or a complete do-over.",
        "I see you in a future where you’re much happier… and single. Just saying.",
        "Your relationship is like a broken shopping cart—no matter how hard you push, it’s still going nowhere.",
        "The cards indicate your relationship is like an expired coupon—looks good, but it’s no longer valid.",
        "Your relationship is like leftovers—was good once, but now it’s just time to toss it out.",
        "The cards show a brighter future—possibly because there’s no one blocking your light.",
        "I see a great weight being lifted from your shoulders… and that weight has a name.",
        "The stars say it’s time to Marie Kondo your relationship. If it doesn’t spark joy, you know what to do.",
        "Your love life is like a Wi-Fi password you can’t remember—complicated and probably not worth the effort.",
        "The universe is giving you a nudge—it’s time to stop trying to fix what’s already broken and start fresh.",
        "Your relationship is like an old pair of shoes—comfortable but full of holes. Time to shop for a new pair?",
        "I see you in a future where you’re much happier… and not constantly apologizing for things you didn’t do.",
        "The stars are aligned… for a breakup. But hey, more room in your life for pizza and wine!"
    };

    public void UpdateScore()
    {
        animatorDialogue.SetBool("DialogueOpen", true);
        playerScore = main.currentOutcomePosNeg;
        DisplayDialogue();
    }

    private void DisplayDialogue()
    {
        string response;

        if (playerScore >= scoreThreshold)
        {
            response = GetRandomResponse(goodResponses);
        }
        else
        {
            response = GetRandomResponse(badResponses);
        }

        StopAllCoroutines();
        StartCoroutine(TypeSentence(response));
        //dialogueText.text = response;
    }

    private string GetRandomResponse(List<string> responses)
    {
        int randomIndex = Random.Range(0, responses.Count);
        return responses[randomIndex];
    }

    IEnumerator TypeSentence(string sentence)
    {
        float typingSpeed = 0.05f;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void CloseDialogue()
    {
        animatorDialogue.SetBool("DialogueOpen", false);
        animatorCurtain.SetBool("CurtainWillClose", true);
        Invoke("RestartScene", 1);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(1);
    }
}
