// ------------------------------------------------------------------------------------------------
// <copyright file="DocumentationTocTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Tests.Components.Pages.Documentation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlazorBlueprint.Components;
    using BlazorBlueprint.Primitives.Extensions;

    using Bunit;

    using Mycelium.Forge.Components.Pages.Documentation;
    using Mycelium.Forge.Models.Documentation;

    [TestFixture]
    public class DocumentationTocTestFixture
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
        public void VerifyGetTocItemClass()
        {
            // Null -> empty string
            var nullResult = DocumentationToc.GetTocItemClass(null);

            // IsActive=true -> contains text-primary
            var activeItem = new DocumentationTocItemModel { Title = "Active", IsActive = true };
            var activeResult = DocumentationToc.GetTocItemClass(activeItem);

            // IsActive=false -> contains text-muted-foreground
            var inactiveItem = new DocumentationTocItemModel { Title = "Inactive", IsActive = false };
            var inactiveResult = DocumentationToc.GetTocItemClass(inactiveItem);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(nullResult, Is.EqualTo(string.Empty));
                Assert.That(activeResult, Does.Contain("text-primary"));
                Assert.That(inactiveResult, Does.Contain("text-muted-foreground"));
            }
        }

        [Test]
        public async Task VerifyOnItemClick()
        {
            var items = new List<DocumentationTocItemModel>
            {
                new() { Title = "Introduction", TargetId = "introduction", IsActive = false },
                new() { Title = "Getting Started", TargetId = "getting-started", IsActive = false },
                new() { Title = "Advanced", TargetId = "advanced", IsActive = false }
            };

            var toc = this.context.Render<DocumentationToc>(parameters => { parameters.Add(x => x.Items, items); });

            // Null item -> no throw
            Assert.DoesNotThrowAsync(() => toc.InvokeAsync(() => toc.Instance.OnItemClick(null)));

            // Item with empty TargetId -> no throw
            var emptyIdItem = new DocumentationTocItemModel { Title = "Empty", TargetId = string.Empty };
            Assert.DoesNotThrowAsync(() => toc.InvokeAsync(() => toc.Instance.OnItemClick(emptyIdItem)));

            // Valid item -> that item becomes active, others inactive
            await toc.InvokeAsync(() => toc.Instance.OnItemClick(items[1]));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(items[0].IsActive, Is.False);
                Assert.That(items[1].IsActive, Is.True);
                Assert.That(items[2].IsActive, Is.False);
            }
        }
    }
}
