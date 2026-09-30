using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Characters;

namespace Astrologer.Scripts.Powers;

[RegisterPower]
public class HypnagogiaPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Astrologer/images/powers/HypnagogiaPower.png",
        BigIconPath: "res://Astrologer/images/powers/HypnagogiaPower.png"
    );

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner.Creature == Owner && card is Dazed)
        {
            await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
        } 
        else if (card.Type == CardType.Status && Owner.HasPower<CleanedPower>())
        {
            await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
        }
    }
}