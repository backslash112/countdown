﻿#pragma checksum "D:\windowsphone\CountDown\CountdownApp\Views\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5CD194139ADE1E2791CB5C625C871D4C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Coding4Fun.Toolkit.Controls;
using CountdownApp.UserControls;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace CountdownApp {
    
    
    public partial class CountdownMainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Coding4Fun.Toolkit.Controls.TimeSpanPicker TimePickerCountdownTime;
        
        internal Microsoft.Phone.Controls.ListPicker SoundsListPicker;
        
        internal CountdownApp.UserControls.WindowsPhoneControlStartButton StartButton;
        
        internal System.Windows.Controls.TextBox TextBoxName;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/CountdownApp;component/Views/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.TimePickerCountdownTime = ((Coding4Fun.Toolkit.Controls.TimeSpanPicker)(this.FindName("TimePickerCountdownTime")));
            this.SoundsListPicker = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("SoundsListPicker")));
            this.StartButton = ((CountdownApp.UserControls.WindowsPhoneControlStartButton)(this.FindName("StartButton")));
            this.TextBoxName = ((System.Windows.Controls.TextBox)(this.FindName("TextBoxName")));
        }
    }
}

