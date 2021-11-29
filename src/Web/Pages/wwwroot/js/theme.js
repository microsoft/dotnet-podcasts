let theme = localStorage.getItem('theme');
if (!theme) {
    theme = matchMedia('(prefers-color-scheme: dark)').matches ? 'Dark' : 'Light';
}
if (theme === 'Dark') {
    document.body.setAttribute('data-theme', theme);
}