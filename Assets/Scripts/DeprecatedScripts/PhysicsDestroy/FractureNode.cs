using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fracture.PhysicsDestroy.Extensions;

namespace Fracture.PhysicsDestroy {
    public class FractureNode : MonoBehaviour {
        private Rigidbody _rb;

        private Dictionary<Joint, FractureNode> JointToChunk { get; set; } = new Dictionary<Joint, FractureNode>();
        private Dictionary<FractureNode, Joint> ChunkToJoint { get; set; } = new Dictionary<FractureNode, Joint>();
        private HashSet<FractureNode> Neighbours { get; set; } = new HashSet<FractureNode>();
        public FractureNode[] NeighboursArray { get; set; } = Array.Empty<FractureNode>();

        private bool Contains(FractureNode fractureNode) {
            return Neighbours.Contains(fractureNode);
        }

        private bool Frozen { get; set; }
        private Vector3 _frozenPos;
        private Quaternion _frozenRot;

        public bool IsStatic => _rb.isKinematic;
        public Color Color { get; set; } = Color.black;
        public bool HasBrokenLinks { get; private set; }


        public void Setup() {
            _rb = GetComponent<Rigidbody>();
            Freeze();

            JointToChunk.Clear();
            ChunkToJoint.Clear();

            foreach (var joint in GetComponents<Joint>()) {
                var chunk = joint.connectedBody.GetOrAddComponent<FractureNode>();
                JointToChunk[joint] = chunk;
                ChunkToJoint[chunk] = joint;
            }

            foreach (FractureNode chunkNode in ChunkToJoint.Keys) {
                Neighbours.Add(chunkNode);

                if (chunkNode.Contains(this) == false) {
                    chunkNode.Neighbours.Add(this);
                }
            }

            NeighboursArray = Neighbours.ToArray();
        }

        public void CleanBrokenLinks() {
            var brokenLinks = JointToChunk.Keys.Where(joint => joint == false).ToList();
            foreach (var brokenLink in brokenLinks) {
                FractureNode body = JointToChunk[brokenLink];

                JointToChunk.Remove(brokenLink);
                ChunkToJoint.Remove(body);

                body.Remove(this);
                Neighbours.Remove(body);
            }

            NeighboursArray = Neighbours.ToArray();
            HasBrokenLinks = false;
        }

        private void Remove(FractureNode chunkNode) {
            ChunkToJoint.Remove(chunkNode);
            Neighbours.Remove(chunkNode);
            NeighboursArray = Neighbours.ToArray();
        }

        // OnJointBreak:
        //      Called when a joint attached to the same game object broke.
        //      When a force that is higher than the breakForce of the joint, the joint will break off.
        //      When the joint breaks off, OnJointBreak will be called and the break force applied to the joint will be passed in.
        //      After OnJointBreak the joint will automatically be removed from the game object and deleted.
        private void OnJointBreak(float breakForce) {
            HasBrokenLinks = true;
        }

        public void Freeze() {
            Frozen = true;
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _rb.gameObject.layer = LayerMask.NameToLayer("Frozen");
            _frozenPos = _rb.transform.position;
            _frozenRot = _rb.transform.rotation;
        }

        public void UnFreeze() {
            Frozen = false;
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.None;
            _rb.gameObject.layer = LayerMask.NameToLayer("Ground");
        }


        private void FixedUpdate() {
            if (Frozen) {
                transform.position = _frozenPos;
                transform.rotation = _frozenRot;
            }
        }

        private void OnDrawGizmos()
        {
            var worldCenterOfMass = transform.TransformPoint(transform.GetComponent<Rigidbody>().centerOfMass);
            
            if (IsStatic)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldCenterOfMass, 0.05f);
            }
            else
            {
                Gizmos.color = Color.SetAlpha(0.5f);
                Gizmos.DrawSphere(worldCenterOfMass, 0.1f);
            }
            
            foreach (var joint in JointToChunk.Keys)
            {
                if (joint)
                {
                    var from = transform.TransformPoint(_rb.centerOfMass);
                    var to = joint.connectedBody.transform.TransformPoint(joint.connectedBody.centerOfMass);
                    Gizmos.color = Color;
                    Gizmos.DrawLine(from, to);
                }
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            foreach (var node in Neighbours)
            {
                var mesh = node.GetComponent<MeshFilter>().mesh;
                Gizmos.color = Color.yellow.SetAlpha(.2f);
                Gizmos.DrawMesh(mesh, node.transform.position, node.transform.rotation);
            }
        }
    }
}