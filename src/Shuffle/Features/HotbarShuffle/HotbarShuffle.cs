using ApacheTech.VintageMods.Shuffle.Features.HotbarShuffle.Settings;
using Gantry.Core.Hosting.Registration;
using Gantry.Services.FileSystem.Hosting;

namespace ApacheTech.VintageMods.Shuffle.Features.HotbarShuffle;

public class HotbarShuffle : ClientModSystem, IClientServiceRegistrar
{
    private static HotbarShuffleSettings _settings;

    public void ConfigureClientModServices(IServiceCollection services, ICoreClientAPI capi)
    {
        ApiEx.Logger.VerboseDebug("Registering HotbarShuffle World Settings");
        services.AddFeatureWorldSettings<HotbarShuffleSettings>();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        ApiEx.Logger.VerboseDebug("Starting HotbarShuffle ModSystem");
        _settings = IOC.Services.GetRequiredService<HotbarShuffleSettings>();
        var parsers = api.ChatCommands.Parsers;
        var command = api.ChatCommands
            .Create("shuffle")
            .HandleWith(OnEnableToggle);

        command
            .BeginSubCommand("min")
            .WithArgs(parsers.IntRange("minimum", 1, 10))
            .HandleWith(OnChangeMinumumSlot);

        command
            .BeginSubCommand("max")
            .WithArgs(parsers.IntRange("minimum", 1, 10))
            .HandleWith(OnChangeMaximumSlot);

        api.Input.RegisterHotKey("shuffle", "Hotbar Shuffle", GlKeys.R, HotkeyType.InventoryHotkeys);
        api.Input.SetHotKeyHandler("shuffle", OnHotkeyPressed);
    }

    private bool OnHotkeyPressed(KeyCombination _)
    {
        _settings.Enabled = !_settings.Enabled;
        Capi.ShowChatMessage(T($"Enabled", _settings.Enabled));
        return true;
    }

    private TextCommandResult OnEnableToggle(TextCommandCallingArgs args)
    {
        _settings.Enabled = !_settings.Enabled;
        return TextCommandResult.Success(T($"Enabled", _settings.Enabled));
    }

    private TextCommandResult OnChangeMinumumSlot(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int>();
        if (value > _settings.MaximumSlot)
        {
            return TextCommandResult.Error(T("MinTooHigh"));
        }
        _settings.MinimumSlot = value;
        return TextCommandResult.Success(T($"SetMinimum", _settings.MinimumSlot));
    }

    private TextCommandResult OnChangeMaximumSlot(TextCommandCallingArgs args)
    {
        var value = args.Parsers[0].GetValue().To<int>();
        if (value < _settings.MinimumSlot)
        {
            return TextCommandResult.Error(T("MaxTooLow"));
        }
        _settings.MaximumSlot = value;
        return TextCommandResult.Success(T($"SetMaximum", _settings.MaximumSlot));
    }

    private static string T(string path, params object[] args)
        => LangEx.FeatureString(nameof(HotbarShuffle), path, args);
}