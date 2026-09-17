using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Astrologer.Scripts;

[RegisterOwnedCardKeyword(nameof(MoonEnhance), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Tarot), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Transient), CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
// [RegisterOwnedCardKeyword(nameof(Unique2), IconPath = "res://icon.svg")] // 如果要加更多关键词，添加特性
// 由于写法和ritsulib标准不同，这里不能用static静态类！！
public class AstrologerKeywords
{
    public static readonly CardKeyword MoonEnhance = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(MoonEnhance)).GetModCardKeyword();
    public static readonly CardKeyword Tarot = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Tarot)).GetModCardKeyword();
    public static readonly CardKeyword Transient = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Transient)).GetModCardKeyword();
    // public static readonly CardKeyword Unique2 = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Unique2)).GetModCardKeyword();
}