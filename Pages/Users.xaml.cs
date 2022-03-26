using System.Linq;
using System.Windows.Controls;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.AdminPages
{
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
            DGUser.ItemsSource = dbMonitoringEntities.gc().Users.ToList();
        }
    }
}