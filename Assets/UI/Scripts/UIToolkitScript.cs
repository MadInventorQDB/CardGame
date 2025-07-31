using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.UI.Scripts
{
    public abstract class UIToolkitScript : MonoBehaviour
    {
        protected VisualElement root;

        protected virtual void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}