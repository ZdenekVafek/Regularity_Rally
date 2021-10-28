﻿#pragma checksum "..\..\..\Control\TimeCollection.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6B41CBFB98222B6045F32B282285228092BE1FC23E2485FCC936274B4D3DCD37"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Regularity_Rally.Control;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Regularity_Rally.Control {
    
    
    /// <summary>
    /// TimeCollection
    /// </summary>
    public partial class TimeCollection : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 264 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Timekeeping_RacerName;
        
        #line default
        #line hidden
        
        
        #line 297 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label CheckLabel;
        
        #line default
        #line hidden
        
        
        #line 299 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox SelectAll;
        
        #line default
        #line hidden
        
        
        #line 301 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox SelectRacerTimeCollection;
        
        #line default
        #line hidden
        
        
        #line 310 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Time_txt_;
        
        #line default
        #line hidden
        
        
        #line 311 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Lap_txt_;
        
        #line default
        #line hidden
        
        
        #line 312 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Penalization_txt_;
        
        #line default
        #line hidden
        
        
        #line 313 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FinalTime_txt_;
        
        #line default
        #line hidden
        
        
        #line 314 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Note_txt_;
        
        #line default
        #line hidden
        
        
        #line 317 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label RecTimeSt;
        
        #line default
        #line hidden
        
        
        #line 319 "..\..\..\Control\TimeCollection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddRecord_btn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Regularity_Rally;component/control/timecollection.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Control\TimeCollection.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Timekeeping_RacerName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.CheckLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.SelectAll = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.SelectRacerTimeCollection = ((System.Windows.Controls.ComboBox)(target));
            
            #line 301 "..\..\..\Control\TimeCollection.xaml"
            this.SelectRacerTimeCollection.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectRacerTimeCollection_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Time_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 310 "..\..\..\Control\TimeCollection.xaml"
            this.Time_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Lap_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 311 "..\..\..\Control\TimeCollection.xaml"
            this.Lap_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Penalization_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 312 "..\..\..\Control\TimeCollection.xaml"
            this.Penalization_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 8:
            this.FinalTime_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 313 "..\..\..\Control\TimeCollection.xaml"
            this.FinalTime_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 9:
            this.Note_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 314 "..\..\..\Control\TimeCollection.xaml"
            this.Note_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 10:
            this.RecTimeSt = ((System.Windows.Controls.Label)(target));
            return;
            case 11:
            this.AddRecord_btn = ((System.Windows.Controls.Button)(target));
            
            #line 319 "..\..\..\Control\TimeCollection.xaml"
            this.AddRecord_btn.Click += new System.Windows.RoutedEventHandler(this.AddRecord_btn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

