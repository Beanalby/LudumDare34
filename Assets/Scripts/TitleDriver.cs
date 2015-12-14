using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LudumDare34 {
    public class TitleDriver : MonoBehaviour {
        public void StartClicked() {
            SceneManager.LoadScene("game");
        }
    }
}