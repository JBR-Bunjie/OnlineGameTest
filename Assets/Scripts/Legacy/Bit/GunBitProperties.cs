using UnityEngine;

namespace OnlineGameTest.LocalLogic.Bit {
    public class GunBitProperties : MonoBehaviour {
        // GameObject Pointer
        [SerializeField] private GameObject _bulletPrefab;
        private static int _gunBitGunBitBulletCurrentMagazine = 45;
        private static int _gunBitBulletGunBitBulletCurrentStoreNumNum = 120;
        private const int _gunBitBulletMagazineCapacity = 60;
        private const int _gunBitBulletMaxStoreNum = 600;

        // Bullet Properties
        [SerializeField] private int _gunBitBulletSpeed = 2500;
        [SerializeField] private float _gunBitBulletMass = 0.5f;
        [SerializeField] private int _gunBitBulletDamage = 10;
        [SerializeField] private int _gunBitBulletExistingTime = 10; // seconds


        
        // Setup public Setter and Getter
        public GameObject BulletPrefab => _bulletPrefab;
        public static int GunBitBulletCurrentMagazine {
            get => _gunBitGunBitBulletCurrentMagazine;
            set => _gunBitGunBitBulletCurrentMagazine = value;
        }

        public static int GunBitBulletCurrentStoreNum {
            get => _gunBitBulletGunBitBulletCurrentStoreNumNum;
            set => _gunBitBulletGunBitBulletCurrentStoreNumNum = value;
        }
        public static int GunBitBulletMagazineCapacity => _gunBitBulletMagazineCapacity;
        public static int GunBitBulletMaxStoreNum => _gunBitBulletMaxStoreNum;

        public int GunBitBulletSpeed => _gunBitBulletSpeed;
        public float GunBitBulletMass => _gunBitBulletMass;
        public int GunBitBulletDamage => _gunBitBulletDamage;
        public int GunBitBulletExistingTime => _gunBitBulletExistingTime;


        // public int 
    }
}