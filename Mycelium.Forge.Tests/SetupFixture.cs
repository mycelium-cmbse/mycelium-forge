// ------------------------------------------------------------------------------------------------
// <copyright file="SetupFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests
{
    using ReactiveUI.Builder;

    /// <summary>
    /// Global setup fixture for the test assembly.
    /// </summary>
    [SetUpFixture]
    public class SetupFixture
    {
        /// <summary>
        /// Performs one-time setup for the entire test assembly before any tests are run.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            RxAppBuilder.CreateReactiveUIBuilder().BuildApp();
        }
    }
}
