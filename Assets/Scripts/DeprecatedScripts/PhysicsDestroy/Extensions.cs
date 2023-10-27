using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fracture.PhysicsDestroy.Extensions {
    public static class Extensions {
        public static Color SetAlpha(this Color color, float value) {
            return new Color(color.r, color.g, color.b, value);
        }
        
        public static ISet<T> ToSet<T>(this IEnumerable<T> iEnumerable) {
            return new HashSet<T>(iEnumerable);
        }
        
        public static T GetOrAddComponent<T>(this Component c) where T : Component {
            return c.gameObject.GetOrAddComponent<T>();
        }
        
        public static Component GetOrAddComponent(this GameObject go, Type componentType) {
            var result = go.GetComponent(componentType);
            return result == null ? go.AddComponent(componentType) : result;
        }
        
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component {
            return GetOrAddComponent(go, typeof(T)) as T;
        }
        
        public static Vector3 Multiply(this Vector3 vectorA, Vector3 vectorB) {
            return Vector3.Scale(vectorA, vectorB);
        }
        
        public static Vector3 Abs(this Vector3 vector) {
            var x = Mathf.Abs(vector.x);
            var y = Mathf.Abs(vector.y);
            var z = Mathf.Abs(vector.z);
            return new Vector3(x, y, z);
        }

        public static Vector3 SetX(this Vector3 vector3, float x) {
            return new Vector3(x, vector3.y, vector3.z);
        }

        public static Vector3 SetY(this Vector3 vector3, float y) {
            return new Vector3(vector3.x, y, vector3.z);
        }

        public static Vector3 SetZ(this Vector3 vector3, float z) {
            return new Vector3(vector3.x, vector3.y, z);
        }
        
        
        public static Bounds GetCompositeMeshBounds(this GameObject go, bool includeInactive = false, bool isSharedMesh = false) {
            // 将包围盒从mesh中分离出来，合并并转移到gameObject上
            Bounds[] bounds = go.GetComponentsInChildren<MeshFilter>(includeInactive:includeInactive)
                .Select(mf => {
                    Mesh mesh = isSharedMesh ? mf.sharedMesh : mf.mesh;
                    Bounds localBound = TransformBounds(mf.transform, go.transform, mesh.bounds);
                    return localBound;
                })
                .Where(b => b.size != Vector3.zero)
                .ToArray();

            if (bounds.Length == 0) return new Bounds();

            if (bounds.Length == 1) return bounds[0];

            Bounds compositeBounds = bounds[0];
            for (var i = 1; i < bounds.Length; i++) compositeBounds.Encapsulate(bounds[i]);
            // Grow the bounds to encapsulate the bounds.

            return compositeBounds;
        }

        private static Bounds TransformBounds(Transform from, Transform to, Bounds bounds) {
            return bounds.GetVertices()
                .Select(bv => TransformPoint(from:from, to:to.transform, vertex:bv))
                .ToBounds();
        }

        // Transform a Vector3 from "localspace" of "Transform from" to "localspace" of "Transform to"
        private static Vector3 TransformPoint(Transform from, Transform to, Vector3 vertex) {
            var world = from.TransformPoint(vertex);
            return to.InverseTransformPoint(world);
        }

        /// <summary>
        /// Return a Vector3 array of the 8 vertices of the bounds Cube.
        /// More About Bounds: https://docs.unity3d.com/ScriptReference/Bounds.html
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private static Vector3[] GetVertices(this Bounds bounds) => new [] {
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z),
            new Vector3(bounds.center.x, bounds.center.y, bounds.center.z) + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z),
        };
        
        private static Bounds ToBounds(this IEnumerable<Vector3> vertices) {
            return vertices.ToArray().ToBounds();
        }
        
        private static Bounds ToBounds(this Vector3[] vertices) {
            var min = Vector3.one * float.MaxValue;
            var max = Vector3.one * float.MinValue;

            // Find the min and max points of the bounds
            // We can reach the goal is because of the special structure of the bounds:
            // default bounds of mesh is "AABB" cube.
            foreach (var currentVertex in vertices) {
                // Returns a vector that is made from the smallest components of two vectors:
                // Vector3 a = new Vector3(1, 2, 3);
                // Vector3 b = new Vector3(4, 3, 2);
                // Min Returns: (1.0f, 2.0f, 2.0f)
                min = Vector3.Min(currentVertex, min);

                // Max is the same thing
                max = Vector3.Max(currentVertex, max);
            }

            return new Bounds((max + min) / 2, max - min);
        }
        
        // private static Vector3 Min(this Vector3 vectorA, Vector3 vectorB) {
        //     return Vector3.Min(vectorA, vectorB);
        // }
        //
        // private static Vector3 Max(this Vector3 vectorA, Vector3 vectorB) {
        //     return Vector3.Max(vectorA, vectorB);
        // }
        
        
        /// <summary>
        /// 计算当前Mesh的体积，以拓展方法为实现方式
        /// 关于拓展方法：https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        /// 关于Mesh体积计算：https://answers.unity.com/questions/52664/how-would-one-calculate-a-3d-mesh-volume-in-unity.html
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static float Volume(this Mesh mesh) {
            float volume = 0;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles; // VAO
            for (int i = 0; i < triangles.Length; i += 3) {
                Vector3 p1 = vertices[triangles[i + 0]];
                Vector3 p2 = vertices[triangles[i + 1]];
                Vector3 p3 = vertices[triangles[i + 2]];
                volume += SignedVolumeOfTriangle(p1, p2, p3);
            }

            return Mathf.Abs(volume);
        }

        private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3) {
            float v321 = p3.x * p2.y * p1.z;
            float v231 = p2.x * p3.y * p1.z;
            float v312 = p3.x * p1.y * p2.z;
            float v132 = p1.x * p3.y * p2.z;
            float v213 = p2.x * p1.y * p3.z;
            float v123 = p1.x * p2.y * p3.z;
            return 1.0f / 6.0f * (-v321 + v231 + v312 - v132 - v213 + v123);
        }
    }
}