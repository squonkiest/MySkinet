using Skinet.API.DTOs;
using Skinet.Core.Entities;

namespace Skinet.API.Extensions
{
    public static class AddressMapingExtensions
    {
        public static AddressDto? ToDto(this Address? address)
        {
            if (address == null)
            {
                return null;
            }

            return new AddressDto
            {
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                Country = address.Country,
                PostalCode = address.PostalCode,
                State = address.State
            };
        }

        public static Address ToEntity(this AddressDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            return new Address
            {
                Line1 = dto.Line1,
                Line2 = dto.Line2,
                City = dto.City,
                Country = dto.Country,
                PostalCode = dto.PostalCode,
                State = dto.State
            };
        }

        public static void UpdateFromDto(this Address address, AddressDto dto)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            address.Line1 = dto.Line1;
            address.Line2 = dto.Line2;
            address.City = dto.City;
            address.Country = dto.Country;
            address.PostalCode = dto.PostalCode;
            address.State = dto.State;
        }
    }
}
