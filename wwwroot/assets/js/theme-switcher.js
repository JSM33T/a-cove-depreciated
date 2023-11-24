"use strict";
(() => {
    'use strict';
    const getStoredTheme = () => localStorage.getItem('theme');
    const setStoredTheme = (theme) => localStorage.setItem('theme', theme);
    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }
        return 'dark';
    };
    const setTheme = (theme) => {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark');
        }
        else {
            document.documentElement.setAttribute('data-bs-theme', theme);
        }
    };
    setTheme(getPreferredTheme());
    const showActiveTheme = (theme) => {
        const themeSwitcher = document.querySelector('[data-bs-toggle="mode"]');
        if (!themeSwitcher) {
            return;
        }
        const themeSwitcherCheck = themeSwitcher.querySelector('input[type="checkbox"]');
        if (theme === 'dark') {
            themeSwitcherCheck.checked = true;
        }
        else {
            themeSwitcherCheck.checked = false;
        }
    };
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme();
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme());
        }
    });
    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme());
        document.querySelectorAll('[data-bs-toggle="mode"]').forEach((toggle) => {
            toggle.addEventListener('click', () => {
                const theme = (toggle.querySelector('input[type="checkbox"]').checked === true) ? 'dark' : 'light';
                setStoredTheme(theme);
                setTheme(theme);
                showActiveTheme(theme);
            });
        });
    });
})();
//# sourceMappingURL=theme-switcher.js.map