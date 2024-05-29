using System.ComponentModel.DataAnnotations;

namespace Finance.Core.Requests.Transactions;

public class GetTransactionByIdRequest : Request
{
    [Required]
    public long Id { get; set; }
}
