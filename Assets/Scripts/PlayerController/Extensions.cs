using UnityEngine;

namespace OnlineGameTest {
    public static class Extensions {
        public static Vector2 SquareToCircleRemap(this Vector2 input) {
            Vector2 output = Vector2.zero;

            output.x = input.x * Mathf.Sqrt(1.0f - input.y * input.y / 2.0f);
            output.y = input.y * Mathf.Sqrt(1.0f - input.x * input.x / 2.0f);

            return output;
        }
    }
}