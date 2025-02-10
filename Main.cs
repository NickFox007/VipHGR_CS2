
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Capabilities;
using HGR;
using VipCoreApi;
using static VipCoreApi.IVipCoreApi;

namespace VipHGR;
public class VipHGR : BasePlugin
{
    public override string ModuleAuthor => "Nick Fox";
    public override string ModuleName => "[VIP] HGR";
    public override string ModuleVersion => "2.0";

    private IVipCoreApi? _vip;
    private IHGRApi? _hgr;

    private HooksModule _viphooks;
    private HooksModuleEnd _viphooksend;
    private GrabsModule _vipgrabs;
    private GrabsModuleEnd _vipgrabsend;
    private RopesModule _vipropes;
    private RopesModuleEnd _vipropesend;

    private PluginCapability<IVipCoreApi> PluginVip { get; } = new("vipcore:core");
    private PluginCapability<IHGRApi> PluginHooks { get; } = new("hgr:nfcore");

    

    public override void OnAllPluginsLoaded(bool hotReload)
    {
        _vip = PluginVip.Get();
        _hgr = PluginHooks.Get();

        hgrCount = new int[3][];
        for (int i = 0; i < 3; i++)
            hgrCount[i] = new int[65];

        if (_vip == null || _hgr == null) return;

        _vip.OnCoreReady += OnCoreLoaded;

        if (hotReload)
            OnCoreLoaded();        
    }

    public override void Unload(bool hotReload)
    {
        _hgr.RemHook(HGRHook);
    }

    private int[][] hgrCount; // 0 - Hooks, 1 - Grabs, 2 - Ropes

    private void OnCoreLoaded()
    {
        _viphooks = new HooksModule(_vip);
        _viphooksend = new HooksModuleEnd(_vip);
        _vipgrabs = new GrabsModule(_vip);
        _vipgrabsend = new GrabsModuleEnd(_vip);
        _vipropes = new RopesModule(_vip);
        _vipropesend = new RopesModuleEnd(_vip);

        _vip.RegisterFeature(_viphooks, FeatureType.Hide);
        _vip.RegisterFeature(_viphooksend, FeatureType.Hide);
        _vip.RegisterFeature(_vipgrabs, FeatureType.Hide);
        _vip.RegisterFeature(_vipgrabsend, FeatureType.Hide);
        _vip.RegisterFeature(_vipropes, FeatureType.Hide);
        _vip.RegisterFeature(_vipropesend, FeatureType.Hide);

        _hgr.AddHook(HGRHook, 50);

    }

    private void HGRHook(PlayerHGR info)
    {
        if(info.State() == HGRState.Disabled)
        {
            int i = 0;
            switch(info.Mode())
            {
                case HGRMode.Hook: i = 0; break;
                case HGRMode.Grab: i = 1; break;
                case HGRMode.Rope: i = 2; break;
                default: return;
            }

            if (hgrCount[i][info.Player().Slot] == -1)
                info.Enable();
            else
                if(hgrCount[i][info.Player().Slot] > 0)
                {                    
                    hgrCount[i][info.Player().Slot]--;
                    info.Enable();

                    if (hgrCount[i][info.Player().Slot] == 0)
                        _vip.PrintToChat(info.Player(), Localizer["vip_hgr.expired"]);
                    else
                        _vip.PrintToChat(info.Player(), String.Format(Localizer["vip_hgr.use_count"], hgrCount[i][info.Player().Slot]));
                }
        }
    }
    

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {        
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 65; j++)
                hgrCount[i][j] = 0;
        return HookResult.Continue;
    }


    [GameEventHandler]
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        foreach (var player in Utilities.GetPlayers())
        {
            var slot = player.Slot;

            for(int i = 0; i < 3; i++)
                if (_vip.IsClientVip(player) && _vip.PlayerHasFeature(player, HGR.NamesEnd[i]))
                {
                    var count = _vip.GetFeatureValue<int>(player, HGR.NamesEnd[i]);
                    if (count == -1)
                        hgrCount[i][slot] = -1;
                    else if (hgrCount[i][slot] != -1)
                        hgrCount[i][slot] += count;
                }
        }
        return HookResult.Continue;
    }


    [GameEventHandler]
    public HookResult OnFreezeEnd(EventRoundFreezeEnd @event, GameEventInfo info)
    {
        foreach (var player in Utilities.GetPlayers())
        {
            var slot = player.Slot;

            for (int i = 0; i < 3; i++)
                if (_vip.IsClientVip(player) && _vip.PlayerHasFeature(player, HGR.Names[i]))
                {
                    var count = _vip.GetFeatureValue<int>(player, HGR.Names[i]);
                    if (count == -1)
                        hgrCount[i][slot] = -1;
                    else if (hgrCount[i][slot] != -1)
                        hgrCount[i][slot] += count;
                }
        }
        return HookResult.Continue;
    }

    
}