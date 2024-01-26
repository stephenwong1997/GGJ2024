using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplayController : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> coloredParts;
    [SerializeField] Animator animator;
    public bool flipX = false;
    public Vector3 originalScale;
    public Color preferredColor;
    float animatorSpeed;
    private void OnEnable()
    {
        //RandomColor();
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
    public void RandomColor()
    {
        for (int i = 0; i < coloredParts.Count; i++)
        {
            coloredParts[i].color = preferredColor;
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
    public enum Weapons
    {
        None = 0,
        Missile = 5,

    }
}
