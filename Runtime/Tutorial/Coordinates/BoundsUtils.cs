using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Brainamics.Core
{
    public static class BoundsUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpaceBounds ToScreen(this SpaceBounds bounds, Camera camera)
            => new(CoordinateMode.Screen, BoundsToScreenRect(bounds.Mode, bounds.Bounds, camera));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect AsRect(this Bounds bounds)
            => new(bounds.center, bounds.size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect BoundsToScreenRect(SpaceBounds bounds, Camera camera)
            => BoundsToScreenRect(bounds.Mode, bounds.Bounds, camera);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect BoundsToScreenRect(Bounds bounds, Camera camera)
            => BoundsToScreenRect(CoordinateMode.World, bounds, camera);

        public static Rect BoundsToScreenRect(CoordinateMode fromMode, Bounds bounds, Camera camera)
        {
            if (camera == null)
                camera = Camera.main;
            var screenRect = new Rect(0, 0, Screen.width - 1, Screen.height - 1);

            switch (fromMode)
            {
                case CoordinateMode.Screen:
                    var min = bounds.min;
                    var max = bounds.max;
                    return Rect.MinMaxRect(min.x, min.y, max.x, max.y);

                case CoordinateMode.World:
                    break;

                default:
                    throw new System.NotImplementedException($"Converting bounds from {fromMode} to Screen is not implemented.");
            }

            float minX = 0, minY = 0, maxX = 0, maxY = 0;
            var xSet = false;
            var ySet = false;

            CheckPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z + bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y + bounds.extents.y, bounds.center.z - bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z + bounds.extents.z));
            CheckPoint(new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, bounds.center.z - bounds.extents.z));

            return Rect.MinMaxRect(minX, minY, maxX, maxY);

            void CheckPoint(Vector3 point)
            {
                var screenPoint = camera.WorldToScreenPoint(point);
                if (screenPoint.z < 0)
                    screenPoint = new Vector3(screenPoint.x, screenPoint.y - screenRect.size.y);
                if (!xSet || screenPoint.x < minX)
                    minX = screenPoint.x;
                if (!ySet || screenPoint.y < minY)
                    minY = screenPoint.y;
                if (!xSet || screenPoint.x > maxX)
                    maxX = screenPoint.x;
                if (!ySet || screenPoint.y > maxY)
                    maxY = screenPoint.y;
                xSet = true;
                ySet = true;
            }
        }

        public static SpaceBounds GetBounds(GameObject obj)
        {
            if (TryGetBounds(obj, out var bounds))
                return bounds;

            throw new System.InvalidOperationException($"Could not get bounds of object '{obj}'.");
        }

        public static bool TryGetBounds(GameObject obj, out SpaceBounds bounds)
        {
            if (TryGetHintBounds(obj, out bounds))
                return true;
            if (TryGetCanvasBounds(obj, out bounds))
                return true;
            if (TryGetWorldBounds(obj, out bounds))
                return true;

            bounds = default;
            return false;
        }

        private static bool TryGetHintBounds(GameObject obj, out SpaceBounds bounds)
        {
            if (!obj.TryGetComponent<IBoundsProvider>(out var hint))
            {
                bounds = default;
                return false;
            }

            bounds = hint.Bounds;
            return true;
        }

        private static bool TryGetCanvasBounds(GameObject obj, out SpaceBounds bounds)
        {
            bounds = default;

            if (obj.transform is not RectTransform rectTransform)
                return false;

            var canvas = obj.GetComponentInParent<Canvas>();
            if (canvas == null)
                return false;

            var rect = rectTransform.GetWorldRect(canvas);
            bounds = new SpaceBounds(CoordinateMode.Screen, rect);
            return true;
        }

        private static bool TryGetWorldBounds(GameObject obj, out SpaceBounds bounds)
        {
            var renderer = obj.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                bounds = new SpaceBounds(CoordinateMode.World, renderer.bounds);
                return true;
            }

            var collider = obj.GetComponentInChildren<Collider>();
            if (collider != null)
            {
                bounds = new SpaceBounds(CoordinateMode.World, collider.bounds);
                return true;
            }

            bounds = default;
            return false;
        }
    }

}
