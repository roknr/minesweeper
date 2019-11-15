using System.Collections.Generic;
using Minesweeper.Core.Interfaces.Routing;
using Minesweeper.Core.Interfaces.Services;
using Minesweeper.Core.Routing;
using Minesweeper.Core.ViewModels;
using Minesweeper.DesktopApp.Services;
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

            Bind<ApplicationViewModel>()
                .To<ApplicationViewModel>()
                .InSingletonScope();

            Bind<IThemeService>()
                .To<ThemeService>()
                .InTransientScope();

            Bind<IModalService>()
                .To<ModalService>()
                .InTransientScope();

            Bind<IUserSettingsService>()
                .To<SimpleJsonUserSettingsService>()
                .InTransientScope();
        }
    }
}
