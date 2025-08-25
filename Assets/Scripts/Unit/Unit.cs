using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitStats stats;
    [SerializeField] List<ItemSO> equippedItems = new List<ItemSO>(); 
    private UnitIUIHandler uiHandler;

    private float maxHP;
    private float currentHP;
    private float attackCooldown;
    private float attackDamage;
    private float attackSpeed;
    private float defense;
    [SerializeField] int level = 1; 

    private SpriteRenderer spriteRenderer;


    private Coroutine attackCoroutine;
    public bool IsDead => currentHP <= 0;

    public List<ItemSO> EquippedItems { get => equippedItems; set => equippedItems = value; }

    void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        uiHandler = GetComponent<UnitIUIHandler>();

        InitializeStats();
        InitializeSprite();
    }

    private void Start()
    {
        uiHandler.UpdateCooldown(attackCooldown);
        uiHandler.UpdateHPText(currentHP, maxHP, IsDead);
        uiHandler.UpdateLevelText(level);
    }

    public void ReceiveDamage(float amount, float defense)
    {
        currentHP -= amount + defense;
        if (currentHP < 0) currentHP = 0;
        UpdateSpriteColor();
        uiHandler.UpdateHPText(currentHP, maxHP, IsDead);
        uiHandler.UpdateLevelText(level);
    }

    public bool CanAttack()
    {
        return attackCooldown <= 0f;
    }

    public void Attack(Unit target)
    {
        target.ReceiveDamage(attackDamage, target.defense);
        attackCooldown = 1f / attackSpeed; // tiempo entre ataques
        AttackMockAnimation(); // Llama a la animación de ataque
    }

    public void TickCooldown(float deltaTime)
    {
        if (attackCooldown > 0) attackCooldown -= deltaTime;
        uiHandler.UpdateCooldown(attackCooldown);
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

    private void InitializeStats()
    {
        maxHP = stats.MaxHP + (equippedItems?.Sum(item => item.Hp) ?? 0f) + LevelBonus()["maxHP"];
        currentHP = maxHP;

        defense = stats.Defense + (equippedItems?.Sum(item => item.Defense) ?? 0f) + LevelBonus()["defense"];

        attackSpeed = stats.AttackSpeed + (equippedItems?.Sum(item => item.AttackSpeed) ?? 0f);
        attackDamage = stats.AttackDamage + (equippedItems?.Sum(item => item.Attack) ?? 0f) + LevelBonus()["attack"];
        attackCooldown = attackSpeed > 0 ? 1f / attackSpeed : float.MaxValue; // evitar división por cero
    }

    private void InitializeSprite()
    {
        spriteRenderer.sprite = stats.Sprite;
        spriteRenderer.flipX = stats.UnitFaction == UnitFaction.Enemy ? true : false;
    }

    public void LevelUP()
    {
        level++;
        InitializeStats(); 
        uiHandler.UpdateHPText(currentHP, maxHP, IsDead);
        uiHandler.UpdateLevelText(level);
    }

    public Dictionary<string, float> LevelBonus()
    {
        float levelBonusAttack = 0f;
        float levelBonusDefense = 0f;
        float levelBonusMaxHP = 0f;

        switch (stats.UnitType)
        {
            case UnitType.Ranged:
                levelBonusAttack = level * 0.5f;
                levelBonusDefense = level * 0.2f;
                levelBonusMaxHP = level * 1f;
                break;

            case UnitType.Tank:
                levelBonusAttack = level * 0.3f;
                levelBonusDefense = level * 0.5f;
                levelBonusMaxHP = level * 2f;
                break;

            default:
                Debug.LogWarning($"Unit type {stats.UnitType} not recognized for level bonuses.");
                break;
        }

        return new Dictionary<string, float>
        {
            { "attack", attackDamage + levelBonusAttack },
            { "defense", defense + levelBonusDefense },
            { "maxHP", maxHP + levelBonusMaxHP }
        };
    }

    private void OnMouseDown()
    {
        Debug.Log($"Unit {stats.UnitName} clicked. Level: {level}, HP: {currentHP}/{maxHP} - Attack: {attackDamage}, Defense: {defense}, Attack Speed: {attackSpeed}");
    }
}


