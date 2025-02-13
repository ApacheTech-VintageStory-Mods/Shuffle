namespace ApacheTech.VintageMods.Shuffle.Features.HotbarShuffle.Settings;

/// <summary>
///     Represents the settings for the Hotbar Shuffle feature.
/// </summary>
public class HotbarShuffleSettings : FeatureSettings<HotbarShuffleSettings>
{
    /// <summary>
    ///     Indicates whether the Hotbar Shuffle feature is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///     The minimum slot number for the hotbar shuffle.
    /// </summary>
    public int MinimumSlot { get; set; } = 1;

    /// <summary>
    ///     The maximum slot number for the hotbar shuffle.
    /// </summary>
    public int MaximumSlot { get; set; } = 10;
}