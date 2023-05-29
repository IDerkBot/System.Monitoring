using CommunityToolkit.Mvvm.DependencyInjection;

namespace SystemMonitoringNetCore.ViewModels;

public class Locator
{
    public UsersViewModel UserLoginView => Ioc.Default.GetRequiredService<UsersViewModel>();
    public RegistrationViewModel RegistrationView => Ioc.Default.GetRequiredService<RegistrationViewModel>();
    public AuthorizationViewModel AuthorizationView => Ioc.Default.GetRequiredService<AuthorizationViewModel>();
    public ReportsViewModel ReportsView => Ioc.Default.GetRequiredService<ReportsViewModel>();
    public FieldInfoViewModel FieldInfoView => Ioc.Default.GetRequiredService<FieldInfoViewModel>();
    public FertilizersViewModel FertilizersView => Ioc.Default.GetRequiredService<FertilizersViewModel>();
    public SelectFieldViewModel SelectFieldView => Ioc.Default.GetRequiredService<SelectFieldViewModel>();
    public CulturesControlViewModel CulturesControl => Ioc.Default.GetRequiredService<CulturesControlViewModel>();
    public AddCultureStatusControlViewModel AddCultureStatusControl => Ioc.Default.GetRequiredService<AddCultureStatusControlViewModel>();
    public AddCultureControlViewModel AddCultureControl => Ioc.Default.GetRequiredService<AddCultureControlViewModel>();
}