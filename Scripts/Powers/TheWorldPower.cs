using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Powers;

[RegisterPower]
public class TheWorldPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Astrologer/images/powers/TheWorldPower.png",
        BigIconPath: "res://Astrologer/images/powers/TheWorldPower.png"
    );
    
    public override bool ShouldTakeExtraTurn(Player player)
	{
		if (Amount > 0)
		{
			return player == Owner.Player;
		}
		return false;
	}

	public override async Task AfterTakingExtraTurn(Player player)
	{
		if (player == Owner.Player)
		{
			await PowerCmd.Decrement(this);
		}
	}
}