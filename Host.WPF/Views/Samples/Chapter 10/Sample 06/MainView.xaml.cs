namespace Book.Views.Samples.Chapter10.Sample06
{
    using ReactiveUI;
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Controls.Primitives;
    using ViewModels.Samples.Chapter10.Sample06;

    public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            this
                .WhenActivated(
                    disposables =>
                    {
                        this
                            .Bind(this.ViewModel, x => x.Filter, x => x.filterTextBox.Text)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.Votes, x => x.dataGrid.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .OneWayBind(this.ViewModel, x => x.VoteCounts, x => x.voteCountsItemControl.ItemsSource)
                            .DisposeWith(disposables);
                        this
                            .WhenAnyValue(x => x.ViewModel.OrderBy, x => x.ViewModel.OrderDirection, (orderBy, orderDirection) => (orderBy, orderDirection))
                            .Do(this.UpdateSortDirections)
                            .Subscribe()
                            .DisposeWith(disposables);
                    });
        }

        private void HeaderClick(object sender, EventArgs e)
        {
            var columnHeader = (DataGridColumnHeader)sender;
            OrderBy orderBy;

            switch ((string)columnHeader.Content)
            {
                case "Name":
                    orderBy = OrderBy.Name;
                    break;
                case "Dinosaur Name":
                    orderBy = OrderBy.DinosaurName;
                    break;
                case "Dinosaur Diet":
                    orderBy = OrderBy.DinosaurDiet;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            ViewModel
                .OrderByCommand
                .Execute(orderBy)
                .Subscribe();
        }

        private void UpdateSortDirections((OrderBy orderBy, OrderDirection orderDirection) orderInfo)
        {
            ListSortDirection? nameOrder = null;
            ListSortDirection? dinosaurNameOrder = null;
            ListSortDirection? dinosaurDietOrder = null;
            var listSortDirection = orderInfo.orderDirection == OrderDirection.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            switch (orderInfo.orderBy)
            {
                case OrderBy.Name:
                    nameOrder = listSortDirection;
                    break;
                case OrderBy.DinosaurName:
                    dinosaurNameOrder = listSortDirection;
                    break;
                case OrderBy.DinosaurDiet:
                    dinosaurDietOrder = listSortDirection;
                    break;
            }

            this.nameColumn.SortDirection = nameOrder;
            this.dinosaurNameColumn.SortDirection = dinosaurNameOrder;
            this.dinosaurDietColumn.SortDirection = dinosaurDietOrder;
        }
    }
}