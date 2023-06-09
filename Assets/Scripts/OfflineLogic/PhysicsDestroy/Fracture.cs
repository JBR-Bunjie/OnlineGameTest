using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// We Add Extension Methods Here
using Fracture.PhysicsDestroy.Extensions;

namespace Fracture.PhysicsDestroy {
    public static class Fracturing {
        public static FractureManager FractureGameObject(
            GameObject gameObject,
            Anchor anchor,
            int seed,
            float chunkDensity,
            int totalChunkNum,
            Material insideMaterial,
            Material outsideMaterial,
            float jointBreakForce
        ) {
            // 1. Get the mesh from the game object
            MeshFilter[] originalMeshFilter = gameObject.GetComponentsInChildren<MeshFilter>();

            // 2. Combine the sub-mesh from original mesh
            CombineInstance[] combineInstances = originalMeshFilter
                .Where(mf => ValidCheck(mf.mesh))
                .Select(mf => new CombineInstance() {
                    mesh = mf.mesh,
                    transform = mf.transform.localToWorldMatrix
                }).ToArray();
            Mesh totalMesh = new Mesh();
            totalMesh.CombineMeshes(combineInstances, true);

            // 3. Use NVMesh to split chunks
            NvBlastExtUnity.setSeed(seed);
            var nvMesh = new NvMesh(
                positions: totalMesh.vertices,
                normals: totalMesh.normals,
                uv: totalMesh.uv,
                verticesCount: totalMesh.vertexCount,
                indices: totalMesh.GetIndices(0),
                indicesCount: (int)totalMesh.GetIndexCount(0)
            );

            List<Mesh> chunkMeshes = FractureMeshesInNvblast(
                totalChunks: totalChunkNum,
                nvMesh: nvMesh
            );

            // 4. Get Chunk properties
            float chunkMass = totalMesh.Volume() * chunkDensity / totalChunkNum;


            // 5. Build New GameObjects
            List<GameObject> chunks = chunkMeshes.Select((thisChunkMesh, index) => {
                // we need to add:
                // - rigidbody,
                // - mesh collider
                // - mesh filer
                // - material(through mesh renderer)
                // - (not here, do this later) fixed joint
                GameObject chunk = CreateSingleChunk(
                    insideMaterial: insideMaterial,
                    outsideMaterial: outsideMaterial,
                    mesh: thisChunkMesh,
                    meshMass: chunkMass
                );
                chunk.name += index.ToString();

                return chunk;
            }).ToList();

            // 6. Transverse all the chunks to:
            // - set parent
            // - Connect all the chunks through fixed joint
            GameObject fractureObject = new GameObject("FractureObject");
            // 6.1
            foreach (var chunk in chunks) {
                chunk.transform.SetParent(fractureObject.transform, false);
            }

            // 6.2
            foreach (var chunk in chunks) {
                ConnectChunks(chunk, jointBreakForce);
            }

            // 7. Set anchored chunks as kinematic
            AnchorChunks(gameObject, anchor);

            // // 8. Setup FractureManager to freeze/unfreeze blocks depending on whether they are connected to the graph or not
            FractureManager graphManager = fractureObject.AddComponent<FractureManager>();
            graphManager.Setup(fractureObject.GetComponentsInChildren<Rigidbody>());
            
            return graphManager;
        }

        private static void AnchorChunks(GameObject gameObject, Anchor anchor) {
            Transform transform = gameObject.GetComponent<Transform>();
            Bounds bounds = gameObject.GetCompositeMeshBounds();
            IEnumerable<Collider> anchoredColliders = GetAnchoredColliders(
                anchor: anchor,
                meshTransform: transform,
                meshBounds: bounds
            );

            foreach (var collider in anchoredColliders) {
                Rigidbody chunkRigidbody = collider.GetComponent<Rigidbody>();
                if (chunkRigidbody != null) {
                    chunkRigidbody.isKinematic = true;
                }
            }
        }

