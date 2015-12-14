using UnityEngine;
using System.Collections;

namespace LudumDare34 {
    public class RecipeManager : MonoBehaviour {

        private static RecipeManager _instance = null;
        public static RecipeManager Instance {
            get {
                if (_instance == null) {
                    Debug.LogError("!!! No RecipeManager singleton!");
                    return null;
                }
                return _instance;
            }
        }
        public void Awake() {
            if (_instance != null) {
                Debug.LogError("!!! already have a RecipeManager!");
                Destroy(gameObject);
                return;
            }
            _instance = this;
            ChooseNewRecipe();
        }

        //private static float RIGHT_INGREDIENT_CHANCE = 0.33f;
        private static float RIGHT_INGREDIENT_CHANCE = 1f;

        public Recipe[] recipes;

        private Recipe currentRecipe = null;
        private Recipe currentInstance = null;

        public Recipe CurrentRecipe { get { return currentRecipe; } }
        int nextIngredientIndex = 0;

        public Ingredient GetRandomIngredient(Ingredient lastIngredient) {
            Ingredient ing = null;
            while (true) {
                if (Random.Range(0f, 1f) < RIGHT_INGREDIENT_CHANCE) {
                    ing = GetRightIngredient();
                } else {
                    ing = GetWrongIngredient();
                }

                if (ing != lastIngredient) {
                    return ing;
                }
            }
        }
        private Ingredient GetRightIngredient() {
            return currentRecipe.ingredients[nextIngredientIndex];
        }
        private Ingredient GetWrongIngredient() {
            // choose a random ingredient from a different recipe
            while (true) {
                int i = Random.Range(0, recipes.Length);
                Recipe r = recipes[i];
                if (r != currentRecipe) {
                    return r.ingredients[Random.Range(0, r.ingredients.Length)];
                }
            }
        }

        public bool AddIngredient(Ingredient ing) {
            GameObject obj = Instantiate(ing.gameObject);
            obj.transform.parent = currentInstance.transform;
            obj.transform.position = Vector3.up;

            if (ing != currentRecipe.ingredients[nextIngredientIndex]) {
                Debug.Log("+++ Adding INCORRECT ing " + ing.displayName);
                currentInstance.Failed();
                GameDriver.Instance.RecipeFailed();
                return true;
            } else {
                Debug.Log("+++ Adding correct ing " + ing.displayName);
                nextIngredientIndex++;
                if (nextIngredientIndex == currentRecipe.ingredients.Length) {
                    Debug.Log("+++ recipe completed!");
                    currentInstance.Succeeded();
                    GameDriver.Instance.RecipeSucceeded();
                    return true;
                }
            }
            return false;
        }

        public void ChooseNewRecipe() {
            nextIngredientIndex = 0;
            // choose a recipe that isn't the current one
            while (true) {
                int r = Random.Range(0, recipes.Length);
                if (recipes[r] != currentRecipe) {
                    currentRecipe = recipes[r];
                    if (currentInstance != null) {
                        Destroy(currentInstance.gameObject);
                    }
                    currentInstance = Instantiate(currentRecipe.gameObject).GetComponent<Recipe>();
                    return;
                }
            }
        }

    }
}