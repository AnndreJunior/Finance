using System.ComponentModel.DataAnnotations;

namespace Finance.Core.Requests.Transactions;

public class GetTransactionByIdRequest
{
    [Required]
    public long Id { get; set; }
}
