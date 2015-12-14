using UnityEngine;
using System.Collections;

namespace LudumDare34 {
    public class HitSound : MonoBehaviour {
        public AudioClip clip;
        public void OnCollisionEnter(Collision col) {
            AudioSource.PlayClipAtPoint(clip, col.contacts[0].point);
        }
    }
}