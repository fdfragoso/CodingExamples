using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    public int Health => health;


    [SerializeField]
    public Slider hpBar; //Placed in this script since all characters will have their HP Bar
    [SerializeField]
    public TextMeshProUGUI curHPText, maxHPText;

    private void Start()
    {
        hpBar.maxValue = health;
        if(maxHPText != null)
            maxHPText.text = string.Format(maxHPText.text, health);
    
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
            //Death shtuff
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        hpBar.value = health;
        if (curHPText != null)
            curHPText.text = health.ToString();
    }
}
