using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class CameraUtils
    {
        public static void FitToBounds(Camera camera, Bounds bounds)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float boundsAspect = bounds.size.x / bounds.size.y;

            // Determine the orthographic size to fit the bounds vertically
            float orthographicSize = bounds.size.y / 2;

            // If the bounds are wider than the screen aspect ratio, adjust orthographic size
            if (boundsAspect > screenAspect)
            {
                orthographicSize = bounds.size.x / (2 * screenAspect);
            }

            camera.orthographicSize = orthographicSize;

            // Adjust camera position to center the bounds in the view
            camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, camera.transform.position.z);
        }
    }
}
