using UnityEngine;

namespace Fracture.PhysicsDestroy {
    public class FractureObject : MonoBehaviour {
        [SerializeField] private int _seed = 0;
        
        [SerializeField] private float _chunkDensity = 50.0f;
        [SerializeField] private int _totalChunkNum = 500;

        [SerializeField] private Material _insideMaterial;
        [SerializeField] private Material _outsideMaterial;
                                           
        [SerializeField] private float _jointBreakForce = 100.0f;
        [SerializeField] private Anchor _anchor = Anchor.Bottom;

        private void Start() {
            Fracturing.FractureGameObject(
                gameObject:gameObject,
                anchor:_anchor,
                seed:_seed,
                chunkDensity:_chunkDensity,
                totalChunkNum:_totalChunkNum,
                insideMaterial:_insideMaterial,
                outsideMaterial:_outsideMaterial,
                jointBreakForce:_jointBreakForce
            );
            gameObject.SetActive(false);
        }
    }
}
