using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public interface ISchemaClient
    {
        Task<IOperationResult<IFarmList>> FarmListAsync(
            CancellationToken cancellationToken = default);

        Task<IOperationResult<IFarmList>> FarmListAsync(
            FarmListOperation operation,
            CancellationToken cancellationToken = default);

        Task<IOperationResult<IAnimalList>> AnimalListAsync(
            CancellationToken cancellationToken = default);

        Task<IOperationResult<IAnimalList>> AnimalListAsync(
            AnimalListOperation operation,
            CancellationToken cancellationToken = default);
    }
}
