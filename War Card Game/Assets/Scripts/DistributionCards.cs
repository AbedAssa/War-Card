using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// DistributionCards class is resposible of distrubation the cards for the players
/// And display cards and card numbers on the screen.
/// </summary>
public class DistributionCards : MonoBehaviour
{
    public TextMeshProUGUI cardNumberTextSystem;  //UI text displays the number of cards System have.
    public TextMeshProUGUI cardNumberTextUser;    //UI text displays the number of cards User have.

    public GameObject cardHolderForSystem;       //The object that hold the card image for System.  
    public GameObject cardHolderForUser;         //The object that hold the card image for User.
    public GameObject cardHolderForDeck;         //The object that hold the card image for cards deck.
    public GameObject cardHolderForAnimation;    //The object that hold the card is moving on screen.

    private int numOfCardSystem = 0;             //Number of cards that has beeen distributed for system.
    private int numOfCardUser = 0;               //Number of cards that has beeen distributed for system.
    private Animator distributeAnima;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.OnCardDistributedForSystem += CardAddedToSystem;
        CustomEvents.OnCardDistributedForUser += CardAddedToUser;
        cardNumberTextUser.SetText("Cards: 0");
        cardNumberTextSystem.SetText("Cards: 0");
        this.distributeAnima = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        CustomEvents.OnExitSecondFlipUserState += UpdateTextNumberAfteWinCard;
    }

    //Get called each time a card is distributed to System
    private void CardAddedToSystem(object obj)
    {
        PlayAudioSource();
        EnableCardHolderUserOrSystem(cardHolderForSystem, numOfCardSystem);
        UpdateTextCardNumber(cardNumberTextSystem, ref numOfCardSystem);
    }

    //Get called each time a card is distributed to User
    private void CardAddedToUser(object obj)
    {
        EnableCardHolderUserOrSystem(cardHolderForUser, numOfCardUser);
        UpdateTextCardNumber(cardNumberTextUser, ref numOfCardUser);
    }


    //Display number of cards in the text UI (Distribution stage).
    private void UpdateTextCardNumber(TextMeshProUGUI text, ref int currentNum)
    {
        currentNum += 1;
        text.SetText("Cards: " + currentNum);
        DisableCardsDeckHolderAndStopAnim();
    }

    //Udating number of cards after player winning a card.
    private void UpdateTextNumberAfteWinCard(object sender)
    {
        StartCoroutine("TextPulsAffect", cardNumberTextSystem);
        StartCoroutine("TextPulsAffect", cardNumberTextUser);

        //cardNumberTextSystem.SetText("Cards: " + Game.instance.systemCardsList.Count);
    }


    //Disable object that holds the deck image after distibution is finished
    private void DisableCardsDeckHolderAndStopAnim()
    {
        if(numOfCardUser == 26)
        {
            cardHolderForDeck.SetActive(false);
            cardHolderForAnimation.SetActive(false);
            Game.instance.playButtonCard.interactable = true;
            Destroy(distributeAnima);


        }
    }


    //Show card holder on the screen after first card is distributed.
    private void EnableCardHolderUserOrSystem(GameObject holder, int numCards)
    {
        if(numCards == 0)
        {
            holder.SetActive(true);
        }
    }

    //Play audio.
    private void PlayAudioSource()
    {
        if (!this.audioSource.isPlaying)
        {
            this.audioSource.Play();
        }
    }
    

    //Making Pauls affect for changin card number for player
    private IEnumerator TextPulsAffect(TextMeshProUGUI obj)
    {
        for(float i = 1f; i <= 1.2; i += 0.05f)
        {
            obj.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
        obj.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        if(obj == cardNumberTextUser)
          obj.SetText("Cards: " + Game.instance.userCardsList.Count);
        else if(obj == cardNumberTextSystem)
            obj.SetText("Cards: " + Game.instance.systemCardsList.Count);
        for (float i=1.2f;i>=1f;i -=0.05f)
        {
            obj.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }
    }




    private void OnDisable()
    {
        CustomEvents.OnCardDistributedForSystem -= CardAddedToSystem;
        CustomEvents.OnCardDistributedForUser   -= CardAddedToUser;
    }
}
