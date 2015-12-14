using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LudumDare34 {
    public class GameDriver : MonoBehaviour {
#region Singleton
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
#endregion

        public static float RECIPE_SUCCESS_DELAY = .5f;
        private static float RECIPE_CHANGE_DELAY = 2f;

        public Text recipeLabel, ingredientLabel, timeLabel, scoreLabel;
        public GameObject GameOverPanel;
        public Button[] gameButtons;
        public Text[] gameLabels;

        private float timeMax = 15, timeReduction = 1, timeStarted = -1;
        private int score = 0;
        private Ingredient newIngredient = null;
        private bool isGameRunning = false;

        private void EnableButtons() {
            foreach (Button b in gameButtons) {
                b.interactable = true;
            }
        }
        private void DisableButtons() {
            foreach (Button b in gameButtons) {
                b.interactable = false;
            }
        }
        private void EnableLabels() {
            foreach (Text t in gameLabels) {
                t.enabled = true;
            }
        }
        private void DisableLabels() {
            foreach (Text t in gameLabels) {
                t.enabled = false;
            }
        }
        private void StartGame() {
            timeStarted = Time.time;
            isGameRunning = true;
            EnableButtons();
            EnableLabels();
        }
        private void EndGame() {
            Debug.Log("+++ Game over!");
            isGameRunning = false;
            GameOverPanel.SetActive(true);
            DisableButtons();
            DisableLabels();
        }
        private void UpdateLabels() {
            recipeLabel.text = "Build " + RecipeManager.Instance.CurrentRecipe.displayName + "!";
            ingredientLabel.text = "Add " + newIngredient.displayName + "?";
            scoreLabel.text = "Things built: " + score;
        }
        private void UpdateTime() {
            string timeDisplay = "";
            if (isGameRunning) {
                float timeLeft = Mathf.Max(0, timeMax - (Time.time - timeStarted));
                timeDisplay = timeLeft.ToString(".00");
                if (timeLeft == 0) {
                    timeDisplay = "0";
                    if (isGameRunning) {
                        EndGame();
                    }
                }
            }
            timeLabel.text = "Time: " + timeDisplay;
        }
        private void InitNextRecipe() {
            StartCoroutine(_initNextRecipe());
        }
        private IEnumerator _initNextRecipe() {
            ingredientLabel.enabled = false;
            yield return new WaitForSeconds(RECIPE_CHANGE_DELAY);
            ingredientLabel.enabled = true;
            RecipeManager.Instance.ChooseNewRecipe();
            newIngredient = RecipeManager.Instance.GetRandomIngredient(null);
            isGameRunning = true;
            timeMax -= timeReduction;
            timeStarted = Time.time;
            UpdateLabels();
            EnableButtons();
        }

#region Game callbacks
        public void RecipeFailed() {
            Debug.Log("+++ todo.");
        }
        public void RecipeSucceeded() {
            score++;
            isGameRunning = false;
            DisableButtons();
            InitNextRecipe();
        }
#endregion

#region Unity callbacks
        public void Start() {
            newIngredient = RecipeManager.Instance.GetRandomIngredient(null);
            UpdateLabels();
            StartGame();
        }
        public void Update() {
            UpdateTime();
        }
#endregion

#region UI callbacks
        public void YesClicked() {
            bool finished = RecipeManager.Instance.AddIngredient(newIngredient);
            if (!finished) {
                newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
                UpdateLabels();
            }
        }

        public void NoClicked() {
            newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
            UpdateLabels();
        }
        public void PlayAgainClicked() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Application.LoadLevel(Application.loadedLevel);
        }
#endregion
    }
}