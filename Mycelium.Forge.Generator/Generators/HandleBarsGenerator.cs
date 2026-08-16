// ------------------------------------------------------------------------------------------------
// <copyright file="HandleBarsGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using HandlebarsDotNet;
    using HandlebarsDotNet.Helpers;

    /// <summary>
    /// Abstract super class from which all <see cref="HandlebarsDotNet"/> based generators
    /// need to derive
    /// </summary>
    public abstract class HandleBarsGenerator : Generator
    {
        /// <summary>
        /// The <see cref="IHandlebars"/> instance used to generate code with
        /// </summary>
        protected readonly IHandlebars Handlebars;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleBarsGenerator"/> class.
        /// </summary>
        protected HandleBarsGenerator()
        {
            this.Templates = new Dictionary<string, HandlebarsTemplate<object, object>>();

            this.Handlebars = HandlebarsDotNet.Handlebars.CreateSharedEnvironment();

            // Registers the community Handlebars.Net.Helpers package (transitively pulled in by
            // uml4net.HandleBars). Without this call, Handlebars.Net 2.1.6 fails to compile a
            // parenthesised subexpression used as a block helper's argument - e.g.
            // {{#each (Class.QueryAllProperties this) as | property | }} - with
            // "Sub-expression does not contain a converted MethodCall expression", and helpers
            // invoked without a leading '#' silently render nothing instead of their value.
            HandlebarsHelpers.Register(this.Handlebars);

            this.RegisterHelpers();
            this.RegisterTemplates();
        }

        /// <summary>
        /// Gets the registered <see cref="HandlebarsTemplate{TContext,TData}"/>
        /// </summary>
        public Dictionary<string, HandlebarsTemplate<object, object>> Templates { get; }

        /// <summary>
        /// Register the custom helpers
        /// </summary>
        protected abstract void RegisterHelpers();

        /// <summary>
        /// Register the code templates
        /// </summary>
        protected abstract void RegisterTemplates();

        /// <summary>
        /// Register a handlebars template based on the template (file) name (without extension)
        /// </summary>
        /// <param name="name">
        /// (file) name (without extension)
        /// </param>
        protected void RegisterTemplate(string name)
        {
            var templatePath = Path.Combine(this.TemplateFolderPath, $"{name}.hbs");

            var template = File.ReadAllText(templatePath);

            var compiledTemplate = this.Handlebars.Compile(template);

            this.Templates.Add(name, compiledTemplate);
        }
    }
}
