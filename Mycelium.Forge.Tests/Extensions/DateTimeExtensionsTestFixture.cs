// ------------------------------------------------------------------------------------------------
// <copyright file="DateTimeExtensionsTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Extensions
{
    using System;

    using Mycelium.Forge.Extensions;

    [TestFixture]
    public class DateTimeExtensionsTestFixture
    {
        [Test]
        [TestCase("2026-08-25T12:00:00Z", "just now")]
        [TestCase("2026-08-25T11:59:30Z", "just now")]
        [TestCase("2026-08-25T11:59:01Z", "just now")]
        [TestCase("2026-08-25T11:59:00Z", "1 minute ago")]
        [TestCase("2026-08-25T11:58:00Z", "2 minutes ago")]
        [TestCase("2026-08-25T11:01:00Z", "59 minutes ago")]
        [TestCase("2026-08-25T11:00:00Z", "1 hour ago")]
        [TestCase("2026-08-25T10:00:00Z", "2 hours ago")]
        [TestCase("2026-08-24T13:00:00Z", "23 hours ago")]
        [TestCase("2026-08-24T12:00:00Z", "1 day ago")]
        [TestCase("2026-08-23T12:00:00Z", "2 days ago")]
        [TestCase("2026-08-19T12:00:00Z", "6 days ago")]
        [TestCase("2026-08-18T12:00:00Z", "1 week ago")]
        [TestCase("2026-08-11T12:00:00Z", "2 weeks ago")]
        [TestCase("2026-07-27T12:00:00Z", "4 weeks ago")]
        [TestCase("2026-07-26T12:00:00Z", "1 month ago")]
        [TestCase("2026-06-26T12:00:00Z", "2 months ago")]
        [TestCase("2025-08-30T12:00:00Z", "12 months ago")]
        [TestCase("2025-08-25T12:00:00Z", "1 year ago")]
        [TestCase("2024-08-25T12:00:00Z", "2 years ago")]
        public void VerifyToTimeAgo(DateTime targetDateTime, string expected)
        {
            var referenceTimeUtc = new DateTime(2026, 8, 25, 12, 0, 0, DateTimeKind.Utc);
            var referenceTimeLocal = referenceTimeUtc.ToLocalTime();
            var targetTimeLocal = targetDateTime.ToLocalTime();

            var actualUtc = targetDateTime.ToTimeAgo(referenceTimeUtc);
            var actualLocal = targetTimeLocal.ToTimeAgo(referenceTimeLocal);
            var actualParameterless = DateTime.UtcNow.ToTimeAgo();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(actualUtc, Is.EqualTo(expected));
                Assert.That(actualLocal, Is.EqualTo(expected));
                Assert.That(actualParameterless, Is.Not.Empty);
            }
        }
    }
}
