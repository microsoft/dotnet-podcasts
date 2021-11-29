function onScroll() {
    var scroll = window.scrollY;
    var header = document.querySelector('#header');

    if (scroll > 50) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}

window.addEventListener('scroll', onScroll, { passive: true });

function addClass() {
    var element = document.getElementById("menu");
    element.classList.add("visible");
}

function removeClass() {
    var element = document.getElementById("menu");
    element.classList.remove("visible");
}

onScroll();