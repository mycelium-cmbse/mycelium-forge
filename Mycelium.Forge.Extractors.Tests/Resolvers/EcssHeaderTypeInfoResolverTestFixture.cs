// ------------------------------------------------------------------------------------------------
// <copyright file="EcssHeaderTypeInfoResolverTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Extractors.Tests.Resolvers
{
    using System.Text.Json;

    using CDP4JsonFileDal.Json;

    using Mycelium.Forge.Extractors.Resolvers;

    using NUnit.Framework;

    [TestFixture]
    public class EcssHeaderTypeInfoResolverTestFixture
    {
        private JsonSerializerOptions serializerOptions;

        [SetUp]
        public void SetUp()
        {
            this.serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = new EcssHeaderTypeInfoResolver()
            };
        }

        [Test]
        public void VerifyGetTypeInfo()
        {
            const string jsonWithDateTime = """
                                            {
                                                "mediaType": "application/x-ecss-c3",
                                                "dataModelVersion": "2.4.1",
                                                "exchangeFileFormatVersion": "1.0",
                                                "createdOn": {
                                                    "utc": "2026-03-30T10:00:00Z",
                                                    "local": "2026-03-30T12:00:00+02:00"
                                                }
                                            }
                                            """;

            var deserializedWithDate = JsonSerializer.Deserialize<ExchangeFileHeader>(jsonWithDateTime, this.serializerOptions);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deserializedWithDate, Is.Not.Null);
                Assert.That(deserializedWithDate.CreatedOn, Is.Not.Null);
                Assert.That(deserializedWithDate.CreatedOn.Utc, Is.EqualTo(DateTime.Parse("2026-03-30T10:00:00Z").ToUniversalTime()));
            }

            const string jsonWithoutDateTime = """
                                               {
                                                   "mediaType": "application/x-ecss-c3",
                                                   "dataModelVersion": "2.4.1",
                                                   "createdOn": null
                                               }
                                               """;

            var deserializedWithoutDate = JsonSerializer.Deserialize<ExchangeFileHeader>(jsonWithoutDateTime, this.serializerOptions);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(deserializedWithoutDate, Is.Not.Null);
                Assert.That(deserializedWithoutDate.CreatedOn, Is.Null);
            }
        }
    }
}
