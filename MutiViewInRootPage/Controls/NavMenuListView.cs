using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace MutiViewInRootPage.Controls
{
    public class NavMenuListView : ListView
    {

        private SplitView splitViewHost;

        public NavMenuListView()
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.IsItemClickEnabled = true;
            this.ItemClick += NavMenuListView_ItemClick;

            //locate the hosting spliview control
            this.Loaded += NavMenuListView_Loaded;
        }

        private void NavMenuListView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this);
            while(parent!=null&&!(parent is Page)&&!(parent is SplitView))
            {
                parent = VisualTreeHelper.GetParent(parent);

            }

            if(parent!=null && parent is SplitView)
            {
                this.splitViewHost = parent as SplitView;

                splitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (control, args) =>
                {
                    this.OnPaneToggled();
                });

                this.OnPaneToggled();
            }
        }

        

        private void NavMenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = this.ContainerFromItem(e.ClickedItem);
            this.InvokeItem(item);
        }

        private void OnPaneToggled()
        {
            if(this.splitViewHost.IsPaneOpen)
            {
                this.ItemsPanelRoot.ClearValue(FrameworkElement.WidthProperty);
                this.ItemsPanelRoot.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            }
            else if(this.splitViewHost.DisplayMode==SplitViewDisplayMode.CompactInline || this.splitViewHost.DisplayMode==SplitViewDisplayMode.CompactOverlay)
            {
                this.ItemsPanelRoot.SetValue(FrameworkElement.WidthProperty, this.splitViewHost.CompactPaneLength);
                this.ItemsPanelRoot.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
            if (this.ItemsPanelRoot == null) return;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Remove the entrance animation on the item containers

            for(int i=0;i<this.ItemContainerTransitions.Count;i++)
            {
                if(this.ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    this.ItemContainerTransitions.RemoveAt(i);
                }
            }
        }


        public void SetSelectedItem(ListViewItem item)
        {
            int index = -1;
            if(item!=null)
            {
                index = this.IndexFromContainer(item);
            }

            for(int i=0; i<this.Items.Count;i++)
            {
                var lvi = (ListViewItem)this.ContainerFromIndex(i);
                if(i!=index)
                {
                    lvi.IsSelected = false;
                }
                else if(i==index)
                {
                    lvi.IsSelected = true;
                }

            }
        }
        ///<summary>
        ///
        /// Occurs when an item has been selected
        /// 
        /// </summary>
        /// 
        public event EventHandler<ListViewItem> ItemInvokded;

       

        private void InvokeItem(object focuedItem)
        {
            this.SetSelectedItem(focuedItem as ListViewItem);
            this.ItemInvokded?.Invoke(this, focuedItem as ListViewItem);

            if(this.splitViewHost==null || this.splitViewHost.IsPaneOpen)
            {
                if(this.splitViewHost!=null && (this.splitViewHost.DisplayMode==SplitViewDisplayMode.CompactInline || this.splitViewHost.DisplayMode==SplitViewDisplayMode.Overlay))
                {
                    this.splitViewHost.IsPaneOpen = false;
                }
                if(focuedItem is ListViewItem)
                {
                    ((ListViewItem)focuedItem).Focus(FocusState.Programmatic);
                }
            }

        }


       
    }
}
