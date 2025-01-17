using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Brainamics.Core
{
    public static class BoundsUtils
    {
        public static Bounds FillBoundsXY(Bounds bounds, Bounds containerBounds, bool allowScaleUp = true)
        {
            // Ensure the container bounds are valid
            if (containerBounds.size.x <= 0 || containerBounds.size.y <= 0)
            {
                Debug.LogError("Container bounds must have positive size in x and y dimensions.");
                return bounds;
            }

            // Calculate the scale factors for the x and y axes
            var scaleFactors = new Vector3(
                containerBounds.size.x / bounds.size.x,
                containerBounds.size.y / bounds.size.y,
                1.0f // Z axis is ignored
            );

            // Choose the smallest scale factor to ensure bounds fit within containerBounds
            var scaleFactor = Mathf.Min(scaleFactors.x, scaleFactors.y);

            // If scaling up is not allowed, clamp the scale factor to 1.0
            if (!allowScaleUp)
            {
                scaleFactor = Mathf.Min(scaleFactor, 1.0f);
            }

            // Calculate the new size for bounds
            var newSize = new Vector3(bounds.size.x * scaleFactor, bounds.size.y * scaleFactor, bounds.size.z);

            // Create the new bounds with the resized size, keeping the same center
            var scaledBounds = new Bounds(bounds.center, newSize);

            // Ensure the new bounds are fully contained within containerBounds
            var newCenter = scaledBounds.center;

            // Adjust the center to ensure containment (only for x and y axes)
            if (scaledBounds.min.x < containerBounds.min.x)
                newCenter.x += containerBounds.min.x - scaledBounds.min.x;
            if (scaledBounds.max.x > containerBounds.max.x)
                newCenter.x -= scaledBounds.max.x - containerBounds.max.x;

            if (scaledBounds.min.y < containerBounds.min.y)
                newCenter.y += containerBounds.min.y - scaledBounds.min.y;
            if (scaledBounds.max.y > containerBounds.max.y)
                newCenter.y -= scaledBounds.max.y - containerBounds.max.y;

            // Set the adjusted center (z-axis remains unchanged)
            scaledBounds.center = newCenter;

            return scaledBounds;
        }
        
        public static Bounds FillBoundsXZ(Bounds bounds, Bounds containerBounds, bool allowScaleUp = true)
        {
            // Ensure the container bounds are valid
            if (containerBounds.size.x <= 0 || containerBounds.size.z <= 0)
            {
                Debug.LogError("Container bounds must have positive size in x and z dimensions.");
                return bounds;
            }

            // Calculate the scale factors for the x and z axes
            var scaleFactors = new Vector3(
                containerBounds.size.x / bounds.size.x,
                1.0f, // Y axis is ignored
                containerBounds.size.z / bounds.size.z
            );

            // Choose the smallest scale factor to ensure bounds fit within containerBounds
            var scaleFactor = Mathf.Min(scaleFactors.x, scaleFactors.z);

            // If scaling up is not allowed, clamp the scale factor to 1.0
            if (!allowScaleUp)
            {
                scaleFactor = Mathf.Min(scaleFactor, 1.0f);
            }

            // Calculate the new size for bounds
            var newSize = new Vector3(bounds.size.x * scaleFactor, bounds.size.y, bounds.size.z * scaleFactor);

            // Create the new bounds with the resized size, keeping the same center
            var scaledBounds = new Bounds(bounds.center, newSize);

            // Ensure the new bounds are fully contained within containerBounds
            var newCenter = scaledBounds.center;

            // Adjust the center to ensure containment (only for x and z axes)
            if (scaledBounds.min.x < containerBounds.min.x)
                newCenter.x += containerBounds.min.x - scaledBounds.min.x;
            if (scaledBounds.max.x > containerBounds.max.x)
                newCenter.x -= scaledBounds.max.x - containerBounds.max.x;

            if (scaledBounds.min.z < containerBounds.min.z)
                newCenter.z += containerBounds.min.z - scaledBounds.min.z;
            if (scaledBounds.max.z > containerBounds.max.z)
                newCenter.z -= scaledBounds.max.z - containerBounds.max.z;

            // Set the adjusted center (y-axis remains unchanged)
            scaledBounds.center = newCenter;

            return scaledBounds;
        }
        
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
