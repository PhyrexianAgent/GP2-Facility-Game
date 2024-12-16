using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class StaminaControl : MonoBehaviour
{
    [SerializeField] private Image barFill;
    [SerializeField] private CanvasGroup bar;
    [Range(1, 10)]
    [SerializeField] private float fadeRate = 5f;
    private Coroutine barFader;

    // Start is called before the first frame update
    void Awake()
    {
        bar = GetComponentInParent<CanvasGroup>();
    }



    public void updateStamBar(float stam, float maxStam)
    {
        if (stam != maxStam)
        {
            bar.alpha = 1;
            if (barFader != null) { StopCoroutine(barFade()); }
        }
        else
        {
            barFader = StartCoroutine(barFade());
        }
        barFill.fillAmount = stam / maxStam;
    }
    public IEnumerator barFade()
    {
        while (bar.alpha != 0)
        {
            bar.alpha = bar.alpha - (fadeRate/1000);
            yield return new WaitForEndOfFrame();
        }
    }

        // Update is called once per frame
        void Update()
        {

        }
}
