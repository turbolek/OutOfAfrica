using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils.Extensions
{
    public static class RectTransformExtensions
    {
        public static float GetArea(this RectTransform rectTransform)
        {
            return rectTransform.rect.width * rectTransform.rect.height;
        }
    }
}