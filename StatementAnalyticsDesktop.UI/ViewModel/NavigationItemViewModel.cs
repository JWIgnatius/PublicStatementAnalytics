using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using StatementAnalyticsDesktop.UI.Event;

namespace StatementAnalyticsDesktop.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;
        private string _detailViewModelName;

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
            _detailViewModelName = detailViewModelName;
            _eventAggregator = eventAggregator;
        }

        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(
                    new OpenDetailViewEventArgs
                    {
                        Id = Id,
                        ViewModelName = _detailViewModelName
                    });
        }
    }
}
