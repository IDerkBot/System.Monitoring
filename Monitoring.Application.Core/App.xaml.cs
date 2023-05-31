using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using SystemMonitoringNetCore.ViewModels;

namespace SystemMonitoringNetCore
{
    /// <summary> Interaction logic for App.xaml </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    
                    .AddSingleton<AuthorizationViewModel>()
                    .AddSingleton<MenuViewModel>()
                    .AddSingleton<RegistrationViewModel>()
                    .AddSingleton<ReportsViewModel>()
                    .AddSingleton<SelectFieldViewModel>()
                    .AddSingleton<FieldInfoViewModel>()
                    .AddSingleton<UsersViewModel>()
                    .AddSingleton<CulturesControlViewModel>()
                    .AddSingleton<AddCultureStatusControlViewModel>()
                    .AddSingleton<AddCultureControlViewModel>()
                    .AddSingleton<FertilizersViewModel>()


                    .BuildServiceProvider());
        }
    }
}