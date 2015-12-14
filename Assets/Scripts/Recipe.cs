using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LudumDare34 {
    public class Recipe : MonoBehaviour {

        public string displayName;
        public GameObject modelTopPrefab;
        public GameObject modelBottomPrefab;
        public Ingredient[] ingredients;

        private GameObject modelBottom = null;
        private GameObject modelTop = null;

        public void Start() {
            transform.position = Vector3.zero;
            modelBottom = Instantiate(modelBottomPrefab);
            modelBottom.transform.parent = transform;
            modelBottom.transform.position = Vector3.zero;
        }

        public void Succeeded() {
            StartCoroutine(_succeeded());
        }
        private IEnumerator _succeeded() {
            yield return new WaitForSeconds(GameDriver.RECIPE_EFFECT_DELAY);
            if (modelTopPrefab != null) {
                modelTop = Instantiate(modelTopPrefab);
                modelTop.transform.parent = transform;
                modelTop.transform.position = Vector3.up;
            }
        }
        public void Failed() {
        }
    }
}