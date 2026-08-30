// ------------------------------------------------------------------------------------------------
// <copyright file="ShortGuidTestFixture.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Common.Tests
{
    using System;
    using System.Linq;

    [TestFixture]
    public class ShortGuidTestFixture
    {
        [Test]
        public void Verify_that_a_known_guid_encodes_to_its_known_short_guid()
        {
            var guid = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

            Assert.That(guid.ToShortGuid(), Is.EqualTo("ZF-oPxdXYkWz_CyWP2avpg"));
        }

        [Test]
        public void Verify_that_a_known_short_guid_decodes_to_its_known_guid()
        {
            Assert.That("ZF-oPxdXYkWz_CyWP2avpg".FromShortGuid(), Is.EqualTo(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")));
        }

        [Test]
        public void Verify_that_encoding_then_decoding_reproduces_the_original_guid()
        {
            var guid = Guid.NewGuid();

            Assert.That(guid.ToShortGuid().FromShortGuid(), Is.EqualTo(guid));
        }

        [Test]
        public void Verify_that_a_short_guid_is_always_22_characters()
        {
            Assert.That(Guid.NewGuid().ToShortGuid(), Has.Length.EqualTo(22));
        }

        [Test]
        public void Verify_that_FromShortGuid_throws_a_FormatException_for_the_wrong_length()
        {
            Assert.That(() => "tooShort".FromShortGuid(), Throws.TypeOf<FormatException>());
        }

        [Test]
        public void Verify_that_encoding_then_decoding_an_array_reproduces_the_original_guids()
        {
            var guids = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            Assert.That(guids.ToShortGuidArray().FromShortGuidArray(), Is.EqualTo(guids));
        }

        [Test]
        public void Verify_that_ToShortGuidArray_is_bracketed_and_comma_separated()
        {
            var guids = new[] { Guid.NewGuid(), Guid.NewGuid() };

            var array = guids.ToShortGuidArray();

            Assert.That(array, Does.StartWith("[").And.EndsWith("]"));
            Assert.That(array.Count(c => c == ','), Is.EqualTo(1));
        }

        [Test]
        public void Verify_that_an_empty_array_encodes_to_empty_brackets()
        {
            Assert.That(Array.Empty<Guid>().ToShortGuidArray(), Is.EqualTo("[]"));
        }

        [Test]
        public void Verify_that_FromShortGuidArray_decodes_empty_brackets_to_an_empty_list()
        {
            Assert.That("[]".FromShortGuidArray(), Is.Empty);
        }

        [Test]
        public void Verify_that_FromShortGuidArray_throws_a_FormatException_when_not_bracketed()
        {
            Assert.That(() => "notBracketed".FromShortGuidArray(), Throws.TypeOf<FormatException>());
        }
    }
}
