using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Addresses;
using Library.Core.Dto.Clients;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AddressService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> AddressExistsAsync(long id)
        {
            Address? address = await unitOfWork.AddressRepository.GetByIdAsync(id);

            return address != null;
        }

        public async Task<bool> AddressIsResidencyAsync(long id)
        {
            bool addressIsMain = await unitOfWork.AddressRepository.AllAsQueryable().AsNoTracking()
                .AnyAsync(a => a.Id == id && a.Type == AddressType.Residency);

            return addressIsMain;
        }

        public async Task<IEnumerable<AddressListDto>> GetClientAddressesAsync(long id)
        {
            IEnumerable<AddressListDto> addresses = mapper.Map<List<AddressListDto>>(await unitOfWork.AddressRepository
                .AllAsQueryable(a => a.ClientId == id)
                .Include(a => a.Country)
                .ToListAsync());

            if (addresses == null)
            {
                throw new NotFoundException($"Client with id {id} was not found!");
            }

            return addresses;
        }

        public AddressCreateDto CreateAddressCreateDtoAsync()
        {
            AddressCreateDto addressCreateDto = new AddressCreateDto()
            {
                CountryId = 1,
                Type = AddressType.Residency
            };

            return addressCreateDto;
        }

        public async Task<long> CreateAddressAsync(long clientId, AddressCreateDto dto)
        {
            Client? client = await unitOfWork.ClientRepository.GetByIdAsync(clientId);

            if (client == null)
            {
                throw new NotFoundException($"Client was with id {clientId} not found!");
            }

            Address newAddress = mapper.Map<Address>(dto);

            newAddress.ClientId = clientId;
            if (newAddress.Type == AddressType.Residency)
            {
                await ChangeOldResidencyAddressAsync(clientId);
            }
            await unitOfWork.AddressRepository.AddAsync(newAddress);
            await unitOfWork.SaveChangesAsync();
            return newAddress.Id;
        }

        public async Task<AddressEditDto> CreateAddressEditDtoAsync(long id)
        {
            AddressEditDto? dto = mapper.Map<AddressEditDto>(await unitOfWork.AddressRepository
                .AllAsQueryable()
                .Where(a => a.Id == id)
                .Include(a => a.Country)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Address with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateAddressAsync(long id, AddressEditDto dto)
        {
            Address? address = await unitOfWork.AddressRepository.AllAsQueryable()
                .Where(a => a.Id == dto.Id)
                .Include(a => a.Client)
                .ThenInclude(cl => cl.Addresses)
                .FirstOrDefaultAsync();

            if (address == null)
            {
                throw new NotFoundException($"Address with id {id} was not found!");
            }

            bool changeOldResidencyAddress = address.Type != AddressType.Residency && dto.Type == AddressType.Residency;

            if (changeOldResidencyAddress)
            {
                await ChangeOldResidencyAddressAsync(address.ClientId);
            }
            address.City = dto.City;
            address.AddressLine = dto.AddressLine;
            address.CountryId = dto.CountryId;
            address.PostalCode = dto.PostalCode;
            address.Type = dto.Type;
            unitOfWork.AddressRepository.Update(address);
            if (address.Client.Addresses.Count(a => a.Type == AddressType.Residency) != 1)
            {
                throw new InvalidOperationException("Client must have exactly one residency address!");
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeClientResidencyAddressAsync(long clientId, long addressId)
        {
            List<Address> clientAddresses = await unitOfWork.AddressRepository.AllAsQueryable()
                .Where(cl => cl.ClientId == clientId)
                .ToListAsync();

            if (!clientAddresses.Any())
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            Address? newResidencyAddress = clientAddresses.FirstOrDefault(cl => cl.Id == addressId);

            if (newResidencyAddress == null)
            {
                throw new NotFoundException($"Address with id {addressId} was not found!");
            }

            if (newResidencyAddress.Type == AddressType.Residency)
            {
                throw new InvalidOperationException("Address is alredy a residency address!");
            }

            await ChangeOldResidencyAddressAsync(clientId);
            newResidencyAddress.Type = AddressType.Residency;

            unitOfWork.AddressRepository.Update(newResidencyAddress);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ClientAddressesEditDto> CreateClientAddressesEditDtoAsync(long clientId)
        {
            ClientAddressesEditDto? dto = mapper.Map<ClientAddressesEditDto>(
                    await unitOfWork.ClientRepository
                    .AllAsQueryable()
                    .AsNoTracking()
                    .Where(cl => cl.Id == clientId)
                    .Include(cl => cl.Addresses)
                    .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            return dto;
        }

        public async Task UpdateClientAddressesAlternateAsync(long clientId, ClientAddressesEditDto dto)
        {
            if (dto.Addresses.Count(a => a.Type == AddressType.Residency) != 1)
            {
                throw new ArgumentException("Client must have exactly one residency address.");
            }

            Client? client = await unitOfWork.ClientRepository
                .AllAsQueryable()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.Addresses)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            foreach (AddressCreateOrEditDto addressDto in dto.Addresses)
            {
                if (addressDto.Id != null)
                {
                    Address? clientAddress = client.Addresses.FirstOrDefault(a => a.Id == addressDto.Id);

                    if (clientAddress == null)
                    {
                        throw new NotFoundException($"Address with id {addressDto.Id} was not found!");
                    }

                    clientAddress.AddressLine = addressDto.AddressLine;
                    clientAddress.City = addressDto.City;
                    clientAddress.Type = addressDto.Type;
                    clientAddress.PostalCode = addressDto.PostalCode;
                    clientAddress.CountryId = addressDto.CountryId;
                }
                else
                {
                    Address? address = new Address()
                    {
                        City = addressDto.City,
                        AddressLine = addressDto.AddressLine,
                        PostalCode = addressDto.PostalCode,
                        Type = addressDto.Type,
                        CountryId = addressDto.CountryId,
                    };

                    client.Addresses.Add(address);
                }
            }

            IEnumerable<Address> archivedAddresses = client.Addresses
                .Where(a => !dto.Addresses.Select(da => da.Id).Contains(a.Id));

            foreach (Address address in archivedAddresses)
            {
                address.Status = AddressStatus.Archived;
            }

            unitOfWork.ClientRepository.Update(client);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ArchiveAddressAsync(long id)
        {
            Address? address = await unitOfWork.AddressRepository.GetByIdAsync(id);

            if (address == null)
            {
                throw new NotFoundException($"Address with id {id} was not found!");
            }

            if (address.Type == AddressType.Residency)
            {
                throw new InvalidOperationException("Cannot archive residency address!");
            }
            address.Status = AddressStatus.Archived;
            unitOfWork.AddressRepository.Update(address);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(long id)
        {
            Address? address = await unitOfWork.AddressRepository.GetByIdAsync(id);

            if (address == null)
            {
                throw new NotFoundException($"Address with id {id} was not found!");
            }

            await unitOfWork.AddressRepository.DeleteAsync(address.Id);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task ChangeOldResidencyAddressAsync(long clientId)
        {
            List<Address> clientAddresses = await unitOfWork.AddressRepository.AllAsQueryable()
                .Where(cl => cl.ClientId == clientId)
                .ToListAsync();

            Address? oldResidencyAddress = clientAddresses.FirstOrDefault(cl => cl.Type == AddressType.Residency);

            if (oldResidencyAddress == null)
            {
                throw new InvalidOperationException("Client must have one residency address!");
            }

            oldResidencyAddress.Type = AddressType.Secondary;

            unitOfWork.AddressRepository.Update(oldResidencyAddress);
        }
    }
}
