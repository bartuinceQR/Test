using System;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _levelsPopupPrefab;

        private void Start()
        {
            LevelManager.Instance.LevelsPopupOpened += DisableThis;
        }

        private void OnDestroy()
        {
            LevelManager.Instance.LevelsPopupOpened -= DisableThis;
        }

        void DisableThis()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (LevelManager.Instance.IsLocked) return;
            LevelManager.Instance.ShowLevelsPopup();
        }
    }
}
