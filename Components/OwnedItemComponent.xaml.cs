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

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath", typeof(string), typeof(ShopItemComponent), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ItemNameProperty = DependencyProperty.Register(
            "ItemName", typeof(string), typeof(ShopItemComponent), new PropertyMetadata(default(string)));


        public static readonly DependencyProperty EquipCommandProperty = DependencyProperty.Register(
            "EqipCommand", typeof(ICommand), typeof(ShopItemComponent), new PropertyMetadata(default(ICommand)));

        // Properties for data binding
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public string ItemName
        {
            get { return (string)GetValue(ItemNameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public ICommand EquipCommand
        {
            get { return (ICommand)GetValue(EquipCommandProperty); }
            set { SetValue(EquipCommandProperty, value); }
        }

        public OwnedItemComponent()
        {
            InitializeComponent();
        }
    }
}
