using System.Linq;
using System.Windows.Controls;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Views.Pages
{
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
            DGUser.ItemsSource = u519667_monitoringEntities.gc().Users.ToList();
        }
    }
}