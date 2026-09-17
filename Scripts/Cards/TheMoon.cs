using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Cards;

[RegisterCard(typeof(TarotCardPool))]
public class TheMoon : ModCardTemplate
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Astrologer/images/cards/TheMoon.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        AstrologerKeywords.Tarot
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<Powers.Anchor>()
    ];
    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Power", 1m)
    ];

    public TheMoon() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int count = 4;
        if (Owner.Creature.HasPower<Powers.NewMoon>())
        {
            count = 3;
        }
        else if (Owner.Creature.HasPower<Powers.CrescentMoon>())
        {
            count = 2;
        }
        else if (Owner.Creature.HasPower<Powers.WaningMoon>())
        {
            count = 1;
        }
        else if (Owner.Creature.HasPower<Powers.FullMoon>())
        {
            count = 0;
        }
        while (count > 0)
        {
            count--;
            if (Owner.Creature.HasPower<Powers.Anchor>())
            {
                return;
            }
            if (Owner.Creature.HasPower<Powers.NewMoon>())
            {
                await PowerCmd.Remove<Powers.NewMoon>(Owner.Creature);
                await PowerCmd.Apply<Powers.CrescentMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            else if (Owner.Creature.HasPower<Powers.CrescentMoon>())
            {
                await PowerCmd.Remove<Powers.CrescentMoon>(Owner.Creature);
                await PowerCmd.Apply<Powers.WaningMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            else if (Owner.Creature.HasPower<Powers.WaningMoon>())
            {
                await PowerCmd.Remove<Powers.WaningMoon>(Owner.Creature);
                await PowerCmd.Apply<Powers.FullMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            else if (Owner.Creature.HasPower<Powers.FullMoon>())
            {
                await PowerCmd.Remove<Powers.FullMoon>(Owner.Creature);
                await PowerCmd.Apply<Powers.NewMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
            else
            {
                await PowerCmd.Apply<Powers.NewMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
            }
        }
        await PowerCmd.Apply<Powers.Anchor>(choiceContext, Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1m);
    }
}