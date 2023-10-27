using System;

namespace InputDetector {
    public static class InputExtensions {
        public static T ToEnum<T>(this string keyName) {
            Type enumType = typeof(T);
            return (T)Enum.Parse(enumType, keyName);
        }
    }
}