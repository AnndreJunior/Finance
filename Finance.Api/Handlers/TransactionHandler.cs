using Finance.Api.Data;
using Finance.Core.Handlers;
using Finance.Core.Models;
using Finance.Core.Common;
using Finance.Core.Requests.Transactions;
using Finance.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Finance.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    private readonly AppDbContext _context = context;

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = new Transaction
            {
                UserId = request.Userid,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, StatusCodes.Status201Created, "TRANSACTION_CREATED");
        }
        catch
        {
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.Userid);

            if (transaction is null)
                return new Response<Transaction?>(null, StatusCodes.Status404NotFound, "TRANSACTION_NOT_FOUND");

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.Userid);

            return transaction is null
                ? new Response<Transaction?>(null, StatusCodes.Status404NotFound, "TRANSACTION_NOT_FOUND")
                : new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.UtcNow.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(
                null,
                StatusCodes.Status500InternalServerError,
                "UNABLE_SET_STARTS_DATE_OR_ENDS_DATE");
        }
        try
        {
            var query = _context
                .Transactions
                .AsNoTracking()
                .Where(x =>
                    x.PaidOrReceivedAt >= request.StartDate &&
                    x.PaidOrReceivedAt <= request.EndDate &&
                    x.UserId == request.Userid)
                .OrderBy(x => x.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(
                transactions,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request is { Type: Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
            request.Amount *= -1;

        try
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.Userid);

            if (transaction is null)
                return new Response<Transaction?>(null, StatusCodes.Status404NotFound, "TRANSACTION_NOT_FOUND");

            transaction.CategoryId = request.CategoryId;
            transaction.Amount = request.Amount;
            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();

            return new Response<Transaction?>(transaction);
        }
        catch
        {
            return new Response<Transaction?>(null, StatusCodes.Status500InternalServerError, "INTERNAL_SERVER_ERROR");
        }
    }
}
