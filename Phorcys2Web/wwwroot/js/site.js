$(document).ready(function () {
    kendo.ui.icon($(".menu"), { icon: "menu", type: "svg" });
});

const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
const themeLink = document.getElementById('telerikThemeLink');

if (themeLink) {
    // Update theme switching to use CDN in production, local in development
    const baseCdnUrl = 'https://kendo.cdn.telerik.com/2023.3.1114/styles/';
    const baseLocalUrl = '/lib/kendo-ui/styles/';

    // Check if we're using CDN (production) or local (development)
    const isUsingCdn = themeLink.href.includes('kendo.cdn.telerik.com');

    if (isUsingCdn) {
        themeLink.href = prefersDark
            ? baseCdnUrl + 'kendo.classic-dark.min.css'
            : baseCdnUrl + 'kendo.classic.min.css';

        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
            themeLink.href = e.matches
                ? baseCdnUrl + 'kendo.classic-dark.min.css'
                : baseCdnUrl + 'kendo.classic.min.css';
        });
    } else {
        // Development - use local files
        themeLink.href = prefersDark
            ? baseLocalUrl + 'classic-main-dark.css'
            : baseLocalUrl + 'classic-main.css';

        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
            themeLink.href = e.matches
                ? baseLocalUrl + 'classic-main-dark.css'
                : baseLocalUrl + 'classic-main.css';
        });
    }
}
