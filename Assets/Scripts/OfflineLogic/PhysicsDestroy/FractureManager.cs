using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Fracture.PhysicsDestroy.Extensions;

namespace Fracture.PhysicsDestroy {
    public class FractureManager : MonoBehaviour {
        private FractureNode[] _nodes;
        
        public void Setup(Rigidbody[] bodies) {
            _nodes = new FractureNode[bodies.Length];
            for (int i = 0; i < bodies.Length; i++) {
                var node = bodies[i].GetOrAddComponent<FractureNode>();
                node.Setup();
                _nodes[i] = node;
            }
        }
        
        private void FixedUpdate() {
            var runSearch = false;
            foreach (var brokenNodes in _nodes.Where(node => node.HasBrokenLinks)) {
                brokenNodes.CleanBrokenLinks();
                runSearch = true;
            }
            
            // Do "SearchGraph"
            if(runSearch)
                SearchGraph(_nodes);
        }

        
        private Color[] colors =
        {
            Color.blue, 
            Color.green, 
            Color.magenta, 
            Color.yellow
        };
        
        /// <summary>
        /// Search the graph from bottom to top, and unfreeze the nodes which has been removed joints.
        /// </summary>
        /// <param name="nodes">what we put in is the whole nodes</param>
        private void SearchGraph(FractureNode[] nodes) {
            List<FractureNode> anchors = nodes.Where(node => node.IsStatic).ToList();

            ISet<FractureNode> search = new HashSet<FractureNode>(nodes);
            
            int index = 0;
            foreach (FractureNode anchor in anchors) {
                if (search.Contains(anchor)) {
                    var subVisited = new HashSet<FractureNode>();
                    
                    Traverse(anchor, search, subVisited);
                    
                    var color = colors[index++ % colors.Length];
                    foreach (var sub in subVisited) {
                        sub.Color = color;
                    }

                    search = search.Where(s => subVisited.Contains(s) == false).ToSet();
                }
            }

            foreach (FractureNode subSearch in search) {
                subSearch.UnFreeze();
                subSearch.Color = Color.black;
            }
        }

        private void Traverse(FractureNode anchor, ISet<FractureNode> search, HashSet<FractureNode> subVisited) {
            if (search.Contains(anchor) && subVisited.Contains(anchor)== false) {
                subVisited.Add(anchor);

                foreach (FractureNode singleNeighbour in anchor.NeighboursArray) {
                    FractureNode neighbour = singleNeighbour;
                    Traverse(neighbour, search, subVisited);
                }
            }
        }
    }
}