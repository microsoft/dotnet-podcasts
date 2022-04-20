/*
    Shows a sticky header when the vertical scroll is greater than 50px,
    and hides it when is lesser

*/
function onScroll() {
    const scroll = window.scrollY;
    const header = document.querySelector('#header');

    if (scroll > 50) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}

window.addEventListener('scroll', onScroll, { passive: true });

function addClass() {
    const element = document.getElementById("menu");
    element.classList.add("visible");
}

function removeClass() {
    const element = document.getElementById("menu");
    element.classList.remove("visible");
}

onScroll();
