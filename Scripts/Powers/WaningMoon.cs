using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Powers;

[RegisterPower]
public class WaningMoon : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Astrologer/images/powers/WaningMoon.png",
        BigIconPath: "res://Astrologer/images/powers/WaningMoon.png"
    );

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
	{
		if (!props.IsPoweredAttack())
		{
			return 1m;
		}
		if (cardSource == null)
		{
			return 1m;
		}
        if (cardSource.Keywords.Contains(AstrologerKeywords.MoonEnhance))
        {
            return 1.5m;
        }
		return 1m;
	}

    public override decimal ModifyBlockMultiplicative(Creature? target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (cardSource == null)
		{
			return 1m;
		}
        if (cardSource.Keywords.Contains(AstrologerKeywords.MoonEnhance))
        {
            return 1.5m;
        }
        return 1m;
    }
}