using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// ASP.NET Core MVC Razor Page <see cref="PageModel"/> tester.
    /// </summary>
    /// <typeparam name="TPageModel">The <see cref="PageModel"/> type..</typeparam>
    public class PageModelTester<TPageModel> where TPageModel : PageModel
    {
        private readonly TPageModel _pageModel;

        /// <summary>
        /// Creates new <see cref="PageModelTester{TPageModel}"/> instance.
        /// </summary>
        /// <param name="pageModel">The <see cref="PageModel"/>.</param>
        public PageModelTester(TPageModel pageModel)
        {
            _pageModel = pageModel;
        }

        /// <summary>
        /// Creates a new <see cref="PageModelActionTester"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the PageModel action to be tested.</param>
        /// <returns>New <see cref="PageModelActionTester"/> instance.</returns>
        public PageModelActionTester<TPageModel> Action(Expression<Func<TPageModel, Func<IActionResult>>> expression)
        {
            var func = expression.Compile();

            return new PageModelActionTester<TPageModel>(_pageModel, func(_pageModel));
        }

        /// <summary>
        /// Creates a new <see cref="PageModelActionTester"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async PageModel action to be tested.</param>
        /// <returns>New <see cref="PageModelActionTester"/> instance.</returns>
        public PageModelActionTester<TPageModel> Action(Expression<Func<TPageModel, Func<Task<IActionResult>>>> expression)
        {
            Func<TPageModel, Func<Task<IActionResult>>> funcWithTask = expression.Compile();

            Func<IActionResult> func = () =>
            {
                Task<IActionResult> task = funcWithTask(_pageModel)();
                task.Wait();
                return task.Result;
            };

            return new PageModelActionTester<TPageModel>(_pageModel, func);
        }
    }
}
