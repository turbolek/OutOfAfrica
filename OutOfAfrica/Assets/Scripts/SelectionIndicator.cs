using UnityEngine;
using UnityEngine.UI;

public class SelectionIndicator : MonoBehaviour
{
    [SerializeField] private Image _hoverImage;
    [SerializeField] private Image _selectImage;

    public void Hover()
    {
        _hoverImage.enabled = true;
    }

    public void Unhover()
    {
        _hoverImage.enabled = false;
    }

    public void Select()
    {
        _selectImage.enabled = true;
    }

    public void Deselect()
    {
        _selectImage.enabled = false;
    }
}