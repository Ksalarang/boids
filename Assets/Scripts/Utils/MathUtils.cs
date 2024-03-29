﻿using UnityEngine;

namespace Utils {
    public static class MathUtils {
        public static float min(float a, float b, float c) {
            var min = Mathf.Min(a, b);
            return min < c ? min : c;
        }

        /// <summary>
        /// Calculates the angle in degrees between the positive x-axis and a vector <br/>
        /// specified by the x and y components. <br/>
        /// The result is always within the range [0, 360] degrees, where 0 angles corresponds <br/>
        /// to the positive x-axis and the angles increase in counterclockwise direction. <br/>
        /// </summary>
        public static float vectorToAngle(float x, float y) {
            var angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;
            return angle;
        }

        /// <summary>
        /// See the overload with parameters (float, float).
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float vectorToAngle(Vector3 vector) {
            var angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            if (angle < 0f) angle += 360f;
            return angle;
        }

        public static Vector3 angleToVector(float angle) {
            var vector = Vector3.zero;
            angle *= Mathf.Deg2Rad;
            vector.x = Mathf.Cos(angle);
            vector.y = Mathf.Sin(angle);
            return vector;
        }

        /// <summary>
        /// Returns the opposite of the angle that must be in range [0, 360]. 
        /// </summary>
        public static float oppositeAngle(float angle) {
            if (angle < 180f) return angle + 180f;
            return angle - 180f;
        }
    }
}