﻿namespace GEDKeeperX.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new GKUI.App());
        }
    }
}
