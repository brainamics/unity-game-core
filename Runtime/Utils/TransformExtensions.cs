using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    public static class TransformExtensions
    {
        public static IEnumerable<Transform> EnumerateObjects(this Scene scene)
        {
            return scene.GetRootGameObjects().SelectMany(o => o.transform.EnumerateDescendants());
        }

        public static IEnumerable<Transform> EnumerateChildren(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
                yield return transform.GetChild(i);
        }

        public static IEnumerable<Transform> EnumerateDescendants(this Transform transform, bool includeSelf = true)
        {
            if (includeSelf)
                yield return transform;

            foreach (var child in transform.EnumerateChildren())
            {
                foreach (var grandChild in child.EnumerateDescendants())
                    yield return grandChild;
            }
        }

        public static Rect GetWorldRect(this RectTransform rt, Vector2 scale)
        {
            // Convert the rectangle to world corners and grab the top left
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 topLeft = corners[0];

            // Rescale the size appropriately based on the current Canvas scale
            var scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }

        public static Rect GetWorldRect(this RectTransform rt, Canvas canvas)
        {
            return rt.GetWorldRect(new Vector2(canvas.scaleFactor, canvas.scaleFactor));
        }

        public static Rect GetWorldRect(this RectTransform rt, CanvasScaler s)
        {
            return rt.GetWorldRect(new Vector2(s.scaleFactor, s.scaleFactor));
        }
    }
}
