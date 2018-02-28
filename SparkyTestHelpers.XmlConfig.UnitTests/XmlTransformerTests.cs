using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.XmlConfig.Transformation;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;

namespace SparkyTestHelpers.XmlConfig.UnitTests
{
    /// <summary>
    /// <see cref="XmlTransformer"/> tests.
    /// </summary>
    [TestClass]
    public class XmlTransformerTests
    {
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
    }
}
