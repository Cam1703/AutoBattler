using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AutoBattler : MonoBehaviour
{
    [SerializeField] List<Unit> playerUnits;
    [SerializeField] List<Unit> enemyUnits;
    private bool battleEnded = false;

    void Update()
    {
        // Remover unidades muertas
        playerUnits = playerUnits.Where(u => u != null && !u.IsDead).ToList();
        enemyUnits = enemyUnits.Where(u => u != null && !u.IsDead).ToList();

        if (playerUnits.Count == 0 || enemyUnits.Count == 0)
        {
            EndBattle();
            return;
        }

        // Ataques de jugadores
        foreach (var player in playerUnits)
        {
            player.TickCooldown(Time.deltaTime);

            if (player.CanAttack())
            {
                Unit closestEnemy = FindClosestTarget(player, enemyUnits);
                if (closestEnemy != null)
                    player.Attack(closestEnemy);
            }
        }

        // Ataques de enemigos
        foreach (var enemy in enemyUnits)
        {
            enemy.TickCooldown(Time.deltaTime);

            if (enemy.CanAttack())
            {
                Unit closestPlayer = FindClosestTarget(enemy, playerUnits);
                if (closestPlayer != null)
                    enemy.Attack(closestPlayer);
            }
        }
    }

    Unit FindClosestTarget(Unit attacker, List<Unit> potentialTargets)
    {
        Unit closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var target in potentialTargets)
        {
            if (target == null || target.IsDead) continue;

            float dist = Vector2.Distance(attacker.transform.position, target.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = target;
            }
        }

        return closest;
    }

    void EndBattle()
    {
        if (battleEnded) return; // Evitar múltiples llamadas
        battleEnded = true;

        if (playerUnits.Count == 0)
            Debug.Log("Jugador perdió");
        else if (enemyUnits.Count == 0){
            Debug.Log("Jugador ganó");
            foreach (var unit in playerUnits)
            {
                unit.LevelUP(); // Subir de nivel a las unidades del jugador
            }
        }
    }
}
