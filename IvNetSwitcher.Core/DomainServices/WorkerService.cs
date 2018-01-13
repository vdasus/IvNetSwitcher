using System;
using System.Collections.Generic;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.DomainServices
{
    public class WorkerService: IWorkerService
    {
        private readonly IServices _service;

        public WorkerService(IServices service)
        {
            _service = service;
        }

        #region Implementation of IWorkerService

        public string GetCurrentStatus()
        {
            var rez = _service.Status();
            return rez.IsSuccess ? rez.Value : rez.Error;
        }

        public IReadOnlyList<Network> ListAvailableNetworks()
        {
            return _service.ListAvailableNetworks();
        }

        public void Run(int delay, int retry, int times = 0)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
