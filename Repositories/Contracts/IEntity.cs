using System;

namespace Repositories.Contracts
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}