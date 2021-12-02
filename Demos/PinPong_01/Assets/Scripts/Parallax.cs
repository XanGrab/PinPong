using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Vector3 startPos;
    SpriteRenderer mySprite;
    public float moveSpeed;

    Transform myTrans;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.gameObject.transform.position;
        mySprite = this.gameObject.GetComponent<SpriteRenderer>();
        myTrans = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTrans.Translate(moveSpeed, 0, 0);
        if(myTrans.position.x > (startPos.x + (1.5 * mySprite.bounds.size.x))){
            myTrans.position = new Vector3 (startPos.x - (1.5f * mySprite.bounds.size.x), myTrans.position.y, myTrans.position.z);
        }
    }
}
