using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Cards;

[RegisterCard(typeof(AstrologerCardPool))]
public class MineralBlasting : ModCardTemplate
{
    private const int energyCost = 1;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Astrologer/images/cards/MineralBlasting.png"
    );
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<Dazed>()
    ];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Ast", 8m),
        ModCardVars.ComputedDamage(
            "ComputedDamage",
            6m,
            (card, target) => {
                int sum = 0;
                foreach (CardModel item in PileType.Exhaust.GetPile(card.Owner).Cards) {
                    if (item is Dazed) {
                        sum += card.DynamicVars["Ast"].IntValue;
                    }
                }
                return card.DynamicVars["ComputedDamage"].BaseValue + sum;
            })
    ];

    public MineralBlasting() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.EvaluateValueOrDefault("ComputedDamage", target: cardPlay.Target))
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target!)
            .Execute(choiceContext);
        List<CardModel> list = [.. PileType.Exhaust.GetPile(Owner).Cards];
        foreach (CardModel item in list)
        {
            if (item is Dazed)
            {
                await CardPileCmd.RemoveFromCombat(item);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Ast"].UpgradeValueBy(4m);
    }
}