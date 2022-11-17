// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onload = () => {
    let count = Number.parseInt(sessionStorage.getItem("cart-count"));
    if (!isNaN(count)) {
        let cartElement = document.getElementById("cart-btn");
        let cartCounterElement = document.getElementById("cart-counter");
        if (cartCounterElement == null) {
            cartCounterElement = document.createElement("div");
            cartCounterElement.id = "cart-counter";
            cartElement.appendChild(cartCounterElement);
        }
        cartCounterElement.innerHTML = `<small id="cart-count">${count}</small>`
    }
}
