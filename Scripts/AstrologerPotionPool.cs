using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts;

public class AstrologerPotionPool : TypeListPotionPoolModel
{
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Astrologer/images/energy.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Astrologer/images/energy.png";

    public override string EnergyColorName => "astrologer";
}