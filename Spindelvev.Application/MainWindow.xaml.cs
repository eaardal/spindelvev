using System;
using System.ComponentModel;
using Spindelvev.Infrastructure;
using Spindelvev.Infrastructure.IoC;

namespace Spindelvev.Application
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var ioc = TinyIoCContainer.Current;

            var connection = ioc.Resolve<IListenerConnection>();
            var listener = ioc.Resolve<ITrafficListener>();

            listener.StopListening(connection);
        }
    }
}
