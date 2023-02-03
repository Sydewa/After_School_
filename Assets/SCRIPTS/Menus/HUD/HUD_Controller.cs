using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HUD_Controller : MonoBehaviour
{
    [SerializeField] GameObject[] healthBars;
    [SerializeField] int[] characterOrder = new int[] {0, 1, 2, 3};
    [SerializeField]Vector2 scaleHUD;
    [SerializeField]Vector2[] hudPositions;
    [SerializeField]Vector2 activeBarPosition;

    private int currentCharacter = 0;

    [SerializeField]float animationDuration;

    private void Start()
    {
        currentCharacter = 0;
        UpdateHealthBars();
    }

    private void UpdateHealthBars()
    {
        
        for (int i = 0; i < healthBars.Length; i++)
        {
            RectTransform healthBar = healthBars[characterOrder[i]].GetComponent<RectTransform>();
            Vector2 startPos = healthBar.anchoredPosition;

            if (i == currentCharacter)
            {
                healthBar.transform.localScale = new Vector3(scaleHUD.y, scaleHUD.y, scaleHUD.y);

                if (startPos != activeBarPosition)
                {
                    healthBar.DOAnchorPos(activeBarPosition, animationDuration).SetEase(Ease.InOutQuad);
                }
            }
            else
            {
                int inactiveIndex = (i < currentCharacter) ? i : i - 1;
                Vector2 inactivePos = hudPositions[inactiveIndex];
                healthBar.transform.localScale = new Vector3(scaleHUD.x, scaleHUD.x, scaleHUD.x);

                if (startPos != inactivePos)
                {
                    healthBar.DOAnchorPos(inactivePos, animationDuration).SetEase(Ease.InOutQuad);
                }
            }
        }
    }



    public void ChangeActiveCharacter(int newCharacter)
    {
        currentCharacter = newCharacter;
        UpdateHealthBars();
    }
}
