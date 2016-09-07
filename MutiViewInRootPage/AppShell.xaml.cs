using MutiViewInRootPage.Controls;
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

        #region Navigation

        private void NavMenuList_ItemInvoked(object sender, ListViewItem e)
        {
            //NavMenuList.SelectedIndex = -1;
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(e);
            if(item!=null)
            {
                AppFrame.Navigate(typeof(RootPages), item.Arguments);
            }
        }

        public void OpenNavPanel()
        {
            TogglePanelButton.IsChecked = true;
        }

        

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            if(e.Content is Page && e.Content!=null)
            {
                var control = (Page)e.Content;
                control.Loaded += Control_Loaded;
            }
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Control_Loaded;
        }

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {

        }
        #endregion

        /*
        public Rect TooglePaneButtonRect { get; private set; }
        ///<summary>
        ///An event to notify listeners when the hambuger button may occlude other content in the app
        /// the custom "PageHeader" user control is using this
        /// </summary>

        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        ///<summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// Hamburger button and trigger the evetn
        /// </summary>
        /// 
        private void CheckTogglePaneButtonSizeChanged()
        {
            TooglePaneButtonRect = RootSplitview.DisplayMode == SplitViewDisplayMode.Inline ||
                RootSplitview.DisplayMode == SplitViewDisplayMode.Overlay ? TogglePanelButton.TransformToVisual(this).TransformBounds(new Rect(0, 0, TogglePanelButton.ActualWidth, TogglePanelButton.ActualHeight)) :
                new Rect();
            TogglePaneButtonRectChanged?.Invoke(this, this.TooglePaneButtonRect);
        }
        */
    }
}