        private static IEnumerable<Collider> GetAnchoredColliders(
            Anchor anchor,
            Transform meshTransform,
            Bounds meshBounds
        ) {
            var anchoredChunks = new HashSet<Collider>();
            var frameWidth = 0.01f;
            var meshWorldCenter = meshTransform.TransformPoint(meshBounds.center);
            var meshWorldExtents = meshBounds.extents.Multiply(meshTransform.lossyScale);

            if (anchor.HasFlag(Anchor.Left)) {
                var center = meshWorldCenter - meshTransform.right * meshWorldExtents.x;
                var halfExtents = meshWorldExtents.Abs().SetX(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            if (anchor.HasFlag(Anchor.Right)) {
                var center = meshWorldCenter + meshTransform.right * meshWorldExtents.x;
                var halfExtents = meshWorldExtents.Abs().SetX(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            if (anchor.HasFlag(Anchor.Bottom)) {
                var center = meshWorldCenter - meshTransform.up * meshWorldExtents.y;
                var halfExtents = meshWorldExtents.Abs().SetY(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            if (anchor.HasFlag(Anchor.Top)) {
                var center = meshWorldCenter + meshTransform.up * meshWorldExtents.y;
                var halfExtents = meshWorldExtents.Abs().SetY(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            if (anchor.HasFlag(Anchor.Front)) {
                var center = meshWorldCenter - meshTransform.forward * meshWorldExtents.z;
                var halfExtents = meshWorldExtents.Abs().SetZ(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            if (anchor.HasFlag(Anchor.Back)) {
                var center = meshWorldCenter + meshTransform.forward * meshWorldExtents.z;
                var halfExtents = meshWorldExtents.Abs().SetZ(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }

            return anchoredChunks;
        }


        private static void ConnectChunks(GameObject chunk, float jointBreakForce, float overlapThreshold = 0.01f) {
            // Rigidbody rb = chunk.GetComponent<Rigidbody>();
            // Mesh mesh  = chunk.GetComponent<MeshFilter>().mesh;
            //
            // Vector3[] vertices = mesh.vertices;
            // foreach (var vertex in vertices) {
            //     Vector3 worldPosition = chunk.transform.TransformPoint(vertex);
            //     Collider[] overlapResults = Physics.OverlapSphere(worldPosition, overlapThreshold);
            //     foreach (Collider overlapResult in overlapResults) {
            //         GameObject colliderObject = overlapResult.transform.gameObject;
            //         if (colliderObject != chunk) {
            //             FixedJoint fixedJoint = colliderObject.AddComponent<FixedJoint>();
            //
            //             fixedJoint.connectedBody = rb;
            //             fixedJoint.breakForce = jointBreakForce;
            //         }
            //     }
            // }


            // 使用Hashset来避免重复计算：因为我们是依据顶点来计算的，但是最终结果却依赖于整个chunk，所以有必要去重，否则会因为计算量过大死机
            var rb = chunk.GetComponent<Rigidbody>();
            var mesh = chunk.GetComponent<MeshFilter>().mesh;
            var overlaps = new HashSet<Rigidbody>();
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++) {
                var worldPosition = chunk.transform.TransformPoint(vertices[i]);
                var hits = Physics.OverlapSphere(worldPosition, overlapThreshold);
                for (var j = 0; j < hits.Length; j++) {
                    overlaps.Add(hits[j].GetComponent<Rigidbody>());
                }
            }

            foreach (var overlap in overlaps) {
                if (overlap.gameObject != chunk.gameObject) {
                    var joint = overlap.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = rb;
                    joint.breakForce = jointBreakForce;
                }
            }
        }

        private static GameObject CreateSingleChunk(
            Material outsideMaterial,
            Material insideMaterial,
            Mesh mesh,
            float meshMass
        ) {
            GameObject chunk = new GameObject("Chunk");

            MeshFilter meshFilter = chunk.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            Rigidbody rigidbody = chunk.AddComponent<Rigidbody>();
            rigidbody.mass = meshMass;

            MeshCollider meshCollider = chunk.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            MeshRenderer meshRenderer = chunk.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterials = new[] {
                outsideMaterial,
                insideMaterial,
            };

            return chunk;
        }

        private static bool ValidCheck(Mesh mesh) {
            bool result = true;

            if (!mesh.isReadable) {
                Debug.LogError($"Mesh Is Not Readable in {mesh}");
                result = false;
            }
            else if (mesh.vertexCount == 0 || mesh.vertexCount % 3 != 0) {
                Debug.LogError($"Bad Vertex Info in {mesh}");
                result = false;
            }
            else if (mesh.uv == null || mesh.uv.Length == 0) {
                Debug.LogError($"Bad UV in {mesh}");
                result = false;
            }

            return result;
        }

        private static List<Mesh> FractureMeshesInNvblast(int totalChunks, NvMesh nvMesh) {
            var fractureTool = new NvFractureTool();
            fractureTool.setRemoveIslands(false);
            fractureTool.setSourceMesh(nvMesh);
            var sites = new NvVoronoiSitesGenerator(nvMesh);
            sites.uniformlyGenerateSitesInMesh(totalChunks);
            fractureTool.voronoiFracturing(0, sites);
            fractureTool.finalizeFracturing();

            // Extract meshes
            var meshCount = fractureTool.getChunkCount();
            var meshes = new List<Mesh>(meshCount);
            for (var i = 1; i < meshCount; i++) {
                NvMesh outsideMesh = fractureTool.getChunkMesh(i, false);
                NvMesh insideMesh = fractureTool.getChunkMesh(i, true);

                Mesh thisChunkMesh = outsideMesh.toUnityMesh();
                thisChunkMesh.subMeshCount = 2;
                thisChunkMesh.SetIndices(insideMesh.getIndexes(), MeshTopology.Triangles, 1);

                meshes.Add(thisChunkMesh);
            }

            return meshes;
        }
    }
}