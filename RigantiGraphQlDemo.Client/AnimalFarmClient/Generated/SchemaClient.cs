using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StrawberryShake;

namespace RigantiGraphQlDemo.Client
{
    public class SchemaClient
        : ISchemaClient
    {
        private const string _clientName = "schemaClient";

        private readonly IOperationExecutor _executor;

        public SchemaClient(IOperationExecutorPool executorPool)
        {
            _executor = executorPool.CreateExecutor(_clientName);
        }

        public Task<IOperationResult<IFarmList>> FarmListAsync(
            CancellationToken cancellationToken = default)
        {

            return _executor.ExecuteAsync(
                new FarmListOperation(),
                cancellationToken);
        }

        public Task<IOperationResult<IFarmList>> FarmListAsync(
            FarmListOperation operation,
            CancellationToken cancellationToken = default)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return _executor.ExecuteAsync(operation, cancellationToken);
        }

        public Task<IOperationResult<IAnimalList>> AnimalListAsync(
            CancellationToken cancellationToken = default)
        {

            return _executor.ExecuteAsync(
                new AnimalListOperation(),
                cancellationToken);
        }

        public Task<IOperationResult<IAnimalList>> AnimalListAsync(
            AnimalListOperation operation,
            CancellationToken cancellationToken = default)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            return _executor.ExecuteAsync(operation, cancellationToken);
        }
    }
}
