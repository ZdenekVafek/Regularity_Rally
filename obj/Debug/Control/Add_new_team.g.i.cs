﻿#pragma checksum "..\..\..\Control\Add_new_team.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F5FE5808081209582A38AFD29F63BFB78F1061493492C8911F722C01228C727A"
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
    /// Add_new_team
    /// </summary>
    public partial class Add_new_team : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 129 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox name_team_txt_;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox short_team_txt_;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox email_team_txt_;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button save_team_btn;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ActionDelete;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ActionClose;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\Control\Add_new_team.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView RacersInTeamList;
        
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
            System.Uri resourceLocater = new System.Uri("/Regularity_Rally;component/control/add_new_team.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Control\Add_new_team.xaml"
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
            this.name_team_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 129 "..\..\..\Control\Add_new_team.xaml"
            this.name_team_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 2:
            this.short_team_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 130 "..\..\..\Control\Add_new_team.xaml"
            this.short_team_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 3:
            this.email_team_txt_ = ((System.Windows.Controls.TextBox)(target));
            
            #line 131 "..\..\..\Control\Add_new_team.xaml"
            this.email_team_txt_.GotFocus += new System.Windows.RoutedEventHandler(this.CarotSet);
            
            #line default
            #line hidden
            return;
            case 4:
            this.save_team_btn = ((System.Windows.Controls.Button)(target));
            
            #line 133 "..\..\..\Control\Add_new_team.xaml"
            this.save_team_btn.Click += new System.Windows.RoutedEventHandler(this.Save_team_btn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ActionDelete = ((System.Windows.Controls.Button)(target));
            
            #line 134 "..\..\..\Control\Add_new_team.xaml"
            this.ActionDelete.Click += new System.Windows.RoutedEventHandler(this.ActionDelete_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ActionClose = ((System.Windows.Controls.Button)(target));
            
            #line 135 "..\..\..\Control\Add_new_team.xaml"
            this.ActionClose.Click += new System.Windows.RoutedEventHandler(this.ActionClose_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.RacersInTeamList = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

