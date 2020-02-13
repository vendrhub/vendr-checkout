using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Vendr.Checkout.Migrations;

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public VendrCheckoutComponent(IScopeProvider scopeProvider,
            IMigrationBuilder migrationBuilder,
            IKeyValueService keyValueService,
            ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            // Execute the core migration plan
            //new Upgrader(new VendrCheckoutMigrationPlan())
            //    .Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        { }
    }
}