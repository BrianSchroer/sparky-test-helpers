using Microsoft.AspNetCore.Mvc;

namespace SparkyTestHelpers.AspNetCore.Controllers
{
    /// <summary>
    /// <see cref="Controller"/> ModelState test helpers.
    /// </summary>
    public static class ModelStateTestHelper
    {
        /// <summary>
        /// Set <see cref="Controller"/> ModelState.IsValid value. Use this method to test
        /// the results of ModelState.IsValid when you don't care *why* it's not valid.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="isValid">Is valid? (true/false).</param>
        public static void SetModelStateIsValid(Controller controller, bool isValid)
        {
            controller.ModelState.Clear();

            if (!isValid)
            {
                controller.ModelState.AddModelError("key", "message");
            }
        }
    }
}
