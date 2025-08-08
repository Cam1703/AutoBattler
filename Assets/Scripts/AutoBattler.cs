using UnityEngine;

public class AutoBattler : MonoBehaviour
{
    [SerializeField] Unit player;
    [SerializeField] Unit enemy;

    void Update()
    {
        if (player.IsDead || enemy.IsDead)
        {
            EndBattle();
            return;
        }

        player.TickCooldown(Time.deltaTime);
        enemy.TickCooldown(Time.deltaTime);

        if (player.CanAttack())
            player.Attack(enemy);

        if (enemy.CanAttack())
            enemy.Attack(player);
    }

    void EndBattle()
    {
        if (player.IsDead)
            Debug.Log("Jugador perdi�");
        else if (enemy.IsDead)
            Debug.Log("Jugador gan�");
    }
}
