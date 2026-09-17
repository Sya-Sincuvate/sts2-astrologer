using Astrologer.Scripts.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Astrologer.Scripts.Relics;

// 注册遗物。如果要写自定义池看添加人物的开头
[RegisterRelic(typeof(AstrologerRelicPool))]
[RegisterCharacterStarterRelic(typeof(AstrologerCharacter))] // 注册起始遗物
public class MoonPhaseIndicatorDisc : ModRelicTemplate
{
    // 稀有度
    public override RelicRarity Rarity => RelicRarity.Starter;

    // 遗物的数值。这里会替换本地化中的{Cards}。
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<Cards.AnchorMoon>()
    ];

    public override RelicAssetProfile AssetProfile => new(
        // 小图标（原版85x85）
        IconPath: $"res://Astrologer/images/relics/MoonPhaseIndicatorDisc.png",
        // 轮廓图标（原版85x85）
        IconOutlinePath: $"res://Astrologer/images/relics/MoonPhaseIndicatorDisc.png",
        // 大图标（原版256x256）
        BigIconPath: $"res://Astrologer/images/relics/MoonPhaseIndicatorDisc.png"
    );

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        //战斗开始时, 将一张月相锚定加入手牌
        if (player == Owner && Owner.PlayerCombatState.TurnNumber == 1)
        {
            List<CardModel> list = [];
            for (int i = 0; i < DynamicVars.Cards.IntValue; i++)
            {
                list.Add(Owner.Creature.CombatState.CreateCard<Cards.AnchorMoon>(Owner));
            }
            await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, Owner);
        }

        // 每回合开始时进行月相变换
        // 月相按照新月→上弦月→下弦月→满月→新月循环
        if (Owner.Creature.HasPower<Anchor>())
        {
            return;
        }
        if (Owner.Creature.HasPower<NewMoon>())
        {
            await PowerCmd.Remove<NewMoon>(Owner.Creature);
            await PowerCmd.Apply<CrescentMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
        else if (Owner.Creature.HasPower<CrescentMoon>())
        {
            await PowerCmd.Remove<CrescentMoon>(Owner.Creature);
            await PowerCmd.Apply<WaningMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
        else if (Owner.Creature.HasPower<WaningMoon>())
        {
            await PowerCmd.Remove<WaningMoon>(Owner.Creature);
            await PowerCmd.Apply<FullMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
        else if (Owner.Creature.HasPower<FullMoon>())
        {
            await PowerCmd.Remove<FullMoon>(Owner.Creature);
            await PowerCmd.Apply<NewMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
        else
        {
            await PowerCmd.Apply<NewMoon>(choiceContext, Owner.Creature, 1, Owner.Creature, null);
        }
    }
}