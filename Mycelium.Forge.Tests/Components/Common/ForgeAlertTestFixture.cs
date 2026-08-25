// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeAlertTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Common
{
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Common;

    [TestFixture]
    public class ForgeAlertTestFixture
    {
        private BunitContext context;

        [SetUp]
        public void SetUp()
        {
            this.context = new BunitContext();

            this.context.Services.AddBlazorBlueprintPrimitives();
            this.context.Services.AddBlazorBlueprintComponents();
            this.context.JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [TearDown]
        public async Task TearDown()
        {
            await this.context.DisposeAsync();
        }

        [Test]
        public void VerifyGetIconClass()
        {
            var defaultAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Default); });

            var infoAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Info); });

            var successAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Success); });

            var warningAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Warning); });

            var dangerAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Danger); });

            var secondaryAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Secondary); });

            var unknownAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, (ForgeAlertVariant)999); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultAlert.Instance.GetIconClass(), Is.EqualTo("text-muted-foreground shrink-0"));
                Assert.That(infoAlert.Instance.GetIconClass(), Is.EqualTo("text-info-icon shrink-0"));
                Assert.That(successAlert.Instance.GetIconClass(), Is.EqualTo("text-success-icon shrink-0"));
                Assert.That(warningAlert.Instance.GetIconClass(), Is.EqualTo("text-warning-icon shrink-0"));
                Assert.That(dangerAlert.Instance.GetIconClass(), Is.EqualTo("text-destructive shrink-0"));
                Assert.That(secondaryAlert.Instance.GetIconClass(), Is.EqualTo("text-muted-foreground shrink-0"));
                Assert.That(unknownAlert.Instance.GetIconClass(), Is.EqualTo("text-muted-foreground shrink-0"));
            }
        }

        [Test]
        public void VerifyGetVariantClass()
        {
            var defaultAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Default); });

            var infoAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Info); });

            var successAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Success); });

            var warningAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Warning); });

            var dangerAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Danger); });

            var secondaryAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, ForgeAlertVariant.Secondary); });

            var unknownAlert = this.context.Render<ForgeAlert>(parameters => { parameters.Add(x => x.Variant, (ForgeAlertVariant)999); });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(defaultAlert.Instance.GetVariantClass(), Is.EqualTo("bg-body-bg border-border text-foreground"));
                Assert.That(infoAlert.Instance.GetVariantClass(), Is.EqualTo("bg-info border-info-border text-info-foreground"));
                Assert.That(successAlert.Instance.GetVariantClass(), Is.EqualTo("bg-success border-success-border text-success-foreground"));
                Assert.That(warningAlert.Instance.GetVariantClass(), Is.EqualTo("bg-warning border-warning-border text-warning-foreground"));
                Assert.That(dangerAlert.Instance.GetVariantClass(), Is.EqualTo("bg-destructive/10 border-destructive/20 text-destructive"));
                Assert.That(secondaryAlert.Instance.GetVariantClass(), Is.EqualTo("bg-muted border-border text-secondary-text"));
                Assert.That(unknownAlert.Instance.GetVariantClass(), Is.EqualTo("bg-body-bg border-border text-foreground"));
            }
        }
    }
}
