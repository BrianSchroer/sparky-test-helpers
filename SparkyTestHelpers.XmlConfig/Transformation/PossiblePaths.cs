using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.XmlConfig.Transformation
{
    /// <summary>
    /// "Possible paths" for situations where an XML file might be in multiple different locations. 
    /// </summary>
    internal class PossiblePaths
    {
        /// <summary>
        /// Possible paths where .config file is located. The first found file is used (can be absolute or relative).
        /// </summary>
        public IEnumerable<string> Paths { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PossiblePaths"/> instance.
        /// </summary>
        /// <param name="paths">Value(s) for the <see cref="Paths"/> property.</param>
        public PossiblePaths(params string[] paths)
        {
            if (paths?.Length == 0)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            Paths = paths;
        }
    }
}
