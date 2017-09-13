using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Library.Collection
{
    public class ObservableIncrementalLoadingCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public event OnLoadMoreItems LoadMoreItemsEvent;
        public delegate List<T> OnLoadMoreItems(uint count);

        private bool _hasMoreItems = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run(async () =>
            {
                var list = LoadMoreItemsEvent?.Invoke(count);

                if (list == null || list.Count == 0)
                {
                    _hasMoreItems = false;
                    return new LoadMoreItemsResult { Count = 0 };
                }

                foreach (var item in list)
                {
                    await CoreApplication.MainView.CoreWindow
                        .Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Add(item);
                        });
                }

                return new LoadMoreItemsResult { Count = (uint)list.Count };
            }).AsAsyncOperation();
        }

        public bool HasMoreItems => _hasMoreItems;
    }
}
