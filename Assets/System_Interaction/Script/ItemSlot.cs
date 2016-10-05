using UnityEngine;

public class ItemSlot : MonoBehaviour {

	public Item HeldItem;
	
	private void Start(){
		
		if(HeldItem != null){
			
			HeldItem.transform.parent = transform;
			HeldItem.PickedUp();
		}
	}
	
	public void PickUp(Item item){
	
		if(HeldItem != null)
			Drop();
		
		HeldItem = item;
		item.transform.SetParent(transform, true);
		item.PickedUp();
		
		OnPickup(item);
	}
	
	public void Drop(){
		
		HeldItem.transform.parent = null;
		HeldItem.Dropped();
		HeldItem = null;
	}
	
	protected virtual void OnPickup(Item item){}
}
