using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{
    public string itemName;
}

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Crafteo/Receta")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;

    [Header("Ingredientes")]
    public CraftingIngredient[] ingredientes;

    [Header("Resultado")]
    public ItemData resultado;

    [Header("Tiempo de crafteo")]
    public float craftingTime = 2f;
}
