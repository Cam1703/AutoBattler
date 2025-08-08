using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitStats stats;
    private float currentHP;
    private float attackCooldown;
    private SpriteRenderer spriteRenderer;

    public bool IsDead => currentHP <= 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.Sprite;
        currentHP = stats.MaxHP;
        attackCooldown = 1f / stats.AttackSpeed; // Inicializa el cooldown de ataque 
    }

    public void ReceiveDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateSpriteColor();
        Debug.Log($"{stats.UnitName} recibió {amount} de daño. HP restante: {currentHP}");
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void Attack(Unit target)
    {
        target.ReceiveDamage(stats.AttackDamage);
        attackCooldown = 1f / stats.AttackSpeed; // tiempo entre ataques
    }

    public void TickCooldown(float deltaTime)
    {
        if (attackCooldown > 0) attackCooldown -= deltaTime;
    }

    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void UpdateSpriteColor()
    {
        if (!IsDead)
        {
            //Efecto parpadeo
            spriteRenderer.color = Color.red; // Cambia el color a rojo temporalmente para indicar daño
            Invoke("ResetSpriteColor", 0.2f); // Llama a ResetSpriteColor después de 0.1 segundos.
        }
        else
        {
            spriteRenderer.color = Color.gray; // Cambia el color a gris si está muerto
        }
    }


    private void ResetSpriteColor()
    {
        if (!IsDead)
        {
            spriteRenderer.color = Color.white; // Resetea el color a blanco si está vivo
        }
    }
}
