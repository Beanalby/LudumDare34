using UnityEngine;
using System.Collections;

namespace LudumDare34 {
    public class RecipeEffect : MonoBehaviour {
        //private Animator anim;
        public void Start() {
            //anim = GetComponent<Animator>();
            foreach (Transform t in transform) {
                t.gameObject.SetActive(false);
            }
        }
        public void Go() {
            foreach (Transform t in transform) {
                t.gameObject.SetActive(true);
            }
            //anim.SetTrigger("Spin");
            StartCoroutine(_stop());
        }
        private IEnumerator _stop() {
            yield return new WaitForSeconds(1.5f);
            //anim.Stop();
            foreach (Transform t in transform) {
                t.gameObject.SetActive(false);
            }
        }

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Go();
            }
        }
    }
}