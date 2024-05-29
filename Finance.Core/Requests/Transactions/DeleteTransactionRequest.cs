using System.ComponentModel.DataAnnotations;

namespace Finance.Core.Requests.Transactions;

public class DeleteTransactionRequest : Request
{
    [Required]
    public long Id { get; set; }
}
