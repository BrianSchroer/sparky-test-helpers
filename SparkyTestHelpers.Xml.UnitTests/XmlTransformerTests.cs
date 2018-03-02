using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Xml.Transformation;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using SparkyTestHelpers.Xml.Config;
using System;
using System.Xml.Linq;

namespace SparkyTestHelpers.Xml.UnitTests
{
    /// <summary>
    /// <see cref="XmlTransformer"/> tests.
    /// </summary>
    [TestClass]
    public class XmlTransformerTests
    {
        [TestMethod]
        public void ForXmlFile_should_throw_exception_if_no_paths_specified()
        {
            AssertExceptionThrown
                .OfType<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: possiblePaths")
                .WhenExecuting(() => XmlTransformer.ForXmlFile());
        }

        [TestMethod]
        public void ForXmlFile_should_return_XmlTransformer_when_one_or_more_paths_specified()
        {
            ForTest.Scenarios
            (
                new[] { "path1" },
                new[] { "path1", "path2" }
            )
            .TestEach(scenario => AssertExceptionNotThrown.WhenExecuting(() =>
            {
                var transformer = XmlTransformer.ForXmlFile(scenario);
                Assert.IsInstanceOfType(transformer, typeof(XmlTransformer));
            }));
        }

        [TestMethod]
        public void TransformedByFile_should_throw_exception_if_no_paths_specified()
        {
            var transformer = XmlTransformer.ForXmlFile("baseFile");

            AssertExceptionThrown
                .OfType<ArgumentNullException>()
                .WithMessage("Value cannot be null.\r\nParameter name: possiblePaths")
                .WhenExecuting(() => transformer.TransformedByFile());
        }

        [TestMethod]
        public void TransformedByFile_should_return_XmlTransformer_when_one_or_more_paths_specified()
        {
            ForTest.Scenarios
            (
                new[] { "path1" },
                new[] { "path1", "path2" }
            )
            .TestEach(scenario => AssertExceptionNotThrown.WhenExecuting(() =>
            {
                var transformer = XmlTransformer.ForXmlFile(scenario);
                Assert.AreSame(transformer, transformer.TransformedByFile(scenario));
            }));
        }

        [TestMethod]
        public void GetBasePath_should_return_path()
        {
            AssertExceptionNotThrown.WhenExecuting(() => Console.WriteLine(XmlTransformer.GetBaseFolder()));
        }

        [TestMethod]
        public void ResolveRelativePath_should_return_expected_values()
        {
            ForTest.Scenarios
            (
                new
                {
                    Base = @"C:\SourceCode\sparky-test-helpers\.vs\SparkyTestHelpers\lut\0\t\SparkyTestHelpers.Xml.UnitTests\bin\Debug",
                    Relative = "../../../../../../../../app.transform1.config",
                    Expected = @"C:\SourceCode\sparky-test-helpers\app.transform1.config"
                },
                new { Base = @"c:\folder\subfolder", Relative = @"something\web.config", Expected = @"c:\folder\subfolder\something\web.config" },
                new { Base = @"c:\folder\subfolder", Relative = @"/something/web.config", Expected = @"c:\something\web.config" },
                // Can use either forward or backward slashes:
                new { Base = @"c:\folder\subfolder", Relative = @"../../something/web.config", Expected = @"c:\something\web.config" },
                new { Base = @"c:\folder\subfolder", Relative = @"..\..\something\web.config", Expected = @"c:\something\web.config" },
                new { Base = @"c:\folder\subfolder", Relative = @"../something\web.config", Expected = @"c:\folder\something\web.config" },
                new { Base = @"c:\folder\subfolder", Relative = @"..\something\web.config", Expected = @"c:\folder\something\web.config" },
                // Full path ignores base:
                new { Base = @"c:\folder\subfolder", Relative = @"c:\different\subDifferent\web.config", Expected = @"c:\different\subDifferent\web.config" }
            )
            .TestEach(scenario =>
                Assert.AreEqual(
                    scenario.Expected.ToLowerInvariant(),
                    XmlTransformer.ResolveRelativePath(scenario.Base, scenario.Relative).ToLowerInvariant()));
        }

