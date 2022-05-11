const theme = localStorage.getItem('theme')
    ?? matchMedia('(prefers-color-scheme: dark)').matches
        ? 'Dark' : 'Light';
if (theme === 'Dark') {
    document.body.setAttribute('data-theme', theme);
}