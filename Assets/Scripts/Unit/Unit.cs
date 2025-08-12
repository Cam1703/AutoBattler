using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitStats stats;
    private float currentHP;
    private float attackCooldown;
    private SpriteRenderer spriteRenderer;
    private TMP_Text hpText;
    private TMP_Text cooldownText;
    private Coroutine attackCoroutine;
    public bool IsDead => currentHP <= 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.Sprite;
        currentHP = stats.MaxHP;
        attackCooldown = 1f / stats.AttackSpeed; // Inicializa el cooldown de ataque 
        hpText = transform.Find("HPText").GetComponent<TMP_Text>();
        cooldownText = transform.Find("CooldownText").GetComponent<TMP_Text>();
        spriteRenderer.flipX = stats.UnitFaction == UnitFaction.Enemy ? true : false; 
        UpdateHPText();
    }

    public void ReceiveDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateSpriteColor();
        UpdateHPText();

    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void Attack(Unit target)
    {
        target.ReceiveDamage(stats.AttackDamage);
        attackCooldown = 1f / stats.AttackSpeed; // tiempo entre ataques
        AttackMockAnimation(); // Llama a la animación de ataque
    }

    public void TickCooldown(float deltaTime)
    {
        if (attackCooldown > 0) attackCooldown -= deltaTime;
        UpdateCooldown();
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

    private void UpdateHPText()
    {
        hpText.text = $"HP: {currentHP}/{stats.MaxHP}";
        hpText.color = IsDead ? Color.gray : Color.white; // Cambia el color del texto si está muerto
    }

    private void UpdateCooldown()
    {
        cooldownText.text = $"{attackCooldown:F1}s";
    }


    private void AttackMockAnimation()
    {
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackAnimationCoroutine(spriteRenderer.flipX));
    }

    private IEnumerator AttackAnimationCoroutine(bool isFlipped)
    {
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = startPosition + new Vector2(isFlipped ? -0.5f : 0.5f, 0);
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = startPosition; // Volver al punto inicial
    }


}
