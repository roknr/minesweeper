using System.Collections.Generic;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Routing;
using Ninject.Modules;

namespace Minesweeper.DesktopApp.IoC
{
    /// <summary>
    /// The minesweeper desktop application dependency injection module.
    /// </summary>
    public class MinesweeperModule : NinjectModule
    {
        /// <summary>
        /// Specifies the DI bindings.
        /// </summary>
        public override void Load()
        {
            Bind<IRouter>()
                .To<Router>()
                .InSingletonScope()
                .WithConstructorArgument<IEnumerable<Route>>(new Routes());
        }
    }
}
