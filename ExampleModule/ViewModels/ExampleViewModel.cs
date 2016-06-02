namespace ExampleModule.ViewModels
{
    using GuiShared.PubSubEvents;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using System.Windows.Input;

    public class ExampleViewModel : BindableBase
    {
        private DelegateCommand<string> buttonOnePressCommand;
        private IEventAggregator eventAggregator;

        public ExampleViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.buttonOnePressCommand = new DelegateCommand<string>(this.SendButtonOneMessage);
        }

        public ICommand ButtonOnePressCommand
        {
            get
            {
                return this.buttonOnePressCommand;
            }
        }

        private void SendButtonOneMessage(string path)
        {
            this.eventAggregator.GetEvent<ButtonOneEvent>().Publish(new GuiShared.PubSubPayloads.ButtonOneInfo() { FilePath = path });
        }
    }
}
