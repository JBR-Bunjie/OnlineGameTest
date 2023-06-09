using UnityEngine;

namespace OnlineGameTest.Bit {
    public class GunBitProperties {
        // GameObject Pointer
        private GameObject _bulletPrefab;
        private int _gunBitBulletCurrentMagazine = 45;
        private int _gunBitBulletCurrentStoreNum = 120;
        private const int _gunBitBulletMagazineCapacity = 60;
        private const int _gunBitBulletMaxStoreNum = 600;

        // Bullet Properties
        private int _gunBitBulletSpeed = 2500;
        private float _gunBitBulletMass = 0.5f;
        private int _gunBitBulletDamage = 10;
        private int _gunBitBulletExistingTime = 10; // seconds

        #region Setup public Setter and Getter
        
        public GameObject BulletPrefab {
            get => _bulletPrefab;
            set => _bulletPrefab = value;
        }
        
        public int GunBitBulletCurrentMagazine {
            get => _gunBitBulletCurrentMagazine;
            set => _gunBitBulletCurrentMagazine = value;
        }
        
        public int GunBitBulletCurrentStoreNum {
            get => _gunBitBulletCurrentStoreNum;
            set => _gunBitBulletCurrentStoreNum = value;
        }
        
        public static int GunBitBulletMagazineCapacity => _gunBitBulletMagazineCapacity;
        
        public static int GunBitBulletMaxStoreNum => _gunBitBulletMaxStoreNum;
        
        public int GunBitBulletSpeed {
            get => _gunBitBulletSpeed;
            set => _gunBitBulletSpeed = value;
        }
        
        public float GunBitBulletMass {
            get => _gunBitBulletMass;
            set => _gunBitBulletMass = value;
        }
        
        public int GunBitBulletDamage {
            get => _gunBitBulletDamage;
            set => _gunBitBulletDamage = value;
        }
        
        public int GunBitBulletExistingTime {
            get => _gunBitBulletExistingTime;
            set => _gunBitBulletExistingTime = value;
        }

        #endregion 
    }
}