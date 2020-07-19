using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Gameplay.UserInput
{
    public class PlayerInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private PlayerInputAction playerInputAction;

        public event Action<PlayerInputAction> PressedEvent;
        public event Action<PlayerInputAction> ReleasedEvent;

        public void OnPointerDown(PointerEventData eventData)
        {
            PressedEvent?.Invoke(playerInputAction);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ReleasedEvent?.Invoke(playerInputAction);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ReleasedEvent?.Invoke(playerInputAction);
        }
    }
}
