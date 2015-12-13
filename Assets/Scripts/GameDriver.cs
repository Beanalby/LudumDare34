using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LudumDare34 {
    public class GameDriver : MonoBehaviour {

        private static GameDriver _instance = null;
        public static GameDriver Instance {
            get {
                if (_instance == null) {
                    Debug.LogError("!!! No GameDriver singleton!");
                    return null;
                }
                return _instance;
            }
        }
        public void Awake() {
            if (_instance != null) {
                Debug.LogError("+++ already have a GameDriver!");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public Text recipeLabel, ingredientLabel;

        private Ingredient newIngredient = null;

        private void UpdateLabels() {
            recipeLabel.text = "Build " + RecipeManager.Instance.CurrentRecipe.displayName + "!";
            ingredientLabel.text = "Add " + newIngredient.displayName + "?";
        }

#region Unity callbacks
        public void Start() {
            newIngredient = RecipeManager.Instance.GetRandomIngredient(null);
            UpdateLabels();
        }
#endregion

#region UI callbacks
        public void YesClicked() {
            RecipeManager.Instance.AddIngredient(newIngredient);
            newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
            UpdateLabels();
        }

        public void NoClicked() {
            newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
            UpdateLabels();
        }
#endregion
    }
}