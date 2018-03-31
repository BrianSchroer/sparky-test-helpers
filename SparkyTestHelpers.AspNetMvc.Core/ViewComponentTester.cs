using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// ASP.NET Core <see cref="ViewComponent"/> tester.
    /// </summary>
    /// <typeparam name="TViewComponent">The <see cref="ViewComponent"/> type.</typeparam>
    public class ViewComponentTester<TViewComponent> where TViewComponent : class
    {
        private readonly TViewComponent _viewComponent;

        /// <summary>
        /// Creates new <see cref="ViewComponentTester{TViewComponent}"/> instance.
        /// </summary>
        /// <param name="viewComponent">The <see cref="ViewComponent"/>.</param>
        public ViewComponentTester(TViewComponent viewComponent)
        {
            _viewComponent = AssertIsViewComponent(viewComponent);
        }

        /// <summary>
        /// Creates a new <see cref="ViewComponentInvocationTester"/> instance for a synchronous method.
        /// </summary>
        /// <param name="expression">Expression to specify the Invoke function to be tested.</param>
        /// <returns>New <see cref="ViewComponentInvocationTester"/> instance.</returns>
        public ViewComponentInvocationTester Invocation(Expression<Func<TViewComponent, Func<IViewComponentResult>>> expression)
        {
            Func<TViewComponent, Func<IViewComponentResult>> func = expression.Compile();

            return new ViewComponentInvocationTester(_viewComponent, func(_viewComponent));
        }

        /// <summary>
        /// Creates a new <see cref="ViewComponentInvocationTester"/> instance for an async method.
        /// </summary>
        /// <param name="expression">Expression to specify the InvokeAsync function to be tested.</param>
        /// <returns>New <see cref="ViewComponentInvocationTester"/> instance.</returns>
        public ViewComponentInvocationTester Invocation(Expression<Func<TViewComponent, Func<Task<IViewComponentResult>>>> expression)
        {
            Func<TViewComponent, Func<Task<IViewComponentResult>>> funcWithTask = expression.Compile();

            Func<IViewComponentResult> func = () =>
            {
                Task<IViewComponentResult> task = funcWithTask(_viewComponent)();
                task.Wait();
                return task.Result;
            };

            return new ViewComponentInvocationTester(_viewComponent, func);
        }

        private TViewComponent AssertIsViewComponent(TViewComponent viewComponent)
        {
            Type thisType = typeof(TViewComponent);

            if (TypeTester.IsOfType(viewComponent, typeof(ViewComponent))
                || thisType.Name.EndsWith("ViewComponent")
                || thisType.GetCustomAttributes(typeof(ViewComponentAttribute), inherit: true).Length > 0)
            {
                return viewComponent;
            }

            throw new ActionTestException(
                "A ViewComponent must inherit from ViewComponent"
                + ", have a name ending with \"ViewComponent\", or have a [ViewComponent] attribute."
                + $" {thisType.FullName} is not a valid ViewComponent type.");
        }
    }
}
