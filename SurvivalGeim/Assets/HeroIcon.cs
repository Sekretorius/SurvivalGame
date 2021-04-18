using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeroIcon : MonoBehaviour
{
    public static HeroIcon instance;

    [SerializeField]
    public Image sprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public IEnumerator Injured()
    {
        if (sprite.color == Color.red)
            yield break;

        Color old = sprite.color;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sprite.color = old;
    }
}
