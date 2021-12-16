using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    private Camera mainCam;
    private float targetAspectRatio = 1.6f;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LetterBoxCamera();
    }

    private void LetterBoxCamera(){
        float w = Screen.width;
        float h = Screen.height;
        float a = w / h;
        Rect r;
        if (a > targetAspectRatio)
        {
            float tw = h * targetAspectRatio;
            float o = (w - tw) * 0.5f;
            r = new Rect(o,0,tw,h);
        }
        else
        {
            float th = w / targetAspectRatio;
            float o = (h - th) * 0.5f;
            r = new Rect(0, o, w, th);
        }
        mainCam.pixelRect = r;
    }
}
