using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GithubX.UWP.Views
{
	public sealed partial class SettingsDialog : ContentDialog
	{
		public SettingsDialog()
		{
			this.InitializeComponent();
		}

		private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			//Save Settings
			Hide();
		}
	}
}
