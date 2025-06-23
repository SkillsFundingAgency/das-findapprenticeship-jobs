namespace SFA.DAS.FindApprenticeship.Jobs.Infrastructure.Slack;

internal struct BlockTypes
{
    public const string Header = "header";
    public const string Divider = "divider";
    public const string Section = "section";
}

internal struct TextTypes
{
    public const string Markdown = "mrkdwn";
    public const string PlainText = "plain_text";
}

public record SlackTextBlock(string Type, string Text, bool? Emoji = null);
public record MarkdownText(string Text, bool? Emoji = null) : SlackTextBlock(TextTypes.Markdown, Text, Emoji);
public record PlainText(string Text, bool? Emoji = null) : SlackTextBlock(TextTypes.PlainText, Text, Emoji);

public record Block(string Type);
public record HeaderBlock(SlackTextBlock Text) : Block(BlockTypes.Header);
public record SectionBlock(SlackTextBlock Text) : Block(BlockTypes.Section);
public record DividerBlock() : Block(BlockTypes.Divider);

public record SlackMessage(string Channel, List<Block>? Blocks = null);