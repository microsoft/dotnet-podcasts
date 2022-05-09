const Theme = {
    dark: 'Dark',
    light: 'Light',
    system: 'System'
};
const themeKey = 'theme';
const dataThemeKey = 'data-theme';

export const registerForSystemThemeChanged = (dotnetObj, callbackMethodName) => {
    const media = matchMedia('(prefers-color-scheme: dark)');
    if (media && dotnetObj) {
        media.onchange = args => {
            dotnetObj.invokeMethod(callbackMethodName, args.matches);
        }
    }
}

export const getSystemTheme = () =>
    matchMedia('(prefers-color-scheme: dark)').matches
        ? Theme.dark : Theme.light;

export const getTheme = () => {
    const savedTheme = localStorage.getItem(themeKey);
    if (savedTheme && savedTheme !== Theme.system) {
        return savedTheme;
    }
    return getSystemTheme();
}

const applyTheme = (theme) => {
    if (theme === Theme.dark) {
        document.body.setAttribute(dataThemeKey, theme);
    } else {
        document.body.removeAttribute(dataThemeKey);
    }
    return theme;
}

export const setTheme = (theme) =>
    localStorage.setItem(themeKey, applyTheme(theme));