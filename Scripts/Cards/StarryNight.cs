using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Cards;

[RegisterCard(typeof(AstrologerCardPool))]
public class StarryNight : ModCardTemplate
{
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    // protected override HashSet<CardTag> CanonicalTags => [
    //     CardTag.Strike
    // ];
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Astrologer/images/cards/StarryNight.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AstrologerKeywords.MoonEnhance,
        CardKeyword.Exhaust
    ];

    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        ModCardVars.Computed(
        "Ast",
        static ctx =>
        {
            if (ctx.Player == null) {
                return 2m;
            }
            if (ctx.Player.Creature.HasPower<Powers.CrescentMoon>() || ctx.Player.Creature.HasPower<Powers.WaningMoon>()) {
                return 3m;
            }
            if (ctx.Player.Creature.HasPower<Powers.FullMoon>()) {
                return 6m;
            }
            return 2m;
        },
        baseValue: 2m)
    ];

    public StarryNight() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.EvaluateValueOrDefault("Ast"), Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}