using UnityEngine;

public class PlayerStats : CharacterStats
{
    public override void CheckHealth()
    {
        base.CheckHealth();
        gameManager.instance.playerHUD2.updateHealth(health, maxHealth);
    }
}
