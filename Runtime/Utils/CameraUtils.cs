using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class CameraUtils
    {
        public static void FitToBounds(Camera camera, Bounds bounds)
        {
            var screenAspect = (float)Screen.width / (float)Screen.height;
            var boundsAspect = bounds.size.x / bounds.size.y;

            var orthographicSize = bounds.size.y / 2;

            if (boundsAspect > screenAspect)
            {
                orthographicSize = bounds.size.x / (2 * screenAspect);
            }

            camera.orthographicSize = orthographicSize;
            camera.transform.position = new Vector3(bounds.center.x, camera.transform.position.y, bounds.center.z);
        }
    }
}
