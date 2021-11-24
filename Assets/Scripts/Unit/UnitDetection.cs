using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unit
{
    public class UnitDetection : MonoBehaviour
    {
        [Title("Reference")]
        public Unit mainScript;


        private void OnTriggerEnter2D(Collider2D other)
        {
            mainScript.NewInteraction(other.transform.GetComponent<Unit>());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            mainScript.ExitInteraction(other.transform.GetComponent<Unit>());
        }
    }
}