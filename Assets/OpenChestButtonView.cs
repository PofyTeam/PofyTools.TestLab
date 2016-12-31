using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.UI;

public class OpenChestButtonView : HUDButtonView
{
	public Chest chest;

	public override void OnPointerDown (UnityEngine.EventSystems.BaseEventData eventData)
	{
		base.OnPointerDown (eventData);
		this.chest.OnRayHit ();
	}
}
