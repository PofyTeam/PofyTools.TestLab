using PofyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CombatHUD : MonoBehaviour
{
    [SerializeField]
    protected PlayerRuntimeData _playerRuntimeData;

    [SerializeField]
    Image _barHealth, _barStamina, _barFocus;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        this._playerRuntimeData.UpdateStats (Time.deltaTime);

        this._barHealth.fillAmount = this._playerRuntimeData.health.CurrentToMaxRatio;
        this._barStamina.fillAmount = this._playerRuntimeData.stamina.CurrentToMaxRatio;
        this._barFocus.fillAmount = this._playerRuntimeData.focus.CurrentToMaxRatio;
    }
}

[System.Serializable]
public class PlayerRuntimeData
{
    public Range health, stamina, focus;
    public float healthRegenerationRate, staminaRegenerationRate, focusRegenerationRate;

    public void UpdateStats (float deltaTime)
    {
        this.health.current += this.healthRegenerationRate * deltaTime;
        this.stamina.current += this.staminaRegenerationRate * deltaTime;
        this.focus.current += this.focusRegenerationRate * deltaTime;
    }
}