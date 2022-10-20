let rate = document.getElementsByClassName("rate-star");
for (let index = 0; index < rate.length; index++) {
    rate[index].addEventListener("click", Rate(rate[index].parentNode.firstElementChild))
}

function Rate(e) {
    e.checked;
}