using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] characters;
    private int currentCharacter = 0;

    public int CurrectCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            characters[i].SetActive(i == this.currentCharacter);
        }
    }
}

