using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VipCoreApi;

namespace VipHGR;

public static class HGR
{
    public static string[] Names = ["Hooks", "Grabs", "Ropes"];
    public static string[] NamesEnd = ["HooksEnd", "GrabsEnd", "RopesEnd"];
}
    

public class HooksModule : VipFeatureBase
{    public override string Feature => HGR.Names[0];
    public HooksModule(IVipCoreApi api) : base(api) { }
}

public class HooksModuleEnd : VipFeatureBase
{
    public override string Feature => HGR.NamesEnd[0];
    public HooksModuleEnd(IVipCoreApi api) : base(api) { }
}

public class GrabsModule : VipFeatureBase
{
    public override string Feature => HGR.Names[1];
    public GrabsModule(IVipCoreApi api) : base(api) { }
}

public class GrabsModuleEnd : VipFeatureBase
{
    public override string Feature => HGR.NamesEnd[1];
    public GrabsModuleEnd(IVipCoreApi api) : base(api) { }
}

public class RopesModule : VipFeatureBase
{
    public override string Feature => HGR.Names[2];
    public RopesModule(IVipCoreApi api) : base(api) { }
}

public class RopesModuleEnd : VipFeatureBase
{
    public override string Feature => HGR.NamesEnd[2];
    public RopesModuleEnd(IVipCoreApi api) : base(api) { }
}
