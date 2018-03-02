using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Web.XmlTransform;

namespace SparkyTestHelpers.Xml.Transformation
{
    /// <summary>
    /// XML (config) file transformation helper.
    /// </summary>
    public class XmlTransformer
    {
        private static readonly Dictionary<string, TransformResults> _transformResults
            = new Dictionary<string, TransformResults>();

        private static readonly object _transformResultsLock = new object();

        private string[] _possibleBaseFilePaths;
        private List<string[]> _transformPathsArray = new List<string[]>();

        internal bool GotTransformResultsFromCache { get; private set; }

        /// <summary>
        /// Creates a new <see cref="XmlTransformer"/> instance.
        /// </summary>
        /// <param name="possiblePaths">The base file possible paths.</param>
        private XmlTransformer(string[] possiblePaths)
        {
            _possibleBaseFilePaths = possiblePaths;
        }

        /// <summary>
        /// Instantiates a new <see cref="XmlTransformer"/> instance for the specified file path.
        /// </summary>
        /// <param name="possiblePaths">
        /// Path(s) where the base XML file might be found. (Can be absolute path or relative to
        /// test runtime Assembly (DLL) file.
        /// </param>
        /// <returns>The <see cref="XmlTransformer"/>.</returns>
        public static XmlTransformer ForXmlFile(params string[] possiblePaths)
        {
            if (possiblePaths?.Length == 0)
            {
                throw new ArgumentNullException(nameof(possiblePaths));
            }

            return new XmlTransformer(possiblePaths);
        }

        /// <summary>
        /// Specifies a transformation XML file path.
        /// </summary>
        /// <param name="possiblePaths">
        /// Path(s) where the transform XML file might be found. (Can be absolute path or relative to
        /// the base path passed to <see cref="XmlTransformer.ForXmlFile(string[])"/>.
        /// </param>
        /// <returns>"This" <see cref="XmlTransformer"/>.></returns>
        public XmlTransformer TransformedByFile(params string[] possiblePaths)
        {
            if (possiblePaths?.Length == 0)
            {
                throw new ArgumentNullException(nameof(possiblePaths));
            }

            _transformPathsArray.Add(possiblePaths);
            return this;
        }

        /// <summary>
        /// Perform XML transformation(s).
        /// </summary>
        /// <returns>The <see cref="TransformResults"/>.</returns>
        public TransformResults Transform()
        {
            lock (_transformResultsLock)
            {
                string key = BuildResultsKey();

                if (_transformResults.ContainsKey(key))
                {
                    GotTransformResultsFromCache = true;
                    return _transformResults[key];
                }

                GotTransformResultsFromCache = false;
                TransformResults results = GetTransformResults();
                _transformResults.Add(key, results);
                return results;
            }
        }

        internal static string GetBaseFolder()
        {
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath.Replace("/", @"\");
            return new FileInfo(filePath).DirectoryName;
        }

        internal static string RemoveXmlNamespaces(string xml)
        {
            return Regex.Replace(xml, "\\s*xmlns=\"[^ >]*\"", string.Empty);
        }

        /// <summary>
        /// Resolve relative path.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>The resolved path.</returns>
        internal static string ResolveRelativePath(string basePath, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }

        private string BuildResultsKey()
        {
            var sb = new StringBuilder();

            sb.Append(string.Join("|", _possibleBaseFilePaths));

            foreach (string[] possiblePaths in _transformPathsArray)
            {
                sb.Append("|");
                sb.Append(string.Join("|", possiblePaths));
            }

            return sb.ToString();
        }

        private TransformResults GetTransformResults()
        {
            var results = new TransformResults { Successful = true };
            var log = new StringBuilder();
            var transformFilePaths = new List<string>();

            string baseFolder = GetBaseFolder();

            string baseFilePath = FindFilePath(baseFolder, _possibleBaseFilePaths, log);

            if (baseFilePath == null)
            {
                results.Successful = false;
            }
            else
            {
                log.AppendLine($"Base XML file is {baseFilePath}");
                baseFolder = new FileInfo(baseFilePath).DirectoryName;

                foreach (string[] possibleTransformPaths in _transformPathsArray)
                {
                    string transformFilePath = FindFilePath(baseFolder, possibleTransformPaths, log);
                    if (transformFilePath == null)
                    {
                        results.Successful = false;
                        break;
                    }
                    else
                    {
                        transformFilePaths.Add(transformFilePath);
                    }
                }
            }

            if (results.Successful)
            {
                ApplyTransformations(baseFilePath, transformFilePaths.ToArray(), results, log);
            }
            else
            {
                results.ErrorMessage = "File not found.";
            }

            results.Log = log.ToString();
            return results;
        }

        private string FindFilePath(string basePath, string[] pathSpec, StringBuilder log)
        {
            string foundPath = null;

            foreach(string possiblePath in pathSpec)
            {
                log.AppendLine($"Resolving \"{possiblePath}\" relative to \"{basePath}\"...");
                string resolvedPath = ResolveRelativePath(basePath, possiblePath);
                log.Append($"Resolved path {resolvedPath}...");

                if (File.Exists(resolvedPath))
                {
                    log.AppendLine("Found!");
                    foundPath = resolvedPath;
                    break;
                }
                else
                {
                    log.AppendLine("Not found.");
                }
            }

            return foundPath;
        }

        private void ApplyTransformations(
            string baseFilePath, 
            string[] transformFilePaths, 
            TransformResults results, 
            StringBuilder log)
        {
            using (var transformableDocument = new XmlTransformableDocument { PreserveWhitespace = true })
            {
                log.AppendLine($"Loading {baseFilePath} to XmlTransformableDocument...");

                transformableDocument.Load(baseFilePath);

                foreach (string transformFilePath in transformFilePaths)
                {
                    using (var sr = new StreamReader(transformFilePath))
                    using (var transformation = new XmlTransformation(sr.BaseStream, new TransformationLogger(log)))
                    {
                        log.AppendLine($"\nApplying transformation file {transformFilePath}...");

                        if (transformation.Apply(transformableDocument))
                        {
                            log.AppendLine("Transformation Successful!");
                        }
                        else
                        {
                            log.AppendLine("Transformation Failed.");
                            results.Successful = false;
                            results.ErrorMessage = $"Transformation failed for file \"{transformFilePath}\".";
                            break;
                        }
                    }
                }

                if (results.Successful)
                {
                    results.TransformedXml = RemoveXmlNamespaces(transformableDocument.OuterXml);
                    results.XDocument = XDocument.Parse(results.TransformedXml, LoadOptions.PreserveWhitespace);
            }
            }
        }
    }
}
