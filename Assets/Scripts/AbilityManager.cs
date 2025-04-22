using UnityEngine;
using PLAYERTWO.PlatformerProject;

public class AbilityManager : MonoBehaviour
{
    private PlayerInputManager input;

    private void Awake()
    {
        input = GetComponent<PlayerInputManager>();
        LockAllAbilities();
    }

    public void LockAllAbilities()
    {
        input.dashEnabled = false;
        input.stompEnabled = false;
        input.airDiveEnabled = false;
        input.glideEnabled = false;
    }

    public void UnlockDash() => input.dashEnabled = true;
    public void UnlockStomp() => input.stompEnabled = true;
    public void UnlockAirDive() => input.airDiveEnabled = true;
    public void UnlockGlide() => input.glideEnabled = true;
}
