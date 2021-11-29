export function play(element) {
    element.play();
}

export function pause(element) {
    element.pause();
}

export function stop(element) {
    element.stop();
}

export function setMuted(element, value) {
    element.muted = value;
}

export function setVolume(element, value) {
    element.volume = value;
}

export function setCurrentTime(element, value) {
    element.currentTime = value;
}