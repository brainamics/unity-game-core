using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class CameraUtils
    {
        public static void FitToBoundsZ(Camera camera, Bounds bounds, bool adjustPosition = true)
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var boundsAspect = bounds.size.x / bounds.size.z;

            var orthographicSize = bounds.size.y / 2;

            if (boundsAspect > screenAspect)
            {
                orthographicSize = bounds.size.x / (2 * screenAspect);
            }

            camera.orthographicSize = orthographicSize;
            if (adjustPosition)
                camera.transform.position = new Vector3(bounds.center.x, camera.transform.position.y, bounds.center.z);
        }

        public static void FitToBoundsX(Camera camera, Bounds bounds, bool adjustPosition = true)
        {
            var screenAspect = (float)Screen.width / Screen.height;
            var boundsAspect = bounds.size.z / bounds.size.y;

            var orthographicSize = bounds.size.y / 2;

            if (boundsAspect > screenAspect)
            {
                orthographicSize = bounds.size.z / (2 * screenAspect);
            }

            camera.orthographicSize = orthographicSize;
            if (adjustPosition)
                camera.transform.position = new Vector3(bounds.center.x, camera.transform.position.y, bounds.center.z);
        }
    }
}
