// ------------------------------------------------------------------------------------------------
// <copyright file="OutboxServiceCollectionExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Outbox.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using Moq;

    using Mycelium.Forge.Dal.DatabaseSource;
    using Mycelium.Forge.Orm.Outbox;
    using Mycelium.Forge.Outbox.Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class OutboxServiceCollectionExtensionsTestFixture
    {
        [Test]
        public void VerifyAddOutbox()
        {
            // Case 1: Null arguments throw ArgumentNullException
            var emptyServices = new ServiceCollection();
            var emptyConfiguration = new ConfigurationBuilder().Build();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => ((IServiceCollection)null).AddOutbox(emptyConfiguration), Throws.ArgumentNullException);
                Assert.That(() => emptyServices.AddOutbox(null), Throws.ArgumentNullException);
            }

            // Case 2: Valid configuration registers OutboxOptions, IOutboxDao, and OutboxDispatcherHostedService
            var validSettings = new Dictionary<string, string>
            {
                { "Outbox:PollingIntervalSeconds", "3" },
                { "Outbox:BatchSize", "15" }
            };

            var validConfiguration = new ConfigurationBuilder().AddInMemoryCollection(validSettings).Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<IDatabaseSource>());
            services.AddOutbox(validConfiguration);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var options = provider.GetRequiredService<IOptions<OutboxOptions>>().Value;
            var outboxDao = scope.ServiceProvider.GetService<IOutboxDao>();
            var hostedServices = provider.GetServices<IHostedService>().ToList();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(options.PollingIntervalSeconds, Is.EqualTo(3));
                Assert.That(options.BatchSize, Is.EqualTo(15));
                Assert.That(outboxDao, Is.Not.Null);
                Assert.That(hostedServices.Exists(s => s is OutboxDispatcherHostedService), Is.True);
            }

            // Case 3: Invalid OutboxOptions triggers OptionsValidationException when accessed
            var invalidSettings = new Dictionary<string, string>
            {
                { "Outbox:BatchSize", "0" }
            };

            var invalidConfiguration = new ConfigurationBuilder().AddInMemoryCollection(invalidSettings).Build();

            var invalidServices = new ServiceCollection();
            invalidServices.AddOutbox(invalidConfiguration);

            Assert.That(() =>
            {
                using var invalidProvider = invalidServices.BuildServiceProvider();
                return invalidProvider.GetRequiredService<IOptions<OutboxOptions>>().Value;
            }, Throws.InstanceOf<OptionsValidationException>());
        }
    }
}
