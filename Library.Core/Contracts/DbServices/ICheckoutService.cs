using Library.Core.Dto.Checkouts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface ICheckoutService
    {
        /// <summary>
        /// Checks if <see cref="Checkout"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> CheckoutExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Checkout"/> with <paramref name="id"/>
        /// has it's <see cref="Checkout.Status"/> is <see cref="CheckoutStatus.Returned"/>
        /// or <see cref="CheckoutStatus.Unreturned"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> isFinalized</returns>
        Task<bool> CheckoutIsFinalizedAsync(long id);

        /// <summary>
        /// Returns <see cref="Checkout"/>s which match the given criteria
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="CheckoutListDto"/></returns>
        Task<IEnumerable<CheckoutListDto>> GetFilteredCheckoutsAsync(CheckoutsFilterDto dto);

        /// <summary>
        /// Returns <see cref="Checkout"/>s which match the given criteria
        /// and have <see cref="Checkout.BookId"/> equal to <paramref name="bookId"/>
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="CheckoutListDto"/></returns>
        Task<IEnumerable<CheckoutListDto>> GetBookCheckoutsAsync(long bookId,
            int page = 1,
            int itemsPerPage = 6);

        /// <summary>
        /// Returns <see cref="Checkout"/>s which match the given criteria
        /// and have <see cref="Checkout.ClientCardId"/> equal to <paramref name="clientCard"/>
        /// </summary>
        /// <param name="clientCard"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="CheckoutListDto"/></returns>
        Task<IEnumerable<CheckoutListDto>> GetClientCardCheckoutsAsync(long clientCard,
            int page = 1,
            int itemsPerPage = 6);

        /// <summary>
        /// Returns a <see cref="CheckoutDetailsDto"/> of <see cref="Checkout"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="CheckoutDetailsDto"/> dto</returns>
        Task<CheckoutDetailsDto> GetCheckoutByIdAsync(long id);

        /// <summary>
        /// Creates <see cref="CheckoutCreateDto"/> and returns it
        /// </summary>
        /// <param name="clientCardId"></param>
        /// <returns><see cref="CheckoutCreateDto"/> dto</returns>
        Task<CheckoutCreateDto> CreateCheckoutCreateDtoAsync(long clientCardId);

        /// <summary>
        /// Creates <see cref="Checkout"/> with data from <see cref="CheckoutCreateDto"/>
        /// </summary>
        /// <param name="clientCardId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> <see cref="Checkout.Id"/></returns>
        Task<long> CreateCheckoutAsync(long clientCardId, CheckoutCreateDto dto);

        /// <summary>
        /// Creates a <see cref="CheckoutFinalizationDto"/> for <see cref="Checkout"/>
        /// with given <paramref name="id"/> and returns it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CheckoutFinalizationDto> CreateCheckoutFinalizationDto(long id);

        /// <summary>
        /// Finalizes <see cref="Checkout"/> with <paramref name="id"/>
        /// Sets its <see cref="Checkout.Status"/> property to <see cref="CheckoutStatus.Returned"/>
        /// or <see cref="CheckoutStatus.Unreturned"/> depending on the data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task FinalizeCheckoutAsync(long id, CheckoutFinalizationDto dto);

        /// <summary>
        /// Deletes <see cref="Checkout"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteCheckoutAsync(long id);
    }
}
