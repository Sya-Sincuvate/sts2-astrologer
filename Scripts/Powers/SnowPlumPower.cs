using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Powers;

[RegisterPower]
public class SnowPlumPower : ModPowerTemplate
{   
    private class Data
	{
		public int etherealCount;
	}
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Astrologer/images/powers/SnowPlumPower.png",
        BigIconPath: "res://Astrologer/images/powers/SnowPlumPower.png"
    );
    protected override object InitInternalData()
    {
        return new Data();
    }
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
	{
		if (card.Owner.Creature == Owner)
		{   
            if (causedByEthereal)
            {
                GetInternalData<Data>().etherealCount++;
            }
            else
            {
                List<CardModel> list = [.. Owner.Player.Character.CardPool.AllCards];
                list.RemoveAll(c => c.Rarity == CardRarity.Token);
                IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(Owner.Player, list, Amount, Owner.Player.RunState.Rng.CombatCardGeneration);
                foreach (CardModel item in distinctForCombat)
                {
                    await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner.Player);
                }
            }
			
		}
	}

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (participants.Contains(Owner))
		{
			Data data = GetInternalData<Data>();
            List<CardModel> list = [.. Owner.Player.Character.CardPool.AllCards];
            list.RemoveAll(c => c.Rarity == CardRarity.Token);
            IEnumerable<CardModel> distinctForCombat = CardFactory.GetDistinctForCombat(Owner.Player, list, Amount * data.etherealCount, Owner.Player.RunState.Rng.CombatCardGeneration);
            foreach (CardModel item in distinctForCombat)
            {
                await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner.Player);
            }
			data.etherealCount = 0;
		}
	}
}