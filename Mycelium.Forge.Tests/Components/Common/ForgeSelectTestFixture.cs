// ------------------------------------------------------------------------------------------------
// <copyright file="ForgeSelectTestFixture.cs" company="Starion Group S.A.">
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

    using Microsoft.AspNetCore.Components;

    using Mycelium.Forge.Components.Common;

    [TestFixture]
    public class ForgeSelectTestFixture
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
        public void VerifyGetContentClass()
        {
            var selectNoFilter = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, false);
            });

            var noFilterClass = selectNoFilter.Instance.GetContentClass();

            var selectWithFilter = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, true);
            });

            var filterClass = selectWithFilter.Instance.GetContentClass();

            var selectWithContentClass = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, false);
                parameters.Add(x => x.ContentClass, "custom-content");
            });

            var withContentClass = selectWithContentClass.Instance.GetContentClass();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noFilterClass, Does.Contain("w-full"));
                Assert.That(filterClass, Does.Contain("w-40"));
                Assert.That(withContentClass, Does.Contain("custom-content"));
            }
        }

        [Test]
        public void VerifyGetDisplayText()
        {
            var select = this.context.Render<ForgeSelect<string>>(parameters => { parameters.Add(x => x.Items, ["Option A"]); });

            // default(string) = null -> empty string
            var nullResult = select.Instance.GetDisplayText(default);

            // Without DisplayTextSelector -> ToString()
            var toStringResult = select.Instance.GetDisplayText("Hello");

            // With DisplayTextSelector
            var selectWithSelector = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["Option A"]);
                parameters.Add(x => x.DisplayTextSelector, v => $"[{v}]");
            });

            var selectorResult = selectWithSelector.Instance.GetDisplayText("Hello");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullResult, Is.EqualTo(string.Empty));
                Assert.That(toStringResult, Is.EqualTo("Hello"));
                Assert.That(selectorResult, Is.EqualTo("[Hello]"));
            }
        }

        [Test]
        public void VerifyGetItemClass()
        {
            var select = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["Alpha", "Beta"]);
                parameters.Add(x => x.Value, "Alpha");
                parameters.Add(x => x.FilterStyle, true);
            });

            var selectedClass = select.Instance.GetItemClass("Alpha");
            var unselectedClass = select.Instance.GetItemClass("Beta");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(selectedClass, Does.Contain("font-semibold text-primary"));
                Assert.That(unselectedClass, Does.Not.Contain("font-semibold text-primary"));
            }
        }

        [Test]
        public void VerifyGetTriggerClass()
        {
            var selectNoFilter = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, false);
            });

            var noFilterClass = selectNoFilter.Instance.GetTriggerClass();

            var selectWithFilter = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, true);
            });

            var filterClass = selectWithFilter.Instance.GetTriggerClass();

            var selectWithTriggerClass = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["A"]);
                parameters.Add(x => x.FilterStyle, true);
                parameters.Add(x => x.TriggerClass, "custom-trigger");
            });

            var withTriggerClass = selectWithTriggerClass.Instance.GetTriggerClass();

            using (Assert.EnterMultipleScope())
            {
                Assert.That(noFilterClass, Does.Contain("w-full"));
                Assert.That(filterClass, Does.Contain("h-8"));
                Assert.That(withTriggerClass, Does.Contain("custom-trigger"));
            }
        }

        [Test]
        public async Task VerifyOnValueChanged()
        {
            string? capturedValue = null;
            var valueChanged = new EventCallbackFactory().Create(this, (string v) => { capturedValue = v; });

            var select = this.context.Render<ForgeSelect<string>>(parameters =>
            {
                parameters.Add(x => x.Items, ["Option A", "Option B", "Option C"]);
                parameters.Add(x => x.ValueChanged, valueChanged);
            });

            await select.InvokeAsync(() => select.Instance.OnValueChanged("Option B"));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(select.Instance.Value, Is.EqualTo("Option B"));
                Assert.That(capturedValue, Is.EqualTo("Option B"));
            }
        }
    }
}
