﻿namespace PB_Blazor.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu;

        private string? NavMenuCssClass => collapseNavMenu ? "collapsed-nav-menu" : null;

        public NavMenu()
        {
            collapseNavMenu = true;
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
