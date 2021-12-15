using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP : MonoBehaviour
{
    public GameObject uiHP;
    private TextMeshProUGUI uiHP_txt;

    [SerializeField]
    public int hp;
    [SerializeField]
    public int maxHp;

    void Start(){
        uiHP_txt = uiHP.GetComponent<TextMeshProUGUI>();
        if(uiHP_txt == null){
            //Debug.Log("UI null.");
        }
        UpdateHealth();
    }
    
    public void UpdateHealth(){
        if(hp > maxHp){
            hp = maxHp;
        }
        if(hp < 0){
            hp = 0;
        }

        uiHP_txt.text = hp.ToString();
    }
}
