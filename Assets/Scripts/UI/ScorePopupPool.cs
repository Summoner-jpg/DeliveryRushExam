using System.Collections.Generic;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupPool : MonoBehaviour
    {
        [SerializeField] private ScorePopupView template;
        [SerializeField] private RectTransform container;  
        [SerializeField] private int initialSize = 5;

        private readonly Queue<ScorePopupView> available = new Queue<ScorePopupView>();

        private void Awake()
        {
            // Oculta el template
            template.gameObject.SetActive(false);

            for (int i = 0; i < initialSize; i++)
            {
                available.Enqueue(CreateNew());
            }
        }

        public ScorePopupView Get()
        {
            ScorePopupView popup = available.Count > 0
                ? available.Dequeue()
                : CreateNew();

            popup.gameObject.SetActive(true);
            return popup;
        }

        public void Release(ScorePopupView popup)
        {
            popup.gameObject.SetActive(false);
            available.Enqueue(popup);
        }

        private ScorePopupView CreateNew()
        {
            // Clona el template que ya existe en la jerarquía
            ScorePopupView popup = Instantiate(template, container);
            popup.gameObject.SetActive(false);
            return popup;
        }
    }
}