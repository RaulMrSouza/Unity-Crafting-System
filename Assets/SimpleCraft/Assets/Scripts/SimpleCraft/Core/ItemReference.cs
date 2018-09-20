using SimpleCraft.Physics;
using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// Reference between GameObject and scriptable object
    /// </summary>
	public class ItemReference : MonoBehaviour {

        [SerializeField]
        protected Item _data;
        public virtual Item Data{
            get { return _data; }
            set { _data = value; }
        }

        [SerializeField]
        private Collider _detectionCollider;
        public Collider DetectionCollider{
            get { return _detectionCollider; }
            set { _detectionCollider = value; }
        }

		[SerializeField]
        private float _amount = 1;
		public float Amount{
			get{ return _amount;}
			set{ _amount = value;}
		}

        private bool _isActive = false;
        public bool IsActive{
            get { return _isActive; }
            set { _isActive = value; }
        }

        /// <summary>
        /// Check if there is any obstruction
        /// </summary>
        /// <returns><c>true</c> if this instance can build; otherwise, <c>false</c>.</returns>
        public bool CanBuild(){
            return SimplePhysics.CanPlaceItem(DetectionCollider);
        }

        public void DestroyItem() {
            if (_data.GetType() == typeof(Item))
                return;

            CraftableItem data = _data as CraftableItem;

            foreach (CraftableItem.CraftCost cost in data.GetCraftCost) {
                float amount = ((cost.amount < 1) ? 1 : (cost.amount/2));
                Manager.InstantiateItem(cost.item, transform.position, amount);
            }
            Destroy(gameObject);
        }
    }
}
