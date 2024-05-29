using System.ComponentModel.DataAnnotations;
using Finance.Core.Enums;

namespace Finance.Core.Requests.Transactions;

public class CreateTransactionRequest : Request
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public long CategoryId { get; set; }

    [Required]
    public DateTime? PaidOrReceivedAt { get; set; }
}
