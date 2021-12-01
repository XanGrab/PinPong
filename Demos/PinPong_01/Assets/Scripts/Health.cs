using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Sprite full;
    public Sprite empty;
    public Transform uiHearts;

    public Image[] hearts;
    public int hp;
    public int numHearts;

    void Start(){
        /*hearts = new Image[ uiHearts.childCount ];
        for(int i = 0; i < uiHearts.childCount; i++){
            hearts[i] = uiHearts.GetChild(i).GetComponent<Image>();
        }        

        UpdateHealth();*/
    }
    
    public void UpdateHealth(){
        for(int i = 0; i < hearts.Length; i++){
            if(hp > numHearts){
                hp = numHearts;
            }

            /*if(i < health){
                hearts[i].sprite = full;
            }else{
                hearts[i].sprite = empty;
            }

            if(i < numHearts){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }*/
        }
    }
}
