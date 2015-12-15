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
                Debug.LogError("!!! already have a GameDriver!");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }
#endregion

        public static float RECIPE_EFFECT_DELAY = .5f;
        private static float RECIPE_CHANGE_DELAY = 1.5f;

        public Text recipeLabel, ingredientLabel, timeLabel, scoreLabel, timeExpiredLabel;
        public GameObject GameOverPanel;
        public RecipeEffect effectCheck, effectX;
        public AudioClip soundYes, soundNo;
        public Sprite heartEmpty, heartFull;
        public Image[] hearts;
        public Button[] gameButtons;
        public Text[] gameLabels;

        private int currentLife = 3;
        private float timeMax = 5, timeReduction = 1, timeStarted = -1;
        private int score = 0;
        private Ingredient newIngredient = null;
        private bool isTimeRunning = false, isGameRunning = false;
        private AudioSource audioSource;

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
            isTimeRunning = true;
            isGameRunning = true;
            EnableButtons();
            EnableLabels();
        }
        private void EndGame() {
            Debug.Log("+++ ending game!");
            isTimeRunning = false;
            isGameRunning = false;
            GameOverPanel.SetActive(true);
            DisableButtons();
            DisableLabels();
        }
        private void UpdateLabels() {
            recipeLabel.text = "Build " + RecipeManager.Instance.CurrentRecipe.displayName + "!";
            ingredientLabel.text = "Add " + newIngredient.displayName + "?";
            scoreLabel.text = "Things built: " + score;
            for (int i = 0; i < hearts.Length; i++) {
                if (currentLife > i) {
                    hearts[i].sprite = heartFull;
                } else {
                    hearts[i].sprite = heartEmpty;
                }
            }
        }
        private void UpdateTime() {
            string timeDisplay = "";
            if (isTimeRunning) {
                float timeLeft = Mathf.Max(0, timeMax - (Time.time - timeStarted));
                timeDisplay = timeLeft.ToString(".00");
                if (timeLeft == 0) {
                    timeDisplay = "0";
                    TimeExpired();
                }
            }
            timeLabel.text = "Time: " + timeDisplay;
        }
        private void TimeExpired() {
            timeExpiredLabel.gameObject.SetActive(true);
            InitNextRecipe(false, false);
        }
        private void InitNextRecipe(bool success, bool effectDelay) {
            StartCoroutine(_initNextRecipe(success, effectDelay));
        }
        private IEnumerator _initNextRecipe(bool success, bool effectDelay) {
            isTimeRunning = false;
            ingredientLabel.enabled = false;
            DisableButtons();
            if (effectDelay) {
                yield return new WaitForSeconds(RECIPE_EFFECT_DELAY);
            }
            if (success) {
                effectCheck.Go();
            } else {
                effectX.Go();
                currentLife--;
                if(currentLife <= 0) {
                    EndGame();
                }
            }
            UpdateLabels();
            yield return new WaitForSeconds(RECIPE_CHANGE_DELAY);
            timeExpiredLabel.gameObject.SetActive(false);
            if (isGameRunning) {

                ingredientLabel.enabled = true;
                RecipeManager.Instance.ChooseNewRecipe();
                newIngredient = RecipeManager.Instance.GetRandomIngredient(null);
                isTimeRunning = true;
                timeMax -= timeReduction;
                timeStarted = Time.time;
                UpdateLabels();
                EnableButtons();
            }
        }

#region Game callbacks
        public void RecipeFailed() {
            InitNextRecipe(false, true);
        }
        public void RecipeSucceeded() {
            score++;
            InitNextRecipe(true, true);
        }
#endregion

#region Unity callbacks
        public void Start() {
            audioSource = GetComponent<AudioSource>();
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
            audioSource.clip = soundYes;
            audioSource.Play();
            bool finished = RecipeManager.Instance.AddIngredient(newIngredient);
            if (!finished) {
                newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
                UpdateLabels();
            }
        }

        public void NoClicked() {
            audioSource.clip = soundNo;
            audioSource.Play();
            newIngredient = RecipeManager.Instance.GetRandomIngredient(newIngredient);
            UpdateLabels();
        }
        public void PlayAgainClicked() {
            SceneManager.LoadScene("title");
            //Application.LoadLevel(Application.loadedLevel);
        }
#endregion
    }
}