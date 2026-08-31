// ------------------------------------------------------------------------------------------------
// <copyright file="UmlCoreCarterModuleGenerator.cs" company="Starion Group S.A.">
//
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
//
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Generators
{
    using uml4net.Extensions;
    using uml4net.StructuredClassifiers;
    using uml4net.xmi.Readers;

    /// <summary>
    /// Generates a Carter <c>ICarterModule</c> per concrete class (<c>Forge</c> excepted - it gets its
    /// own singleton module) - the exact same containment chains
    /// <see cref="UmlCoreOpenApiPathsGenerator"/> uses - registering the same routes with
    /// <c>501 Not Implemented</c> placeholder handlers, via <c>carter-module-uml-template.hbs</c>, the
    /// same Handlebars-templated approach every other generator in this project uses.
    /// </summary>
    /// <remarks>
    /// One OpenAPI path collapses every accepted form of an identifier (guid, ShortGuid, batch,
    /// shortName) into a single, constraint-free path template - but ASP.NET Core actually needs one
    /// literal, constrained route per form, or they're ambiguous at the routing layer. For a top-level
    /// class that's a handful of routes (see the three <c>identifier</c> forms plus an optional
    /// <c>shortName</c> alias below). For a class nested under one or more non-singleton ancestors,
    /// every such ancestor segment carries its own guid-or-ShortGuid choice too, and the whole module
    /// is the cartesian product of those choices - <see cref="BuildAncestorCombinations"/>. A singleton
    /// ancestor segment (upper multiplicity 1, e.g. a hypothetical class nested under
    /// <c>PackageVersion.metaData</c>) contributes no choice at all, since there's only ever one.
    /// <para>
    /// That combinatorial fan-out is exactly why the template itself stays a thin, single
    /// <c>{{#each}}</c> loop over an already-fully-resolved <see cref="CarterRouteRegistration"/> list:
    /// the recursive chain-walking and cartesian-product computation needs the full model
    /// (<see cref="XmiReaderResult"/>), which isn't reachable purely from Handlebars helper
    /// registration (that happens once, at generator construction, before any model is known) - so, as
    /// with every other generator here, the actual computation lives in C#
    /// (<see cref="BuildRegistrations"/> and friends) and the template only renders the result, the
    /// same division of labour <c>dto-class-uml-template.hbs</c>/<c>Property.WriteForDTOClass</c> use.
    /// </para>
    /// </remarks>
    public class UmlCoreCarterModuleGenerator : UmlHandleBarsGenerator
    {
        private const string TemplateName = "carter-module-uml-template";

        /// <summary>
        /// Generates one Carter module file per routable class, plus <c>ForgeModule.cs</c> for the
        /// singleton, and writes each to <paramref name="outputDirectory"/>.
        /// </summary>
        public override async Task GenerateAsync(XmiReaderResult xmiReaderResult, DirectoryInfo outputDirectory)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentNullException.ThrowIfNull(outputDirectory);

            await WriteAsync(this.RenderForgeModule(), outputDirectory, "ForgeModule.cs");

            foreach (var @class in UmlCoreOpenApiPathsGenerator.QueryRoutableClasses(xmiReaderResult))
            {
                var fileName = $"{@class.Name.CapitalizeFirstLetter()}Module.cs";

                await WriteAsync(this.RenderCollectionModule(xmiReaderResult, @class), outputDirectory, fileName);
            }
        }

        /// <summary>
        /// Generates the Carter module for a single, named class, without necessarily writing it to
        /// disk; the rendered text is returned so it can be diffed against a committed golden file by
        /// <c>ExpectedOutputTestFixture</c>.
        /// </summary>
        public Task<string> GenerateCollectionModuleAsync(XmiReaderResult xmiReaderResult, string className)
        {
            ArgumentNullException.ThrowIfNull(xmiReaderResult);
            ArgumentException.ThrowIfNullOrWhiteSpace(className);

            var @class = UmlCoreOpenApiPathsGenerator.QueryRoutableClasses(xmiReaderResult).Single(x => x.Name == className);

            return Task.FromResult(this.RenderCollectionModule(xmiReaderResult, @class));
        }

        /// <summary>
        /// Generates the Carter module for the <c>Forge</c> singleton, without necessarily writing it
        /// to disk.
        /// </summary>
        public Task<string> GenerateForgeModuleAsync()
        {
            return Task.FromResult(this.RenderForgeModule());
        }

        /// <inheritdoc />
        protected override void RegisterHelpers()
        {
            // No custom helpers: every value the template needs - HttpVerb, Path, LambdaParameters,
            // EndpointName - is a plain string property on the already-fully-resolved
            // CarterRouteRegistration the template iterates, so plain {{ registration.X }}
            // interpolation is enough (the same "no helper needed" shape
            // enumprovider-uml-template.hbs's own Parse loop uses).
        }

        /// <inheritdoc />
        protected override void RegisterTemplates()
        {
            this.RegisterTemplate(TemplateName);
        }

        private string RenderForgeModule()
        {
            var model = new CarterModuleModel("Forge",
            [
                new CarterRouteRegistration("Get", "/forge", string.Empty, "GetForge"),
                new CarterRouteRegistration("Patch", "/forge", string.Empty, "UpdateForge")
            ]);

            return this.CodeCleanup(this.Templates[TemplateName](model));
        }

        private string RenderCollectionModule(XmiReaderResult xmiReaderResult, IClass @class)
        {
            var route = UmlCoreOpenApiPathsGenerator.BuildClassRoute(@class, xmiReaderResult);

            var model = new CarterModuleModel(@class.Name, BuildRegistrations(xmiReaderResult, @class, route));

            return this.CodeCleanup(this.Templates[TemplateName](model));
        }

        /// <summary>
        /// The context object <c>carter-module-uml-template.hbs</c> renders - everything the template
        /// needs, already resolved to plain strings, so the template itself stays a single
        /// <c>{{#each}}</c> loop. The template derives the generated class's own name as
        /// <c>{{this.ClassName}}Module</c> directly, so there's no separate <c>ModuleName</c> to keep
        /// in sync with it.
        /// </summary>
        /// <param name="ClassName">The UML class these routes address (e.g. <c>Organization</c>) - both
        /// for doc-comment text and, concatenated with <c>Module</c> in the template, the generated
        /// class's own name.</param>
        /// <param name="Registrations">Every <c>Map*(...).WithName(...)</c> call this module registers, in order.</param>
        private sealed record CarterModuleModel(string ClassName, IReadOnlyList<CarterRouteRegistration> Registrations);

        /// <summary>
        /// One fully-resolved <c>api.Map{HttpVerb}("{Path}", ({LambdaParameters}) => ...).WithName("{EndpointName}")</c> call.
        /// </summary>
        /// <param name="HttpVerb">The <c>Map</c> suffix - <c>Get</c>, <c>Post</c>, <c>Put</c>, <c>Patch</c> or <c>Delete</c>.</param>
        /// <param name="Path">The full, already ancestor-qualified, already-constrained route template
        /// (e.g. <c>/account/{accountIdentifier:guid}/ownedPackage</c>).</param>
        /// <param name="LambdaParameters">Every route parameter's C# lambda declaration, comma-joined
        /// (e.g. <c>Guid accountIdentifier, Guid identifier</c>) - empty for a route with none.</param>
        /// <param name="EndpointName">The value passed to <c>.WithName(...)</c> - must be unique across
        /// every registration in the whole generated API, not just within one module.</param>
        private sealed record CarterRouteRegistration(string HttpVerb, string Path, string LambdaParameters, string EndpointName);

        /// <summary>
        /// Every <c>Map*(...).WithName(...)</c> call <paramref name="class"/>'s module needs, across
        /// every containment chain it's reachable through and, within each chain, every combination of
        /// ancestor forms - the full cartesian fan-out <see cref="BuildAncestorCombinations"/> and
        /// <see cref="AppendChainRegistrations"/> produce. A top-level class (one chain, no ancestors)
        /// still goes through the exact same path, it just fans out to nothing: one chain with zero
        /// ancestors, one ancestor combination (the empty one), one pass through
        /// <see cref="AppendChainRegistrations"/>.
        /// </summary>
        /// <param name="xmiReaderResult">The model to resolve containment chains and ancestor
        /// multiplicity/forms against.</param>
        /// <param name="class">The class these registrations address - the module's own leaf class.</param>
        /// <param name="route">
        /// <paramref name="class"/>'s own <see cref="UmlCoreOpenApiPathsGenerator.ClassRoute"/> -
        /// its composite-owning property's name, whether it's a singleton, and whether it has a
        /// <c>shortName</c> alias - resolved once by the caller rather than re-derived per chain here.
        /// </param>
        private static IReadOnlyList<CarterRouteRegistration> BuildRegistrations(XmiReaderResult xmiReaderResult, IClass @class, UmlCoreOpenApiPathsGenerator.ClassRoute route)
        {
            var registrations = new List<CarterRouteRegistration>();

            var chains = UmlCoreOpenApiPathsGenerator.QueryOwnerChains(xmiReaderResult, @class)
                .OrderBy(chain => string.Join('/', chain.Select(c => c.Name)));

            foreach (var chain in chains)
            {
                var ancestors = chain.Take(chain.Count - 1).ToList();

                foreach (var ancestorCombination in BuildAncestorCombinations(xmiReaderResult, ancestors))
                {
                    AppendChainRegistrations(registrations, @class, route, ancestorCombination);
                }
            }

            return registrations;
        }

        /// <summary>
        /// One ancestor segment's resolved form within a single, fully-disambiguated Carter route. For
        /// a singleton ancestor (upper multiplicity 1), <see cref="ParameterName"/>/
        /// <see cref="RouteConstraint"/>/<see cref="ClrType"/> are all <see langword="null"/> - it
        /// contributes only its own property-named path segment, no route parameter, and no
        /// <see cref="NameToken"/> (there's nothing to disambiguate: only one route shape reaches it).
        /// </summary>
        /// <param name="Class">The ancestor class this segment addresses.</param>
        /// <param name="ParameterName">The lambda/route parameter name (e.g. <c>accountIdentifier</c>) -
        /// <see langword="null"/> for a singleton ancestor.</param>
        /// <param name="CollectionName">The ancestor's own composite-owning property name, verbatim
        /// (e.g. <c>account</c>, <c>ownedPackage</c>).</param>
        /// <param name="RouteConstraint">The ASP.NET Core route constraint token: <c>guid</c> or
        /// <c>ShortGuid</c> - <see langword="null"/> for a singleton ancestor.</param>
        /// <param name="ClrType">The C# type the handler binds this parameter as: <c>Guid</c> or
        /// <c>string</c> - <see langword="null"/> for a singleton ancestor.</param>
        /// <param name="NameToken">Contributes to <c>.WithName(...)</c>, e.g. <c>AccountId</c> or
        /// <c>AccountShortGuid</c> - every combination of ancestor forms needs its own unique endpoint
        /// name, since Carter/ASP.NET Core requires every registered endpoint name to be unique, not
        /// just every OpenAPI operationId. Empty for a singleton ancestor.</param>
        private sealed record AncestorChoice(IClass Class, string? ParameterName, string CollectionName, string? RouteConstraint, string? ClrType, string NameToken);

        /// <summary>
        /// The cartesian product of every non-singleton ancestor's own guid-or-ShortGuid choice,
        /// root-first - one element per fully-disambiguated Carter route this chain needs. A singleton
        /// ancestor contributes exactly one, non-branching segment (no route parameter at all - there's
        /// only ever one). A chain with no ancestors (a top-level class) yields exactly one, empty
        /// combination - the same single, unqualified route shape the class had before nesting existed.
        /// </summary>
        private static IEnumerable<IReadOnlyList<AncestorChoice>> BuildAncestorCombinations(XmiReaderResult xmiReaderResult, IReadOnlyList<IClass> ancestors)
        {
            IEnumerable<IReadOnlyList<AncestorChoice>> combinations = [Array.Empty<AncestorChoice>()];

            foreach (var ancestor in ancestors)
            {
                var route = UmlCoreOpenApiPathsGenerator.BuildClassRoute(ancestor, xmiReaderResult);

                if (route.IsSingleton)
                {
                    var singletonChoice = new AncestorChoice(ancestor, null, route.CollectionName, null, null, string.Empty);
                    combinations = combinations.Select(prefix => (IReadOnlyList<AncestorChoice>)[.. prefix, singletonChoice]);
                    continue;
                }

                var parameterName = UmlCoreOpenApiPathsGenerator.AncestorParameterName(ancestor);

                AncestorChoice[] choices =
                [
                    new AncestorChoice(ancestor, parameterName, route.CollectionName, "guid", "Guid", $"{ancestor.Name.CapitalizeFirstLetter()}Id"),
                    new AncestorChoice(ancestor, parameterName, route.CollectionName, "ShortGuid", "string", $"{ancestor.Name.CapitalizeFirstLetter()}ShortGuid")
                ];

                combinations = combinations.SelectMany(prefix => choices.Select(choice => (IReadOnlyList<AncestorChoice>)[.. prefix, choice]));
            }

            return combinations;
        }

        /// <summary>
        /// Appends the full route block for one class, for one fully-resolved combination of ancestor
        /// forms - an empty <paramref name="ancestorCombination"/> reduces to exactly the flat,
        /// top-level shape a class with no composite owner (other than <c>Forge</c>) has always had. A
        /// singleton class (<see cref="UmlCoreOpenApiPathsGenerator.ClassRoute.IsSingleton"/>) gets no
        /// <c>List</c>/<c>POST</c>/<c>{identifier}</c>/<c>{shortName}</c> - just
        /// <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/<c>DELETE</c> directly at its own property-named path.
        /// </summary>
        private static void AppendChainRegistrations(List<CarterRouteRegistration> registrations, IClass @class, UmlCoreOpenApiPathsGenerator.ClassRoute route, IReadOnlyList<AncestorChoice> ancestorCombination)
        {
            var pathPrefix = string.Concat(ancestorCombination.Select(c => c.RouteConstraint == null ? $"/{c.CollectionName}" : $"/{c.CollectionName}/{{{c.ParameterName}:{c.RouteConstraint}}}"));
            var ancestorParams = string.Concat(ancestorCombination.Where(c => c.ParameterName != null).Select(c => $"{c.ClrType} {c.ParameterName}, "));
            var nameToken = string.Concat(ancestorCombination.Select(c => c.NameToken));
            var collectionPath = $"{pathPrefix}/{route.CollectionName}";

            if (route.IsSingleton)
            {
                AppendSingletonRegistrations(registrations, collectionPath, @class.Name, nameToken, ancestorParams);
                return;
            }

            var collectionParams = ancestorParams.TrimEnd(',', ' ');

            registrations.Add(new CarterRouteRegistration("Get", collectionPath, collectionParams, $"List{nameToken}{route.CollectionName.CapitalizeFirstLetter()}"));
            registrations.Add(new CarterRouteRegistration("Post", collectionPath, collectionParams, $"Create{nameToken}{@class.Name.CapitalizeFirstLetter()}"));

            // {identifier} covers three accepted forms - all documented as one OpenAPI path, since
            // none of them changes the URL's shape - but each needs its own Carter registration with
            // its own route constraint, or the three would be ambiguous at the routing layer (they are
            // otherwise the exact same single-segment template). The built-in "guid" constraint and
            // the custom "ShortGuid"/"EnumerableOfShortGuid" ones (registered in Program.cs) are what
            // let the router tell them apart. EnumerableOfShortGuid is GET-only: a single shared
            // partial body applied to a batch PUT/PATCH, or a batch DELETE, isn't decided yet.
            AppendItemRegistrations(registrations, collectionPath, @class.Name, nameToken, "identifier:guid", "Guid", "identifier", "ById", ancestorParams, includeWrites: true);
            AppendItemRegistrations(registrations, collectionPath, @class.Name, nameToken, "identifier:ShortGuid", "string", "identifier", "ByShortGuid", ancestorParams, includeWrites: true);
            AppendItemRegistrations(registrations, collectionPath, @class.Name, nameToken, "identifier:EnumerableOfShortGuid", "string", "identifier", "ByShortGuids", ancestorParams, includeWrites: false);

            if (route.HasShortName)
            {
                // GET-only: shortName is a read-only alias, PUT/PATCH/DELETE only ever go through identifier.
                AppendItemRegistrations(registrations, collectionPath, @class.Name, nameToken, "shortName", "string", "shortName", "ByShortName", ancestorParams, includeWrites: false);
            }
        }

        /// <summary>
        /// Appends <c>GET</c>/<c>PUT</c>/<c>PATCH</c>/<c>DELETE</c> for a singleton composite child -
        /// no <c>List</c>, no <c>POST</c>, no further identifier segment, since there's always exactly
        /// one and <paramref name="ancestorParams"/> (the parent context) already addresses it.
        /// </summary>
        private static void AppendSingletonRegistrations(List<CarterRouteRegistration> registrations, string path, string className, string nameToken, string ancestorParams)
        {
            var lambdaParams = ancestorParams.TrimEnd(',', ' ');

            registrations.Add(new CarterRouteRegistration("Get", path, lambdaParams, $"Get{nameToken}{className.CapitalizeFirstLetter()}"));
            registrations.Add(new CarterRouteRegistration("Put", path, lambdaParams, $"Set{nameToken}{className.CapitalizeFirstLetter()}"));
            registrations.Add(new CarterRouteRegistration("Patch", path, lambdaParams, $"Update{nameToken}{className.CapitalizeFirstLetter()}"));
            registrations.Add(new CarterRouteRegistration("Delete", path, lambdaParams, $"Delete{nameToken}{className.CapitalizeFirstLetter()}"));
        }

        /// <summary>
        /// Appends one item route's <c>GET</c> (and, unless <paramref name="includeWrites"/> is
        /// <see langword="false"/>, its <c>PUT</c>/<c>PATCH</c>/<c>DELETE</c>) registrations for a
        /// single route template, under one already-resolved ancestor path prefix.
        /// </summary>
        /// <param name="registrations">The registration list being built.</param>
        /// <param name="collectionPath">The full, already ancestor-qualified collection path (e.g.
        /// <c>/account/{accountIdentifier:guid}/address</c>).</param>
        /// <param name="className">The class this route addresses, for operationId naming.</param>
        /// <param name="nameToken">The ancestor-form-combination token contributed to every
        /// <c>.WithName(...)</c> here - empty for a top-level class.</param>
        /// <param name="routeTemplate">The leaf route segment, with its constraint suffix if any (e.g.
        /// <c>"identifier:guid"</c>, <c>"shortName"</c>).</param>
        /// <param name="clrType">The C# type the handler binds the leaf parameter as.</param>
        /// <param name="parameterName">The leaf lambda parameter name - matches the part of
        /// <paramref name="routeTemplate"/> before any <c>:</c> constraint suffix.</param>
        /// <param name="operationIdSuffix">Appended to every operation registered here.</param>
        /// <param name="ancestorParams">Every non-singleton ancestor's own lambda parameter
        /// declaration, each followed by <c>", "</c> - prepended to the leaf's own parameter in every
        /// handler here.</param>
        /// <param name="includeWrites">Whether this route also gets <c>PUT</c>/<c>PATCH</c>/
        /// <c>DELETE</c> registrations - <see langword="false"/> for the batch
        /// <c>EnumerableOfShortGuid</c> route and the <c>shortName</c> alias: a shared body applied to
        /// more than one resource, or a batch delete, isn't decided yet, and <c>shortName</c> is a
        /// read-only alias, writes only ever go through <c>identifier</c>.</param>
        private static void AppendItemRegistrations(List<CarterRouteRegistration> registrations, string collectionPath, string className, string nameToken, string routeTemplate, string clrType, string parameterName, string operationIdSuffix, string ancestorParams, bool includeWrites)
        {
            var fullPath = $"{collectionPath}/{{{routeTemplate}}}";
            var lambdaParams = $"{ancestorParams}{clrType} {parameterName}";

            registrations.Add(new CarterRouteRegistration("Get", fullPath, lambdaParams, $"Get{nameToken}{className.CapitalizeFirstLetter()}{operationIdSuffix}"));

            if (!includeWrites)
            {
                return;
            }

            registrations.Add(new CarterRouteRegistration("Put", fullPath, lambdaParams, $"Set{nameToken}{className.CapitalizeFirstLetter()}{operationIdSuffix}"));
            registrations.Add(new CarterRouteRegistration("Patch", fullPath, lambdaParams, $"Update{nameToken}{className.CapitalizeFirstLetter()}{operationIdSuffix}"));
            registrations.Add(new CarterRouteRegistration("Delete", fullPath, lambdaParams, $"Delete{nameToken}{className.CapitalizeFirstLetter()}{operationIdSuffix}"));
        }
    }
}
