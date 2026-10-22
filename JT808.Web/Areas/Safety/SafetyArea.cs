using System.ComponentModel;
using NewLife;
using NewLife.Cube;

namespace JT808.Web.Areas.Safety;

[DisplayName("主动安全")]
public class SafetyArea : AreaBase
{
    public SafetyArea() : base(nameof(SafetyArea).TrimSuffix("Area")) { }
}