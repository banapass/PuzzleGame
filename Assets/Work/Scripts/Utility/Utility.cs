namespace Utility
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Utility
    {
        public static float GetCameraOrthSize(float _height,float _pixelPerUnit)
        {
            return _height * (1 / _pixelPerUnit) * 0.5f;
        }
    }

}