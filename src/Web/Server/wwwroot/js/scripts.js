const onScroll = () => {
    const scroll = window.scrollY;
    const header = document.querySelector('#header');
    if (!header) return;

    if (scroll > 50) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}

window.addEventListener('scroll', onScroll, { passive: true });

const addClass = () => {
    const element = document.getElementById("menu");
    if (element) element.classList.add("visible");
}

const removeClass = () => {
    const element = document.getElementById("menu");
    if (element) element.classList.remove("visible");
}

onScroll();