using Finance.Api.Data;
using Finance.Core.Handlers;
using Finance.Core.Models;
using Finance.Core.Requests.Categories;
using Finance.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    private readonly AppDbContext _context = context;

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            UserId = request.Userid,
            Title = request.Title,
            Description = request.Description
        };
        try
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(category, StatusCodes.Status201Created, "CREATED");
        }
        catch (Exception)
        {
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category is null)
                return new Response<Category?>(null, StatusCodes.Status404NotFound, "CATEGORY_NOT_FOUND");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(category, message: "CATEGORY_UPDATED");
        }
        catch (Exception)
        {
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var query = _context.Categories
            .AsNoTracking()
            .Where(x => x.UserId == request.Userid)
            .OrderBy(x => x.Title);

        var categories = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var count = await query.CountAsync();

        return new PagedResponse<List<Category>?>(categories, count, request.PageNumber, request.PageSize);
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return category is null
                ? new Response<Category?>(null, StatusCodes.Status404NotFound, "CATEGORY_NOT_FOUND")
                : new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (category is null)
                return new Response<Category?>(null, StatusCodes.Status404NotFound, "CATEGORY_NOT_FOUND");

            category.Title = request.Title;
            category.Description = request.Description;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return new Response<Category?>(category, message: "CATEGORY_UPDATED");
        }
        catch (Exception)
        {
            return new Response<Category?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }
}
