using UnityEngine;

public class Hands : ItemSlot {
	
	public void InvokeUse(){
		
		if(HeldItem != null){
				
				HeldItem.Use();
		}
	}
	
	public void InvokeDrop(){
		
		Drop();
	}
	
	void OnTriggerEnter2D(Collider2D other){
		
		if(HeldItem != null)
			return;
		
		Item item = other.GetComponent<Item>();
		
		if(item != null){
			
			PickUp(item);
		}
	}
	
	protected override void OnPickup(Item item){
		
		item.transform.localPosition = new Vector2(0.8f, 0);
	}
}
