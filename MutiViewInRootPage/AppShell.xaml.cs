using MutiViewInRootPage.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MutiViewInRootPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell : Page
    {
        

        public static AppShell Current = null;

        public List<NavMenuItem> NavList { get; } = new List<NavMenuItem>(new[]
        {
            new NavMenuItem()
            {
                Symbol = Symbol.Add,
                Label = "Add feed",
                DestPage = typeof(RootPages),
                Arguments = typeof(AddFeedView)
            },
            new NavMenuItem()
            {
                Symbol = Symbol.Edit,
                Label = "Edit feeds",
                DestPage = typeof(RootPages),
                Arguments = typeof(EditFeedView)
            }
        });
        public AppShell()
        {
            this.InitializeComponent();
            Current = this;

        }

        public Frame AppFrame => AppShellFrame;


        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        private void UpdateAutomationName<T>(ContainerContentChangingEventArgs args, string value)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is T)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, value);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        private void NavMenuItemContrinerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) =>
            UpdateAutomationName<NavMenuItem>(args, ((NavMenuItem)args.Item).Label);
        
    }
}
