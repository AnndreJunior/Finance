using System.ComponentModel.DataAnnotations;

namespace Finance.Core.Requests.Categories;

public class CreateCategoryRequest : Request
{
    [Required]
    [MaxLength(80)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
}
