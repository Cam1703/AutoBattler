using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitIUIHandler : MonoBehaviour
{
    private List<ItemSO> equippedItems = new List<ItemSO>();

    [SerializeField] List<GameObject> itemSlots = new List<GameObject>();

    private TMP_Text hpText;
    private TMP_Text cooldownText;
    private TMP_Text levelText;


    void Start()
    {
        equippedItems = GetComponent<Unit>().EquippedItems;
        hpText = transform.Find("HPText").GetComponent<TMP_Text>();
        cooldownText = transform.Find("CooldownText").GetComponent<TMP_Text>();
        levelText = transform.Find("LevelText").GetComponent<TMP_Text>();

        InitializeItems();
    }


    public void UpdateHPText(float currentHP, float maxHP, bool isDead)
    {
        hpText.text = $"HP: {currentHP:F0}/{maxHP:F0}";
        hpText.color = isDead ? Color.gray : Color.white; // Cambia el color del texto si está muerto
    }

    public void UpdateCooldown(float attackCooldown)
    {
        cooldownText.text = $"Cooldown: {attackCooldown:F1}s";
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = $"Level: {level}";
    }

    private void InitializeItems()
    {
        for (int i = 0; i < equippedItems.Count; i++)
        {
            {
                var item = equippedItems[i];

                if (item == null) continue;

                itemSlots[i].SetActive(true); // Activar slot
                SpriteRenderer itemBG = itemSlots[i] != null ? itemSlots[i].GetComponent<SpriteRenderer>() : null;
                GameObject child = itemSlots[i] != null ? itemSlots[i].transform.GetChild(0).gameObject : null;
                SpriteRenderer itemImage = child != null ? child.GetComponent<SpriteRenderer>() : null;

                if (itemImage != null)
                {
                    itemImage.sprite = item.Sprite;
                    switch (item.ItemRarity)
                    {
                        case ItemRarity.Common:
                            itemBG.color = Color.white; 
                            break;
                        case ItemRarity.Uncommon:
                            itemBG.color = Color.green;
                            break;
                        case ItemRarity.Rare:
                            itemBG.color = Color.blue; 
                            break;
                        case ItemRarity.Epic:
                            itemBG.color = new Color(0.5f, 0, 0.5f);
                            break;
                        case ItemRarity.Legendary:
                            itemBG.color = Color.yellow; 
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning("No SpriteRenderer found in children to display item sprite.");
                }
            }
        }
    }
}
