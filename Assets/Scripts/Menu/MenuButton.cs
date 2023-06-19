using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject _levelsPopupPrefab;

        public void OnPointerClick(PointerEventData eventData)
        {
            Instantiate(_levelsPopupPrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
