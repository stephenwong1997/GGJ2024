using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplayController : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> coloredParts;
    [SerializeField] Animator animator;
    [SerializeField] WeaponContoller weaponController;
    public bool flipX = false;
    public Vector3 originalScale;
    public Color preferredColor;
    float animatorSpeed;
    private string[] chickenColor = new string[] { "FF9989", "FF1113", "FFC611", "85D22E", "329F9E", "38AEFF", "38AEFF", "7338FF", "CC58FF", "FFFFFF" };
    private string[] eggColor = new string[] { "FF5F60", "FFF886", "B4FD61", "66E2E1", "6BC3FF", "6B8BFF", "976BFF", "E6ABFF", "FFBFB5", "FFFFFF" };
    public void Start()
    {

    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        animatorSpeed = animator.speed;
        animator.speed = Random.value * 5;
        Invoke("DelayCall", .1f);
    }
    private void DelayCall()
    {
        animator.speed = animatorSpeed;
    }
    public void RandomColor(int PlayerID)
    {
        string hexColor;
        if (PlayerID % 2 == 0)
        {
            hexColor=chickenColor[PlayerID / 2];
        }
        else {
            hexColor = eggColor[PlayerID / 2];
        }


        if (ColorUtility.TryParseHtmlString("#" + hexColor, out preferredColor))
        {
            for (int i = 0; i < coloredParts.Count; i++)
            {
                coloredParts[i].color = preferredColor;
            }
        }

    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }
    public void SetBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }
    public void SetInt(string name, int value)
    {
        animator.SetInteger(name, value);
    }
    private void Update()
    {
        if (flipX)
        {
            Vector3 flipXScale = originalScale;
            flipXScale.x = -originalScale.x;
            transform.localScale = flipXScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}
