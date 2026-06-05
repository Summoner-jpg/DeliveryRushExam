using TMPro;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float lifetime = 1.1f;
        [SerializeField] private float moveSpeed = 55f;

        private float age;
        private ScorePopupPool pool;

        // Pool le pasa su referencia para entregar el popup
        public void Setup(string message, ScorePopupPool ownerPool)
        {
            age = 0f;
            messageText.text = message;
            pool = ownerPool;
        }

        private void Update()
        {
            age += Time.deltaTime;
            transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - age / lifetime;
            }

            if (age >= lifetime)
            {
                pool.Release(this); // trato de evitar Destroy Object
            }
        }
    }
}
