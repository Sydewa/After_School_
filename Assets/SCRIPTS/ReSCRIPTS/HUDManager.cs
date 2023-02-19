using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    [Header ("Masks de las healthbars")]
    [SerializeField]Image EricMask;
    [SerializeField]Image AntiaMask;
    [SerializeField]Image SoraMask;
    [SerializeField]Image MossiMask;

    [Header ("Cambio de activeCharacter")]
    [SerializeField] GameObject[] healthBars;
    [SerializeField] int[] characterOrder = new int[] {0, 1, 2, 3};
    [SerializeField]Vector2 scaleHUD;
    [SerializeField]Vector2[] hudPositions;
    [SerializeField]Vector2 activeBarPosition;
    int currentCharacter = 0;
    [SerializeField]float animationDuration;

    void Awake()
    {
#region Singelton
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
#endregion
        currentCharacter = 0;
        UpdateHealthBars();
    }

    private void Start()
    {
        //UpdateHealthBars();
    }

    void Update()
    {
        EricHealthBar();
        AntiaHealthBar();
        SoraHealthBar();
        MossiHealthBar();
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

#region Health Bars get fill
    void EricHealthBar()
    {
        float fillAmount = (float)EricStateManager.Instance.CurrentHealth / (float)EricStateManager.Instance.Health;
        EricMask.fillAmount = fillAmount;
    }

    void AntiaHealthBar()
    {
        float fillAmount = (float)AntiaStateManager.Instance.CurrentHealth / (float)AntiaStateManager.Instance.Health;
        AntiaMask.fillAmount = fillAmount;
    }

    void SoraHealthBar()
    {
        float fillAmount = (float)SoraStateManager.Instance.CurrentHealth / (float)SoraStateManager.Instance.Health;
        SoraMask.fillAmount = fillAmount;
    }

    void MossiHealthBar()
    {
        float fillAmount = (float)MossiStateManager.Instance.CurrentHealth / (float)MossiStateManager.Instance.Health;
        MossiMask.fillAmount = fillAmount;
    }
#endregion
}