        [TestMethod]
        public void RemoveXmlNamespaces_should_remove_namespaces()
        {
            Assert.AreEqual(
                "<assemblyBinding><assemblyBinding>",
                XmlTransformer.RemoveXmlNamespaces(
                    "<assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">"
                    + "<assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\">"));
        }

        [TestMethod]
        public void Transform_should_work_with_no_transform_files()
        {
            TransformResults results =
                XmlTransformer
                    .ForXmlFile("SparkyTestHelpers.Xml.UnitTests.dll.config")
                    .Transform();

            Console.WriteLine(results.Log);
            Assert.IsTrue(results.Successful);
            Assert.IsNull(results.ErrorMessage);
            StringAssert.Contains(results.TransformedXml, "Original Value1");
            Assert.IsInstanceOfType(results.XDocument, typeof(XDocument));
        }

        [TestMethod]
        public void Transform_should_cache_TransformResults()
        {
            var transformer =
                XmlTransformer.ForXmlFile("base.config")
                .TransformedByFile("transform1.config", "transform2.config");

            TransformResults results1 = transformer.Transform();
            Assert.IsFalse(transformer.GotTransformResultsFromCache);

            TransformResults results2 = transformer.Transform();
            Assert.AreSame(results1, results2);
            Assert.IsTrue(transformer.GotTransformResultsFromCache);
        }

        [TestMethod]
        public void Transform_should_work_with_single_level_transformation()
        {
            TransformResults results =
                XmlTransformer
                    .ForXmlFile("SparkyTestHelpers.Xml.UnitTests.dll.config")
                    .TransformedByFile(RelativePaths("app.transform1.config"))
                    .Transform();

            Console.WriteLine(results.Log);
            Assert.IsTrue(results.Successful);
            Assert.IsNull(results.ErrorMessage);
            StringAssert.Contains(results.TransformedXml, "Value1 updated by transform1");
            StringAssert.Contains(results.TransformedXml, "transform1 applied");
            Assert.IsInstanceOfType(results.XDocument, typeof(XDocument));
        }

        [TestMethod]
        public void Transform_should_work_with_two_level_transformation()
        {
            TransformResults results =
                XmlTransformer
                    .ForXmlFile("SparkyTestHelpers.Xml.UnitTests.dll.config")
                    .TransformedByFile(RelativePaths("app.transform1.config"))
                    .TransformedByFile(RelativePaths("app.transform2.config"))
                    .Transform();

            Console.WriteLine(results.Log);
            Assert.IsTrue(results.Successful);
            Assert.IsNull(results.ErrorMessage);
            StringAssert.Contains(results.TransformedXml, "Value1 updated by transform2");
            StringAssert.Contains(results.TransformedXml, "transform1 applied");
            StringAssert.Contains(results.TransformedXml, "transform2 applied");
            Assert.IsInstanceOfType(results.XDocument, typeof(XDocument));
        }

        [TestMethod]
        public void Transform_XElement_into_XMlTester_should_work()
        {
            TransformResults results =
                XmlTransformer
                    .ForXmlFile("SparkyTestHelpers.Xml.UnitTests.dll.config")
                    .TransformedByFile(RelativePaths("app.transform1.config"))
                    .Transform();

            Assert.IsTrue(results.Successful);

            XmlTester xmlTester = new XmlTester(results.XDocument);
            Assert.IsInstanceOfType(xmlTester, typeof(XmlTester));

            xmlTester.AssertAppSettingsValue("Key1", "Value1 updated by transform1");
        }

        private static string[] RelativePaths(string filePath) =>
            new[]
            {
                filePath,
                $"../../../SparkyTestHelpers.Xml.UnitTests/{filePath}",
                $"../../../../../../../../SparkyTestHelpers.Xml.UnitTests/{filePath}"
            };
    }
}
