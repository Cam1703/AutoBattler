using UnityEngine;

[CreateAssetMenu(menuName = "AutoBattler/UnitStats")]
public class UnitStats : ScriptableObject
{
    [SerializeField] string unitName = "Unit";
    [SerializeField] float maxHP = 100f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackSpeed = 1f; // ataques por segundo
    [SerializeField] float defense = 0f;
    [SerializeField] UnitType unitType = UnitType.Tank;
    [SerializeField] Sprite sprite;
    [SerializeField] UnitFaction unitFaction = UnitFaction.Player;
    [SerializeField] bool givesXP = true;
    [SerializeField] int xpValue = 10;

    public string UnitName { get => unitName; set => unitName = value; }
    public float MaxHP { get => maxHP; set => maxHP = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Defense { get => defense; set => defense = value; }
    public UnitType UnitType { get => unitType; set => unitType = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public UnitFaction UnitFaction { get => unitFaction; set => unitFaction = value; }
    public bool GivesXP { get => givesXP; set => givesXP = value; }
    public int XpValue { get => xpValue; set => xpValue = value; }
}

public enum UnitType
{
    Ranged,
    Tank
}

public enum UnitState
{
    Idle,
    Attacking,
    Moving,
    Dead
}

public enum UnitFaction
{
    Player,
    Enemy
}
