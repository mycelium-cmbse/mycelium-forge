// ------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtensionTestFixture.cs" company="Starion Group S.A.">
// 
//   Copyright 2026 Starion Group S.A.
//   SPDX-License-Identifier: Apache-2.0
// 
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Mycelium.Forge.Generator.Tests.Extensions
{
    using System;
    using System.Linq;

    using uml4net.Classification;
    using uml4net.Extensions;
    using uml4net.SimpleClassifiers;
    using uml4net.StructuredClassifiers;
    using uml4net.Values;

    using GeneratorClassExtensions = Mycelium.Forge.Generator.Extensions.ClassExtensions;
    using GeneratorPropertyExtension = Mycelium.Forge.Generator.Extensions.PropertyExtension;

    /// <summary>
    /// Suite of tests for the <see cref="GeneratorPropertyExtension" /> class.
    /// </summary>
    [TestFixture]
    public class PropertyExtensionTestFixture
    {
        private IProperty administratorProperty;
        private IProperty packageTypeProperty;
        private IProperty billingEmailProperty;
        private IProperty downloadCountProperty;
        private IProperty idProperty;
        private IProperty scopePrimaryAddressProperty;
        private IProperty scopeAddressProperty;
        private IProperty orgInvitationTargetProperty;

        /// <summary>
        /// Sets up the test fixture before each test execution.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.administratorProperty = GetOwnedProperty("Organization", "administrator");
            this.packageTypeProperty = GetOwnedProperty("Package", "packageType");
            this.billingEmailProperty = GetOwnedProperty("Scope", "billingEmail");
            this.downloadCountProperty = GetOwnedProperty("Package", "downloadCount");
            this.idProperty = GetOwnedProperty("Thing", "id");
            this.scopePrimaryAddressProperty = GetOwnedProperty("Scope", "primaryAddress");
            this.scopeAddressProperty = GetOwnedProperty("Scope", "address");
            this.orgInvitationTargetProperty = GetOwnedProperty("OrganizationInvitation", "target");
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue" /> handles various enum
        /// default value specifications.
        /// </summary>
        [Test]
        public void VerifyQueryIsEnumPropertyWithDefaultValue()
        {
            var defaultVisibilityProp = GetOwnedProperty("Scope", "defaultPackageVisibility");
            var enumeration = new Enumeration { Name = "TestEnum" };
            var literal = new EnumerationLiteral { Name = "DefaultVal" };
            enumeration.OwnedLiteral.Add(literal);

            var enumPropertyWithLiteralInstance = new Property
            {
                Name = "enumProp",
                Type = enumeration
            };

            var instanceVal = new InstanceValue { Instance = literal };
            enumPropertyWithLiteralInstance.DefaultValue.Add(instanceVal);

            var enumPropertyWithNonEnumInstance = new Property
            {
                Name = "enumProp2",
                Type = enumeration
            };

            var nonEnumInstanceVal = new InstanceValue { Instance = new InstanceSpecification { Name = "SomeInstance" } };
            enumPropertyWithNonEnumInstance.DefaultValue.Add(nonEnumInstanceVal);

            var enumPropertyWithStringLiteral = new Property
            {
                Name = "enumProp3",
                Type = enumeration
            };

            enumPropertyWithStringLiteral.DefaultValue.Add(new LiteralString { Value = "DefaultVal" });

            var enumPropertyWithOtherLiteral = new Property
            {
                Name = "enumProp4",
                Type = enumeration
            };

            enumPropertyWithOtherLiteral.DefaultValue.Add(new LiteralInteger { Value = 42 });

            var enumPropertyWithNullString = new Property
            {
                Name = "enumProp5",
                Type = enumeration
            };

            enumPropertyWithNullString.DefaultValue.Add(new LiteralString { Value = "null" });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(this.billingEmailProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(defaultVisibilityProp), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(enumPropertyWithLiteralInstance), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(enumPropertyWithNonEnumInstance), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(enumPropertyWithStringLiteral), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(enumPropertyWithOtherLiteral), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryIsEnumPropertyWithDefaultValue(enumPropertyWithNullString), Is.False);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryManyToManySourcePropertyName" /> returns the source property
        /// column name.
        /// </summary>
        [Test]
        public void VerifyQueryManyToManySourcePropertyName()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManySourcePropertyName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManySourcePropertyName(this.packageTypeProperty), Throws.TypeOf<ArgumentException>());

                Assert.That(GeneratorPropertyExtension.QueryManyToManySourcePropertyName(this.administratorProperty), Is.EqualTo("sourceOrganization"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryManyToManySourcePropertyTypeName" /> returns the source owner
        /// type name.
        /// </summary>
        [Test]
        public void VerifyQueryManyToManySourcePropertyTypeName()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManySourcePropertyTypeName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManySourcePropertyTypeName(this.packageTypeProperty), Throws.TypeOf<ArgumentException>());
                Assert.That(GeneratorPropertyExtension.QueryManyToManySourcePropertyTypeName(this.administratorProperty), Is.EqualTo("Organization"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryManyToManyTableName" /> returns the formatted junction table
        /// name.
        /// </summary>
        [Test]
        public void VerifyQueryManyToManyTableName()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTableName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTableName(this.packageTypeProperty), Throws.TypeOf<ArgumentException>());
                Assert.That(GeneratorPropertyExtension.QueryManyToManyTableName(this.administratorProperty), Is.EqualTo("Organization_administrator__Account"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryManyToManyTargetPropertyName" /> returns the target property
        /// column name.
        /// </summary>
        [Test]
        public void VerifyQueryManyToManyTargetPropertyName()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTargetPropertyName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTargetPropertyName(this.packageTypeProperty), Throws.TypeOf<ArgumentException>());

                Assert.That(GeneratorPropertyExtension.QueryManyToManyTargetPropertyName(this.administratorProperty), Is.EqualTo("targetAccount"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryManyToManyTargetPropertyTypeName" /> returns the target type
        /// name.
        /// </summary>
        [Test]
        public void VerifyQueryManyToManyTargetPropertyTypeName()
        {
            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTargetPropertyTypeName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => GeneratorPropertyExtension.QueryManyToManyTargetPropertyTypeName(this.packageTypeProperty), Throws.TypeOf<ArgumentException>());
                Assert.That(GeneratorPropertyExtension.QueryManyToManyTargetPropertyTypeName(this.administratorProperty), Is.EqualTo("Account"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute" /> evaluates opposite SQL
        /// attribute requirement.
        /// </summary>
        [Test]
        public void VerifyQueryOppositeAttributeNeedsSqlAttribute()
        {
            var targetClass = new Class { Name = "Target" };

            var refPropWithoutOpposite = new Property
            {
                Name = "refProp",
                Type = targetClass
            };

            refPropWithoutOpposite.LowerValue.Add(new LiteralInteger { Value = 1 });
            refPropWithoutOpposite.UpperValue.Add(new LiteralUnlimitedNatural { Value = "1" });

            var addressClass = GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == "Address");

            var addressOwnerProperty = GeneratorClassExtensions.QuerySqlSingleReferenceProperties(addressClass).Single(x => x.Name == "owner");

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(this.administratorProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(this.billingEmailProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(this.packageTypeProperty), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(this.orgInvitationTargetProperty), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(this.scopeAddressProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOppositeAttributeNeedsSqlAttribute(addressOwnerProperty), Is.True);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute" /> evaluates SQL attribute
        /// requirement.
        /// </summary>
        [Test]
        public void VerifyQueryOwnedAttributeNeedsSqlAttribute()
        {
            var targetClass = new Class { Name = "Target" };

            var enumerableRefProp = new Property
            {
                Name = "enumerableRef",
                Type = targetClass
            };

            enumerableRefProp.UpperValue.Add(new LiteralUnlimitedNatural { Value = "*" });

            var singleRefWithoutOpposite = new Property
            {
                Name = "singleRef",
                Type = targetClass
            };

            singleRefWithoutOpposite.LowerValue.Add(new LiteralInteger { Value = 1 });
            singleRefWithoutOpposite.UpperValue.Add(new LiteralUnlimitedNatural { Value = "1" });

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(this.administratorProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(this.billingEmailProperty), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(this.packageTypeProperty), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(this.orgInvitationTargetProperty), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(this.scopePrimaryAddressProperty), Is.True);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(enumerableRefProp), Is.False);
                Assert.That(GeneratorPropertyExtension.QueryOwnedAttributeNeedsSqlAttribute(singleRefWithoutOpposite), Is.True);
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QueryPropertyNameBasedOnUmlProperties" /> formats names according
        /// to UML attributes.
        /// </summary>
        [Test]
        public void VerifyQueryPropertyNameBasedOnUmlProperties()
        {
            var derivedUnionProperty = new Property { Name = "DerivedUnionProp", IsDerivedUnion = true };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QueryPropertyNameBasedOnUmlProperties(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QueryPropertyNameBasedOnUmlProperties(this.billingEmailProperty), Is.EqualTo("BillingEmail"));
                Assert.That(GeneratorPropertyExtension.QueryPropertyNameBasedOnUmlProperties(this.downloadCountProperty), Is.EqualTo("downloadCount"));
                Assert.That(GeneratorPropertyExtension.QueryPropertyNameBasedOnUmlProperties(derivedUnionProperty), Is.EqualTo("derivedUnionProp"));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QuerySqlAttributeName" /> generates lower-camel-case column names.
        /// </summary>
        [Test]
        public void VerifyQuerySqlAttributeName()
        {
            var nullNameProp = new Property { Name = null! };
            var emptyNameProp = new Property { Name = "   " };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QuerySqlAttributeName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QuerySqlAttributeName(this.billingEmailProperty), Is.EqualTo("billingEmail"));
                Assert.That(GeneratorPropertyExtension.QuerySqlAttributeName(this.idProperty), Is.EqualTo("id"));
                Assert.That(GeneratorPropertyExtension.QuerySqlAttributeName(nullNameProp), Is.EqualTo(string.Empty));
                Assert.That(GeneratorPropertyExtension.QuerySqlAttributeName(emptyNameProp), Is.EqualTo(string.Empty));
            }
        }

        /// <summary>
        /// Verifies that <see cref="GeneratorPropertyExtension.QuerySqlTypeName" /> maps UML types to SQL types correctly.
        /// </summary>
        [Test]
        public void VerifyQuerySqlTypeName()
        {
            var enumeration = new Enumeration { Name = "StatusKind" };
            var enumProp = new Property { Name = "status", Type = enumeration };

            var unknownCustomType = new DataType { Name = "UnknownCustomType" };
            var unknownProp = new Property { Name = "customProp", Type = unknownCustomType };

            var targetClass = new Class { Name = "Target" };

            var enumerableRef = new Property
            {
                Name = "refs",
                Type = targetClass
            };

            enumerableRef.UpperValue.Add(new LiteralUnlimitedNatural { Value = "*" });

            var unTypedProp = new Property { Name = "empty" };

            using (Assert.EnterMultipleScope())
            {
                Assert.That(() => GeneratorPropertyExtension.QuerySqlTypeName(null!), Throws.TypeOf<ArgumentNullException>());
                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(this.billingEmailProperty), Is.EqualTo("text"));
                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(this.packageTypeProperty), Is.EqualTo("uuid"));
                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(enumProp), Is.EqualTo("text"));

                foreach (var (umlType, expectedSql) in GeneratorPropertyExtension.SqlTypeMapping)
                {
                    var dataType = new DataType { Name = umlType };
                    var prop = new Property { Name = "testProp", Type = dataType };
                    Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(prop), Is.EqualTo(expectedSql));
                }

                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(unknownProp), Is.EqualTo("text"));
                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(enumerableRef), Is.EqualTo("[uuid]"));
                Assert.That(GeneratorPropertyExtension.QuerySqlTypeName(unTypedProp), Is.EqualTo(string.Empty));
            }
        }

        /// <summary>
        /// Retrieves a property from the model by class name and property name.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The <see cref="IProperty" /> instance.</returns>
        private static IProperty GetOwnedProperty(string className, string propertyName)
        {
            var umlClass = GeneratorSetupFixture.XmiReaderResult.Packages
                .SelectMany(package => package.QueryPackages())
                .SelectMany(package => package.PackagedElement.OfType<IClass>())
                .Single(x => x.Name == className);

            return umlClass.OwnedAttribute.Single(x => x.Name == propertyName);
        }
    }
}
