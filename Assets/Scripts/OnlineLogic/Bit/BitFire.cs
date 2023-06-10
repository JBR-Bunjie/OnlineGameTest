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
        
        public bool StoreAmmoAlert { get; private set; }
        public bool MagAmmoAlert { get; private set; }
        

        private void Awake() {
            StoreAmmoAlert = false;
            MagAmmoAlert = false;
        }

        // temp values
        private uint _counter = 0;


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
                if (GunBitProperties.GunBitBulletCurrentMagazine <= 0) {
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

            // bullet.transform.rotation = GunBitProperties.BulletPrefab.transform.rotation;
            // bullet.GetComponent<Rigidbody>().AddForce(pointer * GunBitProperties.GunBitSpeed);

            
            // Destroy(bullet, gunBitProperties.GunBitBulletExistingTime);
            StartCoroutine(DestroyBulletObject(bullet));
            
            if (GunBitProperties.GunBitBulletCurrentMagazine <= 0) {
                // if the Current Magazine has no ammo, then try to reload
                GunBitReload();
            }
        }

        private GameObject CreateBulletObject(Vector3 worldInitialPosition, int bulletSortOffset) {
            GameObject bullet = Instantiate(
                RemoteGlobalValues.PrefabPointer[RemoteGlobalValues.ToolPrefabStartIndex + bulletSortOffset],
                position: worldInitialPosition,
                rotation: Quaternion.identity
            );
        
            NetworkServer.Spawn(bullet);
            GunBitProperties.GunBitBulletCurrentMagazine -= 1;
            if (!isClientOnly)
                AmmoReflect();
            
            return bullet;
        }
        
        [TargetRpc]
        public void AmmoReflect() {
            GunBitProperties.GunBitBulletCurrentMagazine -= 1;
        }
        
        IEnumerator DestroyBulletObject(GameObject bullet) {
            yield return new WaitForSeconds(5);
            NetworkServer.Destroy(bullet);
            yield return null;
        }
        
        public void GunBitReload() {
            int reloadBulletNeededNum = GunBitProperties.GunBitBulletMagazineCapacity -
                                        GunBitProperties.GunBitBulletCurrentMagazine;

            // if (GunBitProperties.GunBitBulletCurrentStoreNum == 0) {
            //     AmmoAlert(store:true);
            // }
            // else 
            if (GunBitProperties.GunBitBulletCurrentStoreNum < reloadBulletNeededNum) {
                GunBitProperties.GunBitBulletCurrentMagazine += GunBitProperties.GunBitBulletCurrentStoreNum;
                GunBitProperties.GunBitBulletCurrentStoreNum = 0;
            }
            else {
                GunBitProperties.GunBitBulletCurrentMagazine = GunBitProperties.GunBitBulletMagazineCapacity;
                GunBitProperties.GunBitBulletCurrentStoreNum -= reloadBulletNeededNum;
            }
            
            // ReCheck
            if (GunBitProperties.GunBitBulletCurrentStoreNum <= GunBitProperties.GunBitBulletMagazineCapacity / 2) {
                AmmoAlert(mag: MagAmmoAlert, store: true);
            }
            else {
                AmmoAlert(mag: MagAmmoAlert, store: false);
            }
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
        //         
        //     }
        // }

        private void AmmoAlert(bool mag = false, bool store = false) {
            MagAmmoAlert = mag;
            StoreAmmoAlert = store;
        }

        private uint BulletDynamicGuid() {
            return _counter++;
        }

        public static void PickUpAmmo() { }
    }
}