using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SuperbetBeclean.Components
{
    /// <summary>
    /// Interaction logic for OwnedItemComponent.xaml
    /// </summary>
    public partial class OwnedItemComponent : UserControl
    {

        public static readonly DependencyProperty OwnedImagePathProperty = DependencyProperty.Register(
            "OwnedImagePath", typeof(string), typeof(ShopItemComponent), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty OwnedItemNameProperty = DependencyProperty.Register(
            "OwnedItemName", typeof(string), typeof(ShopItemComponent), new PropertyMetadata(default(string)));


        // Properties for data binding
        public string OwnedImagePath
        {
            get { return (string)GetValue(OwnedImagePathProperty); }
            set { SetValue(OwnedImagePathProperty, value); }
        }

        public string OwnedItemName
        {
            get { return (string)GetValue(OwnedItemNameProperty); }
            set { SetValue(OwnedItemNameProperty, value); }
        }
        

        public OwnedItemComponent()
        {
            InitializeComponent();
        }
    }
}
