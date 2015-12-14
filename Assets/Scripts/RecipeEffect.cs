using UnityEngine;
using System.Collections;

namespace LudumDare34 {
    public class RecipeEffect : MonoBehaviour {
        private AudioSource audioSource;
        //private Animator anim;
        public void Start() {
            //anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            foreach (Transform t in transform) {
                t.gameObject.SetActive(false);
            }
        }
        public void Go() {
            audioSource.Play();
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