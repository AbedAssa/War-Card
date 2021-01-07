using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game class is controlling the game, responsible for playing animation and follow the game roules.
/// </summary>
public class Game : MonoBehaviour
{
    public static Game instance;

    public List<(ECards Card, Sprite Img)> userCardsList { private set; get; }   //User cards.
    public List<(ECards Card, Sprite Img)> systemCardsList { private set; get; } //System cards.
    public GameObject prefabUserCard;                                            //User animated card.  
    public GameObject prefabSystemCard;                                          //System animated card.
    public Transform userPrefabLocation;                                         //Location for instantiate user animated card.                                           
    public Transform systemPrefabLocation;                                       //Location for instantiate system animated card.
    public Button playButtonCard;                                                //Button should be clicked to play card.
    public GameObject CardHolderTopStack;                                        //The card holder in the top(to make cards slide down of the stack).
    public GameObject prefabDrawPanel;                                           //Draw animation cards.
    public GameObject resultPanel;                                               //Results panel for gane ends.




    public GameObject userCardHolder;                                           //Display a card for user UI.
    public GameObject systemCardHolder;                                         //Display a card for system UI.
    public Sprite blueCard;                                                     //Sprite of card back (uncoverd).

    private GameObject prefabUserObject;                                        //hold a refrence to the instantiated prefabUserCard.
    private GameObject prefabSystemObject;                                      //hold a refrence to the instantiated prefabSystemCard.
    private GameObject prefabDrawnObject;                                       //hold a refrence to the instantiated prefabDrawPanel.
    private List<GameObject> listOldCards;                                      //holding list of old instantiated animation cards.                                     
    private bool triggerOnce = false;                                           //Check variable.
    private bool drawhappens = false;                                           //Check variable.
    private List<(ECards Card, Sprite Img)> threeCards;                         //Holds the values to add it later to the winner. 



