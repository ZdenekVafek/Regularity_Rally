﻿#pragma checksum "..\..\..\Control\Add_UserToBrand.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E45C5EF5101C9E3F036BFF051B2EC3A800D9FFE2D08DB32F30E4A6E85901E878"
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
    /// Add_UserToBrand
    /// </summary>
    public partial class Add_UserToBrand : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 81 "..\..\..\Control\Add_UserToBrand.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid NameCars;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\Control\Add_UserToBrand.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView UserList;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\Control\Add_UserToBrand.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddUser_btn;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\Control\Add_UserToBrand.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseSelectUser;
        
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
            System.Uri resourceLocater = new System.Uri("/Regularity_Rally;component/control/add_usertobrand.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Control\Add_UserToBrand.xaml"
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
            this.NameCars = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.UserList = ((System.Windows.Controls.ListView)(target));
            
            #line 91 "..\..\..\Control\Add_UserToBrand.xaml"
            this.UserList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.UserList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.AddUser_btn = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\..\Control\Add_UserToBrand.xaml"
            this.AddUser_btn.Click += new System.Windows.RoutedEventHandler(this.AddUser_btn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.CloseSelectUser = ((System.Windows.Controls.Button)(target));
            
            #line 103 "..\..\..\Control\Add_UserToBrand.xaml"
            this.CloseSelectUser.Click += new System.Windows.RoutedEventHandler(this.CloseSelectUser_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

