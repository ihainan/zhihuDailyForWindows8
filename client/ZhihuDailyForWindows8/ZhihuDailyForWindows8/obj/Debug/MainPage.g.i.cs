﻿

#pragma checksum "C:\Users\ihainan\documents\visual studio 2012\Projects\ZhihuDailyForWindows8\ZhihuDailyForWindows8\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "556D62CD650CA5B461417317FCC1ADD9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZhihuDailyForWindows8
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBar bottomAppBar; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressBar loadingProgress; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel sp_refresh; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.StackPanel sp_loadmore; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.GridView newsContent; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView newsContentListView; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing pr_refresh; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressRing pr; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualStateGroup ApplicationViewStates; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenLandscape; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Filled; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState FullScreenPortrait; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.VisualState Snapped; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///MainPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            bottomAppBar = (global::Windows.UI.Xaml.Controls.AppBar)this.FindName("bottomAppBar");
            loadingProgress = (global::Windows.UI.Xaml.Controls.ProgressBar)this.FindName("loadingProgress");
            sp_refresh = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("sp_refresh");
            sp_loadmore = (global::Windows.UI.Xaml.Controls.StackPanel)this.FindName("sp_loadmore");
            newsContent = (global::Windows.UI.Xaml.Controls.GridView)this.FindName("newsContent");
            newsContentListView = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("newsContentListView");
            pr_refresh = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("pr_refresh");
            pr = (global::Windows.UI.Xaml.Controls.ProgressRing)this.FindName("pr");
            ApplicationViewStates = (global::Windows.UI.Xaml.VisualStateGroup)this.FindName("ApplicationViewStates");
            FullScreenLandscape = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenLandscape");
            Filled = (global::Windows.UI.Xaml.VisualState)this.FindName("Filled");
            FullScreenPortrait = (global::Windows.UI.Xaml.VisualState)this.FindName("FullScreenPortrait");
            Snapped = (global::Windows.UI.Xaml.VisualState)this.FindName("Snapped");
        }
    }
}



