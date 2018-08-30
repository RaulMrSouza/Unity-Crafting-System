using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Physics{
    class SimplePhysics{
        /// <summary>
        /// Check if there is any obstruction
		/// by overlaping the detection collider
        /// Author: Raul Souza
        /// </summary>
        /// <param name="col"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static bool CanPlaceItem(Collider col) { 
            Collider[] colliders;
            colliders = null;

            Transform transform = col.gameObject.transform;

            //can't overlap these types
            if (col.GetType() == typeof(MeshCollider) || col.GetType() == typeof(WheelCollider))
                return true;
            else if (col.GetType() == typeof(BoxCollider)){
                BoxCollider collider = (BoxCollider)col;
                
                Vector3 extents = new Vector3(collider.size.x * transform.localScale.x / 2,
                                               collider.size.y * transform.localScale.y / 2,
                                               collider.size.z * transform.localScale.z / 2);

                Vector3 position =
                    new Vector3(transform.position.x + collider.center.x,
                        transform.position.y + collider.center.y,
                        transform.position.z + collider.center.z);

                colliders = UnityEngine.Physics.OverlapBox(position, extents, transform.localRotation);

            }else if (col.GetType() == typeof(CapsuleCollider)){
                CapsuleCollider collider = (CapsuleCollider)col;

                Vector3 position =
                    new Vector3(transform.position.x + collider.center.x,
                        transform.position.y + collider.center.y,
                        transform.position.z + collider.center.z);

                Vector3 point0 = position;
                Vector3 point1 = position;

                float scale;
                if (collider.direction == 0){
                    scale = Mathf.Max(transform.localScale.y, transform.localScale.z);
                    point0.x = point0.x - collider.height * scale / 2;
                    point1.x = point1.x + collider.height * scale / 2;
                }else if (collider.direction == 1){
                    scale = Mathf.Max(transform.localScale.x, transform.localScale.z);
                    point0.y = point0.y - collider.height * scale / 2;
                    point1.y = point1.y + collider.height * scale / 2;
                }else if (collider.direction == 2){
                    scale = Mathf.Max(transform.localScale.y, transform.localScale.x);
                    point0.z = point0.z - collider.height * scale / 2;
                    point1.z = point1.z + collider.height * scale / 2;
                }

                colliders = UnityEngine.Physics.OverlapCapsule(point0, point1, collider.radius);
            }else if (col.GetType() == typeof(SphereCollider)){
                SphereCollider collider = (SphereCollider)col;

                Vector3 position =
                    new Vector3(transform.position.x + collider.center.x,
                        transform.position.y + collider.center.y,
                        transform.position.z + collider.center.z);

                colliders = UnityEngine.Physics.OverlapSphere(position, 
                    collider.radius* Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z));
            }

            foreach (Collider c in colliders){
                if (!c.gameObject.transform.IsChildOf(transform) && !c.isTrigger){
                    return false;
                }
            }
            return true;
        }
    }
}
