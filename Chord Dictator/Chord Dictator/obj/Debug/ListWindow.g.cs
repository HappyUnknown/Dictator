﻿#pragma checksum "..\..\ListWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AB3B03C2119E8EB223F72542898B960B93B5F1F6762A4895AF464A49A664B899"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Chord_Dictator;
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


namespace Chord_Dictator {
    
    
    /// <summary>
    /// ListWindow
    /// </summary>
    public partial class ListWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView cmbElements;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgElement;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbImgPath;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnChangeImg;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbName;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnChangeName;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbAuPath;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnChangeAu;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\ListWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnApply;
        
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
            System.Uri resourceLocater = new System.Uri("/Chord Dictator;component/listwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListWindow.xaml"
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
            this.cmbElements = ((System.Windows.Controls.ListView)(target));
            
            #line 12 "..\..\ListWindow.xaml"
            this.cmbElements.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbElements_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.imgElement = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.tbImgPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnChangeImg = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\ListWindow.xaml"
            this.btnChangeImg.Click += new System.Windows.RoutedEventHandler(this.btnChangeImg_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tbName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnChangeName = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\ListWindow.xaml"
            this.btnChangeName.Click += new System.Windows.RoutedEventHandler(this.btnChangeName_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.tbAuPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.btnChangeAu = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\ListWindow.xaml"
            this.btnChangeAu.Click += new System.Windows.RoutedEventHandler(this.btnChangeAu_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnApply = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\ListWindow.xaml"
            this.btnApply.Click += new System.Windows.RoutedEventHandler(this.btnApply_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
