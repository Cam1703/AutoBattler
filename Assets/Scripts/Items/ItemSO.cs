using UnityEngine;

[CreateAssetMenu(menuName = "AutoBattler/NewItem")]
public class ItemSO : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] float attack;
    [SerializeField] float attackSpeed;
    [SerializeField] float hp;
    [SerializeField] float defense;
    [SerializeField] Sprite sprite;
    [SerializeField] ItemType itemType;
    [SerializeField] ItemRarity itemRarity;

    public string ItemName { get => itemName; set => itemName = value; }
    public float Attack { get => attack; set => attack = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Hp { get => hp; set => hp = value; }
    public float Defense { get => defense; set => defense = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public ItemType ItemType { get => itemType; set => itemType = value; }
    public ItemRarity ItemRarity { get => itemRarity; set => itemRarity = value; }
}

public enum ItemType
{
    Weapon,
    Armor,
    Accessory
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
