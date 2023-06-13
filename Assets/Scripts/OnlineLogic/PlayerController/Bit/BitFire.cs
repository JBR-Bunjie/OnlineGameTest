using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace OnlineGameTest.Bit {
    public class BitFire : NetworkBehaviour {
        // Prepare
        private string PlayerId => SearchLocalInstance.GetPlayerID(gameObject);
        private PlayerManager LocalInstance => SearchLocalInstance.GetPlayerManager(PlayerId);

        // Im
        private bool IsLocalPlayer => LocalInstance.IsLocalPlayer;


        private GunBitProperties GunBitProperties => LocalInstance.GunBitProperties;

        [SyncVar] private int _localMagazineAmmo;
        [SyncVar] private int _localStoreAmmo;
        private object _ammoChangeLock = new object();
        
        public bool StoreAmmoAlert { get; private set; }
        public bool MagAmmoAlert { get; private set; }


        private void Awake() {
            StoreAmmoAlert = false;
            MagAmmoAlert = false;
        }

        // Only run on server side;
        [Command]
        private void GunBitPropertiesDataInit() {
            _localMagazineAmmo = GunBitProperties.GunBitBulletInitialMagazine;
            _localStoreAmmo = GunBitProperties.GunBitBulletInitialStoreNum;
            
            GunBitProperties.GunBitBulletCurrentMagazine = GunBitProperties.GunBitBulletInitialMagazine;
            GunBitProperties.GunBitBulletCurrentStoreNum = GunBitProperties.GunBitBulletInitialStoreNum;
        }

        private void Start() {
            if (IsLocalPlayer) {
                GunBitPropertiesDataInit();
                // AmmoReflect();
            }
        }


        /// <summary>
        /// Notices: Make Sure That Running This Command On Server Side Only
        /// </summary>
        /// <param name="gunBitPointer">the direction which the bullet should go</param>
        /// <param name="worldInitialPosition"></param>
        /// <param name="gunBitProperties"></param>
        public void FireBullet(
            Vector3 worldInitialPosition,
            Vector3 gunBitPointer,
            GunBitProperties gunBitProperties
        ) {
            if (GunBitProperties.GunBitBulletCurrentMagazine <= 10) {
                AmmoAlert(mag: true, store: StoreAmmoAlert);
                if (GunBitProperties.GunBitBulletCurrentMagazine == 0) {
                    GunBitReload();
                    return;
                }
            }
            else {
                AmmoAlert(mag: false, store: StoreAmmoAlert);
            }

            GameObject bullet = CreateBulletObject(worldInitialPosition, 0);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Transform tf = rb.transform; // Transform tf = bullet.GetComponent<Transform>();

            tf.position = worldInitialPosition;
            tf.forward = gunBitPointer;
            rb.velocity = tf.forward * gunBitProperties.GunBitBulletSpeed;
            rb.mass = gunBitProperties.GunBitBulletMass;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            StartCoroutine(DestroyBulletObject(bullet));
        }

        private GameObject CreateBulletObject(Vector3 worldInitialPosition, int bulletSortOffset) {
            GameObject bullet = Instantiate(
                RemoteGlobalValues.PrefabPointer[RemoteGlobalValues.ToolPrefabStartIndex + bulletSortOffset],
                position: worldInitialPosition,
                rotation: Quaternion.identity
            );

            NetworkServer.Spawn(bullet);
            
            lock(_ammoChangeLock)
                GunBitProperties.GunBitBulletCurrentMagazine -= 1;
            
            if (GunBitProperties.GunBitBulletCurrentMagazine <= 0) {
                // if the Current Magazine has no ammo, then try to reload
                GunBitReload();
            }

            _localMagazineAmmo = GunBitProperties.GunBitBulletCurrentMagazine;
            _localStoreAmmo = GunBitProperties.GunBitBulletCurrentStoreNum;

            return bullet;
        }

        IEnumerator DestroyBulletObject(GameObject bullet) {
            yield return new WaitForSeconds(5);
            NetworkServer.Destroy(bullet);
            yield return null;
        }

        public void GunBitReload() {
            int reloadBulletNeededNum = GunBitProperties.GunBitBulletMagazineCapacity -
                                        GunBitProperties.GunBitBulletCurrentMagazine;

            if (GunBitProperties.GunBitBulletCurrentStoreNum < reloadBulletNeededNum) {
                GunBitProperties.GunBitBulletCurrentMagazine += GunBitProperties.GunBitBulletCurrentStoreNum;
                GunBitProperties.GunBitBulletCurrentStoreNum = 0;
            }
            else {
                GunBitProperties.GunBitBulletCurrentMagazine = GunBitProperties.GunBitBulletMagazineCapacity;
                GunBitProperties.GunBitBulletCurrentStoreNum -= reloadBulletNeededNum;
            }

            _localMagazineAmmo = GunBitProperties.GunBitBulletCurrentMagazine;
            _localStoreAmmo = GunBitProperties.GunBitBulletCurrentStoreNum;

            // // ReCheck
            // AmmoAlert(
            //     mag: MagAmmoAlert,
            //     store: GunBitProperties.GunBitBulletCurrentStoreNum <= 
            //            GunBitProperties.GunBitBulletMagazineCapacity / 2
            // );
        }

        // [TargetRpc]
        private void AmmoReflect() {
            GunBitProperties.GunBitBulletCurrentMagazine = _localMagazineAmmo;
            GunBitProperties.GunBitBulletCurrentStoreNum = _localStoreAmmo;
            
            // ReCheck
            AmmoAlert(
                mag: GunBitProperties.GunBitBulletCurrentMagazine <= 10,
                store: GunBitProperties.GunBitBulletCurrentStoreNum <= 
                       GunBitProperties.GunBitBulletMagazineCapacity / 2
            );
        }

        private void AmmoAlert(bool mag = false, bool store = false) {
            MagAmmoAlert = mag;
            StoreAmmoAlert = store;
        }

        public static void PickUpAmmo() { }

        private void Update() {
            AmmoReflect();
        }


        // [ServerCallback]
        // private void OnCollisionEnter(Collision collision) {
        //     // Note: 'col' holds the collision information. If the
        //     // Ball collided with a racket, then:
        //     //   col.gameObject is the racket
        //     //   col.transform.position is the racket's position
        //     //   col.collider is the racket's collider
        //
        //     // did we hit a racket? then we need to calculate the hit factor
        //     if (collision.transform.GetComponent<PlayerManager>()) {
        //         PlayerManager playerManager = collision.transform.GetComponent<PlayerManager>();
        //         
        //         playerManager.HealthUpdate(-10);
        //     }
        // }
    }
}