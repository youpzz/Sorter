using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "youpzdev/scriptables/new item")]
public class ItemScriptable : ScriptableObject
{
    [Header("Основная Информация")]
    public string itemName;
    public string itemDescription;


    [Space(10)]
    [Header("Префабы")]

    [Tooltip("Префаб объекта, который будет спавниться на конвейерной ленте")]
    public GameObject itemPrefab;
    
    [Tooltip("Префаб объекта, который игрок будет осматривать")]
    public GameObject inspectionPrefab;

    [Space(10)]
    [Header("Настройки")]

    [Tooltip("Ценность предмета (Мусор/Обычный/Ценный)")]
    public Rarity rarity;
    [Tooltip("Тип предмета (Стандартный/Аномальный)")]
    public Type type;
}


public enum Rarity
{
    Trash, Common, Rare
}

public enum Type
{
    Standart, Anomaly
}
