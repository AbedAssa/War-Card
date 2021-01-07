using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enterance to the game
/// this class display a black panel when game is started and deactivate it when
/// game start.
/// </summary>
public class FadeOut : MonoBehaviour
{
    public Button playButton;                   
    public float fadeOutSpeed = 1f;             
    private bool fadeOut = true;
    private bool buttonClicked = false;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(StartGame);
    }


    //Get triggered when Play button Clicked.
    void StartGame()
    {
        buttonClicked = true;
        anim.SetBool("GameStarted", true);
        Destroy(playButton.gameObject);
    }


    //deactivate the panel when user click Play
    private void Update()
    {
        if (buttonClicked)
        {
            if (fadeOut)
            {
                Color objectColor = GetComponent<Image>().color;
                float fadeOutAmont = objectColor.a - (fadeOutSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeOutAmont);
                GetComponent<Image>().color = objectColor;
                if (objectColor.a <= 0f)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        


    }
}
