// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Xaml.Sample
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.DataContext = this;
            this.ThemeManager = new ThemeManager(
                new ObservableCollection<ThemeInfo>
                {
                    new ThemeInfo("/Themes/Dark.xaml"),
                    new ThemeInfo("/Themes/Light.xaml"),
                });
            this.InitializeComponent();

            this.Loaded += (s, e) => this.ThemeManager.CurrentTheme = this.ThemeManager.ThemeInfos[0];
        }

        public ThemeManager ThemeManager { get; }

        public List<ResourceDicionaryInfo> ResourceDictionaries
        {
            get
            {
                var list = new List<ResourceDicionaryInfo>();
                var dictionary = (IEnumerable)typeof(Sundew.Xaml.Optimizations.ResourceDictionary).GetField("ResourceDictionaries", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
                foreach (var o in dictionary)
                {
                    var value = o.GetType().GetProperty("Value").GetValue(o);
                    var key = (Uri)o.GetType().GetProperty("Key").GetValue(o);
                    var references = ((ICollection)value.GetType().GetProperty(
                                         "ReferencingResourceDictionaries",
                                         BindingFlags.Instance | BindingFlags.Public)
                                         .GetValue(value)).Count;
                    list.Add(new ResourceDicionaryInfo(key, references));
                }

                return list;
            }
        }

        public class ResourceDicionaryInfo
        {
            public ResourceDicionaryInfo(Uri uri, int references)
            {
                this.Uri = uri;
                this.References = references;
            }

            public Uri Uri { get; }

            public int References { get; }
        }
    }
}
