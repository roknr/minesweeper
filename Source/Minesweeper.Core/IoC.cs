using Ninject;
using Ninject.Modules;

namespace Minesweeper.Core
{
    /// <summary>
    /// The globally available inversion of control container that provides
    /// dependency injection functionality.
    /// </summary>
    public static class IoC
    {
        #region Private properties

        /// <summary>
        /// Flag indicating whether the IoC container has been initialized.
        /// </summary>
        private static bool Initialized { get; set; } = false;

        /// <summary>
        /// The actual application dependency injection container.
        /// </summary>
        private static IKernel Kernel { get; set; } = null!;

        #endregion

        #region Public methods

        /// <summary>
        /// Sets up the dependency injection container. Should only be called once, at the very beginning,
        /// before using any other functionality.
        /// </summary>
        /// <param name="module">The application module that defines service bindings.</param>
        public static void Setup(INinjectModule module)
        {
            // Only initialize once
            if (Initialized)
                return;

            // Create the DI container by passing in the service bindings
            Kernel = new StandardKernel(module);

            Initialized = true;
        }

        /// <summary>
        /// Gets an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of instance to get.</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        #endregion
    }
}
