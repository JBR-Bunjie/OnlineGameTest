using UnityEngine;

namespace OnlineGameTest {
    public abstract class SpawnPoint {
        [SerializeField] private Transform[] _points;
        private Transform[] Points => _points;

        public Transform GetRandomSpawnPoint() {
            return Points[Random.Range(0, Points.Length)];
        }
    }
}