    //Instantiate
    //Subscribe events
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        userCardsList = LoadData.instance.userCardsList;
        systemCardsList = LoadData.instance.systemCardsList;
        playButtonCard.onClick.AddListener(OnClickPlayCardButton);
        CustomEvents.OnExitSecondFlipUserState += OnCardFacingDown;
        CustomEvents.OnExitFirstFlipUserState += OnCardMovingFromStack;
        CustomEvents.OnExitMovingCardCenterState += ShowCardHolderInCenter;
        listOldCards = new List<GameObject>();
        threeCards = new List<(ECards Card, Sprite Img)>();
    }

    //Keep checking if both players have cards to play.
    private void Update()
    {
        ShowResults();
    }


    //Get called when user click button to play, instantiate card animation,and chhecking for draw
    //Destroy useless obk=jects.
    private void OnClickPlayCardButton()
    {
        SlidUpDown();
        prefabDrawnObject?.transform.GetChild(0).gameObject.SetActive(false);
        if (userCardsList.Count == 0 || systemCardsList.Count == 0)
        {
            return;
        }

        if (drawhappens)
        {
            PlayDrawAnimation();
            listOldCards.Add(prefabUserObject);
            listOldCards.Add(prefabSystemObject);
        }
        else
        { 
             Destroy(prefabUserObject);
             Destroy(prefabSystemObject);
        }
        int winnerNum = CheckWinner(userCardsList[0], systemCardsList[0]);
        CheckDraw(winnerNum);
        prefabUserObject = Instantiate(prefabUserCard, userPrefabLocation.parent);
        prefabSystemObject = Instantiate(prefabSystemCard,systemPrefabLocation.parent);
        triggerOnce = false;
        Audio.instance.PlayClickAudio();
        SetActiveCardHolderTopStack(false, false);

    }

    //Showing the card holder on the screen for User and System on the bottom
    //of the card stack, to make slid up affect, and remove this card holder in case
    //there is one card left.
    void SlidUpDown()
    {
        if (userCardsList.Count == 1)
        {
            userCardHolder.SetActive(false);
        }
        else if (userCardsList.Count > 1)
        {
            userCardHolder.SetActive(true);
        }
        if (systemCardsList.Count == 1)
        {
            systemCardHolder.SetActive(false);

        }
        else if (systemCardsList.Count > 1)
        {
            systemCardHolder.SetActive(true);
        }

    }

    //Check if there is a draw and player have extra 3 cards, if so play draw animation
    //Otherwise it ends the game.
    private void CheckDraw(int winnerNumber)
    {
        if (winnerNumber == 0)
        {
            drawhappens = true;
            if (userCardsList.Count < 5)
            {
                ShowResultsDraw(1);
                SetActiveCardHolderTopStack(false, true);
                
            }
            else if (systemCardsList.Count < 5)
            {
                ShowResultsDraw(2);
                SetActiveCardHolderTopStack(true, false);
            }
            else
            {
                prefabDrawnObject = Instantiate(prefabDrawPanel, userPrefabLocation.parent);

            }
        }
    }

    //Play Draw Animation and remove the played card pluse 3 cards
    //from each player to new list.
    private void PlayDrawAnimation()
    {
        if(userCardsList.Count > 5 && systemCardsList.Count > 5)
        {
            threeCards = new List<(ECards Card, Sprite Img)>();
            for (int i = 0; i < 4; i++)
            {
                threeCards.Add(userCardsList[0]);
                threeCards.Add(systemCardsList[0]);
                userCardsList.RemoveAt(0);
                systemCardsList.RemoveAt(0);
            }

        }
        drawhappens = false;
    }

    //This event get triggered when animation of playing card is start, and fisrt state is ended.
    void OnCardMovingFromStack(object sender){
        int winnerNum = CheckWinner(userCardsList[0], systemCardsList[0]);
        StateMachingChanger(winnerNum);
        prefabUserObject.GetComponent<Image>().sprite  = userCardsList[0].Img;
        prefabSystemObject.GetComponent<Image>().sprite = systemCardsList[0].Img;
    }


    //This event get triggered when the card is flipping on face and showing the back of the card
    //Updating lists of User and System depnds on the winner.
    void OnCardFacingDown(object objec)
    {
        if (triggerOnce == true)
            return;
        triggerOnce = true;
        prefabUserObject.GetComponent<Image>().sprite = blueCard;
        prefabSystemObject.GetComponent<Image>().sprite = blueCard;
        Audio.instance.PlaySlideCardsAudio();
        if (userCardsList.Count > 1 && systemCardsList.Count > 1)
            SetActiveCardHolderTopStack(true, true);

        int winnerNum = CheckWinner(userCardsList[0], systemCardsList[0]);
        if (winnerNum == 1)
        {
            AddCardToWinner(userCardsList, systemCardsList);
            if(prefabDrawnObject != null)
            {
                prefabDrawnObject.GetComponent<Animator>().SetInteger("Status", 1);
            }
            GetOldValues(userCardsList);
        }
        else if (winnerNum == 2)
        {
            AddCardToWinner(systemCardsList, userCardsList);
            if (prefabDrawnObject != null)
            {
                prefabDrawnObject.GetComponent<Animator>().SetInteger("Status", 2);
            }
            GetOldValues(systemCardsList);
        }

    }


    //Show the card holder in the center of the screen when ever there is a draw.
    //This event get triggired when the first state exit (state name: MoveCardCenter 1).
    void ShowCardHolderInCenter(object sender)
    {
        prefabDrawnObject?.transform.GetChild(0).gameObject.SetActive(true);
    }


    //Add the draw cards where played to the screen to the winner in the next round.
    void GetOldValues(List<(ECards Card, Sprite Img)> lis)
    {
        if (threeCards != null && threeCards.Count > 0)
        {
            for (int i = 0; i < threeCards.Count; i++)
            {
                lis.Add(threeCards[i]);
                print("Errr1");

            }
            threeCards = new List<(ECards Card, Sprite Img)>();
            foreach (GameObject obj in listOldCards)
            {
                Destroy(obj);

            }
            listOldCards = new List<GameObject>();
        }
    }


    //Change card holder to show card sliding down.
    private void SetActiveCardHolderTopStack(bool top,bool bottom)
    {
        CardHolderTopStack.transform.GetChild(0).gameObject.SetActive(top);
        CardHolderTopStack.transform.GetChild(1).gameObject.SetActive(top);
    }

    //CheckWinner or draw
    int CheckWinner((ECards Card, Sprite Img)listItemUser, (ECards Card, Sprite Img) listItemSystem)
    {
        if ((int)listItemUser.Card > (int)listItemSystem.Card)
        {
            return 1;
        }
        else if ((int)listItemUser.Card < (int)listItemSystem.Card)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }


    //Add card to the list of the winner, in case there is no draw
    private void AddCardToWinner(List<(ECards Card, Sprite Img)> winnerList, List<(ECards Card, Sprite Img)> losierList)
    {
        (ECards Card, Sprite Img) loserCard = losierList[0];
        (ECards Card, Sprite Img) winnerCard = winnerList[0];
        losierList.RemoveAt(0);
        winnerList.RemoveAt(0);
        winnerList.Add(winnerCard);
        winnerList.Add(loserCard);
    }

    //Show results at the end of the game if one of player run out of cards
    private void ShowResults()
    {
        if(userCardsList.Count == 0)
        {
            resultPanel.SetActive(true);
            resultPanel.transform.GetChild(0).GetComponent<Text>().text = "User Win";
        }
        if (systemCardsList.Count == 0)
        {
            resultPanel.SetActive(true);
            resultPanel.transform.GetChild(0).GetComponent<Text>().text = "System Win";
        }
    }

    //Show results when there is a draw and one of players donsn't have enough cards.
    private void ShowResultsDraw(int numPlayer)
    {
        resultPanel.SetActive(true);
        if(numPlayer == 1)
            resultPanel.transform.GetChild(0).GetComponent<Text>().text = "System Win, User run out of cards";
        else if(numPlayer == 2)
            resultPanel.transform.GetChild(0).GetComponent<Text>().text = "User Win, System run out of cards";

    }


    //Updade the animation values.
    void StateMachingChanger(int winnerNumber)
    {
        if (winnerNumber == 1)
        {
            prefabUserObject.GetComponent<Animator>().SetInteger("Status", 1);
            prefabSystemObject.GetComponent<Animator>().SetInteger("Status", 1);
        }
        else if (winnerNumber == 2)
        {
            prefabUserObject.GetComponent<Animator>().SetInteger("Status", 2);
            prefabSystemObject.GetComponent<Animator>().SetInteger("Status", 2);
        }
        else
        {
            prefabUserObject.GetComponent<Animator>().SetBool("Draw", true);
            prefabSystemObject.GetComponent<Animator>().SetBool("Draw", true);
        }
    }

    //Unsubscribe events.
    private void OnDisable()
    {
        CustomEvents.OnExitSecondFlipUserState -= OnCardFacingDown;
        CustomEvents.OnExitFirstFlipUserState -= OnCardMovingFromStack;
        CustomEvents.OnExitMovingCardCenterState -= ShowCardHolderInCenter;
    }

}
