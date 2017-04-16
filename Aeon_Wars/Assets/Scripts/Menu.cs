using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    public bool Display;
    private Animator anim;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        setDisplay(Display);
    }

    public void setDisplay(bool value)
    {
        anim.SetBool("Display",value);
        canvasGroup.blocksRaycasts = canvasGroup.interactable = value;
    }
}
