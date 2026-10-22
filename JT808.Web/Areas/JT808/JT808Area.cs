using System.ComponentModel;
using NewLife;
using NewLife.Cube;

namespace JT808.Web.Areas.JT808;

[DisplayName("核心管理")]
public class JT808Area : AreaBase
{
    public JT808Area() : base(nameof(JT808Area).TrimSuffix("Area")) { }
}