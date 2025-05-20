using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text currenHealthText;
    [SerializeField] private TMP_Text maxHealthText;

    public void updateHealth(int health, int maxHealth)
    {
        currenHealthText.text = health.ToString();
        maxHealthText.text = maxHealth.ToString();

    }
}
