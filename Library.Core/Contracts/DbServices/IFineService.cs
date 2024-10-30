using Library.Core.Dto.Fines;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IFineService
    {
        /// <summary>
        /// Checks if <see cref="Fine"/> with given <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> FineExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Fine"/> has <see cref="Fine.Status"/> equal to <see cref="FineStatus.Unpaid"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> isPaid</returns>
        Task<bool> FineIsUnpaidAsync(long id);

        /// <summary>
        /// Checks if <see cref="Checkout"/> with <paramref name="checkoutId"/>
        /// has an unpaid <see cref="Fine"/>
        /// </summary>
        /// <param name="checkoutId"></param>
        /// <returns><see cref="bool"/> hasUnpaidFine</returns>
        Task<bool> CheckoutHasUnpaidFineAsync(long checkoutId);

        /// <summary>
        /// Checks if <see cref="Checkout"/> with <paramref name="checkoutId"/>
        /// has a <see cref="Fine"/> with <see cref="Fine.Reason"/> equal to <paramref name="fineReason"/>
        /// </summary>
        /// <param name="checkoutId"></param>
        /// <param name="fineReason"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> CheckoutHasSameFineAsync(long checkoutId, FineReason fineReason);

        /// <summary>
        /// Returns all <see cref="Client"/> as <see cref="IEnumerable{T}"/> of <see cref="FineListDto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="FineListDto"/></returns>
        Task<IEnumerable<FineListDto>> GetClientFinesAsync(long clientId);

        /// <summary>
        /// Returns all <see cref="Fine"/> with <see cref="Fine.Code"/> equal to <paramref name="code"/>
        /// </summary>
        /// <param name="code"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="FineListDto"/></returns>
        Task<IEnumerable<FineListDto>> GetFineHistoryByCodeAsync(string code);

        /// <summary>
        /// Creates <see cref="FineCreateDto"/> for <see cref="Checkout"/>
        /// with given <paramref name="checkoutId"/>
        /// </summary>
        /// <param name="checkoutId"></param>
        /// <returns><see cref="FineCreateDto"/> dto</returns>
        Task<FineCreateDto> CreateFineCreateDtoAsync(long checkoutId);

        /// <summary>
        /// Creates a <see cref="Fine"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="Fine.Id"/></returns>
        Task<long> CreateFineAsync(FineCreateDto dto);

        /// <summary>
        /// Creates a <see cref="FinePaymentDto"/> for <see cref="Fine"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="FinePaymentDto"/> dto</returns>
        Task<FinePaymentDto> CreateFinePaymentDtoAsync(long id);

        /// <summary>
        /// Updates a <see cref="Fine"/> with <paramref name="id"/>
        /// <see cref="Fine.Status"/> to <see cref="FineStatus.Paid"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task PayFineAsync(long id, FinePaymentDto dto);

        /// <summary>
        /// Creates a <see cref="FineAdjustmentDto"/> for <see cref="Fine"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="FineAdjustmentDto"/> dto</returns>
        Task<FineAdjustmentDto> CreateFineAdjustmentDtoAsync(long id);

        /// <summary>
        /// Changes the <see cref="Fine.Amount"/> of a <see cref="Fine"/>,
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AdjustFineAsync(long id, FineAdjustmentDto dto);

        /// <summary>
        /// Creates a <see cref="FineWaiverDto"/> for <see cref="Fine"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="FineWaiverDto"/> dto</returns>
        Task<FineWaiverDto> CreateFineWaiveDtoAsync(long id);

        /// <summary>
        /// Changes a <see cref="Fine"/>, with <paramref name="id"/>,
        /// <see cref="Fine.Status"/> to <see cref="FineStatus.Waived"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task WaiveFineAsync(long id, FineWaiverDto dto);
    }
}
