using System.ComponentModel.DataAnnotations;

namespace Finance.Core.Requests.Categories;

public class DeleteCategoryRequest : Request
{
    [Required]
    public long Id { get; set; }
}
