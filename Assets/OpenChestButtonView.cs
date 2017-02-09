using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.UI;
using PofyTools.Sound;
using PofyTools;

public class OpenChestButtonView : ButtonView
{
    #region implemented abstract members of ButtonView

    protected override void OnClick()
    {
        SoundManager.PlayVariation(this.chest.chestHit);
    }

    #endregion

    public Chest chest;
    //    SoundManager.PlayVariation(this.chest.chestHit);
    //    public override void OnPointerDown(UnityEngine.EventSystems.BaseEventData eventData)
    //    {
    //        base.OnPointerDown(eventData);
    //
    //    }
}
