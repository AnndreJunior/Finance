using Finance.Core.Enums;

namespace Finance.Core.Models;

public class Transaction
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatdAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidOrReceivedAt { get; set; }
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;
    public decimal Amount { get; set; }
    public string UserId { get; set; } = string.Empty;
}
