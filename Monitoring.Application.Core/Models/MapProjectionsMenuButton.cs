// XAML Map Control - https://github.com/ClemensFischer/XAML-Map-Control
// Copyright © 2023 Clemens Fischer
// Licensed under the Microsoft Public License (Ms-PL)

using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Markup;
using MapControl;

namespace SystemMonitoringNetCore.Models
{
    [ContentProperty(nameof(Projection))]
    public class MapProjectionItem
    {
        public string Text { get; set; }
        public string Projection { get; set; }
    }

    [ContentProperty(nameof(MapProjections))]
    public class MapProjectionsMenuButton : MenuButton
    {
        private string selectedProjection;

        public MapProjectionsMenuButton()
            : base("\uE809")
        {
            ((INotifyCollectionChanged)MapProjections).CollectionChanged += (s, e) => InitializeMenu();
        }

        public static readonly DependencyProperty MapProperty = DependencyProperty.Register(
            nameof(Map), typeof(MapBase), typeof(MapProjectionsMenuButton),
            new PropertyMetadata(null, (o, e) => ((MapProjectionsMenuButton)o).InitializeMenu()));

        public MapBase Map
        {
            get => (MapBase)GetValue(MapProperty);
            set => SetValue(MapProperty, value);
        }

        public Collection<MapProjectionItem> MapProjections { get; } = new ObservableCollection<MapProjectionItem>();

        private void InitializeMenu()
        {
            if (Map != null)
            {
                var menu = CreateMenu();

                foreach (var item in MapProjections)
                {
                    menu.Items.Add(CreateMenuItem(item.Text, item.Projection, MapProjectionClicked));
                }

                var initialProjection = MapProjections.Select(p => p.Projection).FirstOrDefault();

                if (initialProjection != null)
                {
                    SetMapProjection(initialProjection);
                }
            }
        }

        private void MapProjectionClicked(object sender, RoutedEventArgs e)
        {
            var item = (FrameworkElement)sender;
            var projection = (string)item.Tag;

            SetMapProjection(projection);
        }

        private void SetMapProjection(string projection)
        {
            if (selectedProjection != projection)
            {
                selectedProjection = projection;
                Map.MapProjection = MapProjection.Factory.GetProjection(selectedProjection);
            }

            UpdateCheckedStates();
        }

        private void UpdateCheckedStates()
        {
            foreach (var item in GetMenuItems())
            {
                item.IsChecked = selectedProjection == (string)item.Tag;
            }
        }
    }
}
