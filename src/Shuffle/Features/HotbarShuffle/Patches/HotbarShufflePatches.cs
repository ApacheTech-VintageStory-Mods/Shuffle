using ApacheTech.VintageMods.Shuffle.Features.HotbarShuffle.Settings;
using Gantry.Core.Extensions.Helpers;
using Gantry.Services.FileSystem.Configuration.Consumers;

namespace ApacheTech.VintageMods.Shuffle.Features.HotbarShuffle.Patches;

/// <summary>
///     Contains patches for the Hotbar Shuffle feature.
/// </summary>
/// <seealso cref="WorldSettingsConsumer{HotbarShuffleSettings}" />
[HarmonyClientSidePatch]
public class HotbarShufflePatches : WorldSettingsConsumer<HotbarShuffleSettings>
{
    /// <summary>
    ///     Postfix patch for the OnBlockBuild method in SystemMouseInWorldInteractions.
    ///     Shuffles the active hotbar slot if the Hotbar Shuffle feature is enabled and the player is not sprinting.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SystemMouseInWorldInteractions), "OnBlockBuild")]
    public static void Patch_SystemMouseInWorldInteractions_OnBlockBuild_Postfix()
    {
        if (!Settings.Enabled) return;
        var player = ApiEx.Client.World.Player;
        if (player.Entity.Controls.Sprint) return;
        player.InventoryManager.ActiveHotbarSlotNumber = (int)RandomEx.RandomValueBetween(Settings.MinimumSlot - 1, Settings.MaximumSlot);
    }
}
