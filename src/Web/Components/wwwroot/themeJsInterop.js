const Theme = {
    Dark: 'Dark',
    Light: 'Light'
};

export function getTheme() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        return savedTheme;
    }
    return matchMedia('(prefers-color-scheme: dark)').matches ? Theme.Dark : Theme.Light;
}

export function applyTheme(value) {
    if (value === Theme.Dark) {
        document.body.setAttribute('data-theme', value);
    } else {
        document.body.removeAttribute('data-theme');
    }
}

export function setTheme(value) {
    applyTheme(value);
    localStorage.setItem('theme', value);
}

export function initializeTheme() {
    const value = getTheme();
    applyTheme(value);
}