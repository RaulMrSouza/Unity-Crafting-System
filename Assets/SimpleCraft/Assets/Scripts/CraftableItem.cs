using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCraft{
	/// <summary>
	/// A item that can be crafted by the Player using Resources.
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class CraftableItem : MonoBehaviour {

		[SerializeField] private string _itemName = "";
		public string ItemName{
			get { return _itemName; }
			set { _itemName = value; }
		}

		[SerializeField] private bool _hasRigidBody = false;
		public bool HasRigidBody{
			get { return _hasRigidBody; }
			set { _hasRigidBody = value; }
		}

		[SerializeField] private float _offset = 2.0f;
		public float Offset{
			get { return _offset; }
			set { _offset = value; }
		}

		[Serializable]
		public struct Cost {
			public Resource.Type resource;
			public float amount;
		}

		[SerializeField] private Cost[] _buildingCost;
		public Cost[] BuildingCost{
			get { return _buildingCost; }
			set { _buildingCost = value; }
		}
			
		[SerializeField] private Collider _detectionCollider;
		public Collider DetectionCollider{
			get { return _detectionCollider; }
			set { _detectionCollider = value; }
		}

		/// <summary>
		/// Check if there is any obstruction
		/// by overlaping the detection collider
		/// </summary>
		/// <returns><c>true</c> if this instance can build; otherwise, <c>false</c>.</returns>
		public bool CanBuild(){
			Collider[] colliders;
			colliders = null;

			//can't overlap these types
			if (_detectionCollider.GetType () == typeof(MeshCollider) || _detectionCollider.GetType () == typeof(WheelCollider))
				return true;
			
			else if (_detectionCollider.GetType () == typeof(BoxCollider)) {
				BoxCollider collider = (BoxCollider)_detectionCollider;

				Vector3 extents = new Vector3 (collider.size.x * transform.localScale.x / 2, 
											   collider.size.y * transform.localScale.y / 2,
											   collider.size.z * transform.localScale.z / 2);

				Vector3 position = 
					new Vector3 (transform.position.x + collider.center.x,
						transform.position.y + collider.center.y,
						transform.position.z + collider.center.z);
				
				colliders = Physics.OverlapBox (position, extents);

			} else if (_detectionCollider.GetType () == typeof(CapsuleCollider)) {
				CapsuleCollider collider = (CapsuleCollider)_detectionCollider;

				Vector3 position = 
					new Vector3 (transform.position.x + collider.center.x,
						transform.position.y + collider.center.y,
						transform.position.z + collider.center.z);

				Vector3 point0 = position;
				Vector3 point1 = position;

				if (collider.direction == 0) {
					point0.x = point0.x - collider.height / 2;
					point1.x = point1.x + collider.height / 2;
				}else if (collider.direction == 1) {
					point0.y = point0.y - collider.height / 2;
					point1.y = point1.y + collider.height / 2;
				}else if (collider.direction == 2) {
					point0.z = point0.z - collider.height / 2;
					point1.z = point1.z + collider.height / 2;
				}

				colliders = Physics.OverlapCapsule (point0, point1, collider.radius);

			} else if (_detectionCollider.GetType () == typeof(SphereCollider)) {
				SphereCollider collider = (SphereCollider)_detectionCollider;

				Vector3 position = 
					new Vector3 (transform.position.x + collider.center.x,
						transform.position.y + collider.center.y,
						transform.position.z + collider.center.z);
				colliders = Physics.OverlapSphere (position, collider.radius);
			}

			foreach (Collider c in colliders) {
				if (!c.gameObject.transform.IsChildOf (this.transform) && !c.isTrigger) {
					return false;
				}
			}
			return true;
		}
	}
}