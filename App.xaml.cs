﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Callisto.Controls;
using TheShoppingList.Classes;
using TheShoppingList.Settings;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TheShoppingList
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }
            rootFrame.Navigate(typeof(MainPage), args);
            MainPage p = rootFrame.Content as MainPage;
            //p.RootNamespace = this.GetType().Namespace;

            //// Shuttle the event args to the scenario selector to display the proper scenario.
            //p.ProtocolEvent = null;

            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // Settings Wide
            //SettingsCommand storage = new SettingsCommand("SettingsW", "Settings Wide", (x) =>
            //{
            //    SettingsFlyout settings = new SettingsFlyout();
            //    settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
            //    settings.HeaderText = "Settings Wide";

            //    settings.Content = new SettingsWide();
            //    settings.IsOpen = true;
            //});
            //args.Request.ApplicationCommands.Add(storage);

            // Settings Narrow
            //SettingsCommand settingsNarrow = new SettingsCommand("SettingsNarrow", "List Settings", (x) =>
            //{
            //    SettingsFlyout settings = new SettingsFlyout();
            //    settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;
            //    //settings.HeaderBrush = new SolidColorBrush(Colors.Blue);
            //    //settings.Background = new SolidColorBrush(Colors.Gray);
            //    settings.HeaderText = "List Settings";

            //    settings.Content = new SettingsNarrow();
            //    settings.IsOpen = true;
            //});
            //args.Request.ApplicationCommands.Add(settingsNarrow);


            // About
            SettingsCommand about = new SettingsCommand("About", "About", (x) =>
                                                                              {
                                                                                  SettingsFlyout settings =
                                                                                      new SettingsFlyout();
                                                                                  settings.FlyoutWidth =
                                                                                      SettingsFlyout.SettingsFlyoutWidth
                                                                                          .Narrow;
                                                                                  settings.HeaderText = "About";

                                                                                  settings.Content = new About();
                                                                                  settings.IsOpen = true;
                                                                              });
            args.Request.ApplicationCommands.Add(about);

            var privacyCommand = new SettingsCommand("privacyPage", "Privacy", command =>
                                                                                   {
                                                                                       var frame = Window.Current.Content as Frame;
                                                                                       if (
                                                                                           frame != null)
                                                                                           frame.Navigate(
                                                                                               typeof (PrivacyPage));
                                                                                   });
            args.Request.ApplicationCommands.Add(privacyCommand);

            //SettingsCommand settingsPrivacy = new SettingsCommand("SettingsPrivacy", "Privacy", (x) =>
            //{
            //    SettingsFlyout settings = new SettingsFlyout();
            //    settings.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Narrow;
            //    //settings.HeaderBrush = new SolidColorBrush(Colors.Blue);
            //    //settings.Background = new SolidColorBrush(Colors.Gray);
            //    settings.HeaderText = "Privacy";

            //    settings.Content = new SettingsNarrow();
            //    settings.IsOpen = true;
            //});
            //args.Request.ApplicationCommands.Add(settingsPrivacy);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            var source = Application.Current.Resources["shoppingSource"] as ShoppingSource;

            if (source != null)
            {
                await source.SaveListsAsync();
            }
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running

            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                TheShoppingList.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await TheShoppingList.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (TheShoppingList.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

           // frame.Navigate(typeof(SearchResultsPage), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}
