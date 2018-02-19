
namespace Stateless1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Fabric;
    using System.Threading;
    using System.Threading.Tasks;
    using global::IStatelessBackendService.Interfaces;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Remoting.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Stateless1 : StatelessService, IStatelessBackendService
    {
        private long iterations = 0;

        public Stateless1(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<long> GetCountAsync()
        {
            ServiceEventSource.Current.ServiceMessage(this.Context, "In the backend service, getting the count!");
            long result = await Task.FromResult(this.iterations);
            if (result % 5 == 0)
            {

                Thread.Sleep(10 * 60 * 1000);
                // try {
                //     throw new InvalidOperationException("Not happy with this number!");
                // }
                // catch (Exception e) {
                //     ServiceEventSource.Current.ServiceMessage(this.Context, e.Message);
                // }
                //
            }
            return result;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

  
        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ++this.iterations;

                if (this.iterations % 50 == 1)
                {
                    // Raise "working" event only once in every 50 iterations
                    ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", this.iterations);
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

    }
}
