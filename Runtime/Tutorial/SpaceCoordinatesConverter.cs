using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class SpaceCoordinatesConverter
    {
        public static SpaceCoordinates To(this SpaceCoordinates coordinates, CoordinateMode mode, Camera camera = null, float depth = -1)
        {
            var position = coordinates.Position;
            if (mode == coordinates.Mode)
                return coordinates;

            switch (mode)
            {
                case CoordinateMode.Screen:
                    return coordinates.Mode switch
                    {
                        CoordinateMode.World => SpaceCoordinates.Screen(ResolveCamera().WorldToScreenPoint(position)),
                        CoordinateMode.Viewport => SpaceCoordinates.Screen(ViewportToScreen(position)),
                        _ => throw new System.NotImplementedException($"Conversion of {coordinates.Mode} coordinates to {mode} is not implemented."),
                    };
                case CoordinateMode.Viewport:
                    return coordinates.Mode switch
                    {
                        CoordinateMode.World => SpaceCoordinates.Viewport(ScreenToViewport(ResolveCamera().WorldToScreenPoint(position))),
                        CoordinateMode.Screen => SpaceCoordinates.Viewport(ScreenToViewport(position)),
                        _ => throw new System.NotImplementedException($"Conversion of {coordinates.Mode} coordinates to {mode} is not implemented."),
                    };
                case CoordinateMode.World:
                    return coordinates.Mode switch
                    {
                        CoordinateMode.Screen => SpaceCoordinates.World(ResolveCamera().ScreenToWorldPoint(GetDepthPosition())),
                        CoordinateMode.Viewport => SpaceCoordinates.World(ResolveCamera().ViewportToWorldPoint(GetDepthPosition())),
                        _ => throw new System.NotImplementedException($"Conversion of {coordinates.Mode} coordinates to {mode} is not implemented."),
                    };
                case CoordinateMode.None:
                    throw new System.InvalidOperationException($"Cannot convert space coordinates to {mode}.");
                default:
                    throw new System.NotImplementedException($"Conversion of space coordinates to {mode} is not implemented.");
            }

            Camera ResolveCamera()
            {
                if (camera == null)
                    camera = Camera.main;
                return camera;
            }

            Vector3 GetDepthPosition()
            {
                return depth >= 0 ? new(position.x, position.y, depth) : position;
            }
        }

        public static SpaceCoordinates ToScreen(this SpaceCoordinates coordinates, Camera camera = null)
            => coordinates.To(CoordinateMode.Screen, camera);

        public static SpaceCoordinates ToViewport(this SpaceCoordinates coordinates, Camera camera = null)
            => coordinates.To(CoordinateMode.Viewport, camera);

        public static SpaceCoordinates ToWorld(this SpaceCoordinates coordinates, Camera camera = null, float depth = -1)
            => coordinates.To(CoordinateMode.World, camera, depth);

        private static Vector2 ViewportToScreen(Vector2 position)
            => new(position.x * Screen.width, position.y * Screen.height);

        private static Vector2 ScreenToViewport(Vector2 position)
            => new(position.x / Screen.width, position.y / Screen.height);
    }
}
