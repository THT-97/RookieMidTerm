function SaveCart(id, name, saleprice, listprice, colors, sizes) {
    console.log(`${id} ${name} ${saleprice} ${listprice} ${colors} ${sizes}`);
    let cart = JSON.parse(sessionStorage.getItem("cart"));
    let count = Number.parseInt(sessionStorage.getItem("cart-count"));
    isNaN(count) ? count = 1 : count += 1;
    entity = {
        id: id,
        name: name,
        saleprice: saleprice,
        listprice: listprice,
        colors: colors,
        sizes: sizes,
        quantity: 1
    };
    if (cart == null) cart = [entity];
    else {
        let dup = [...cart].findIndex(e => e.id == entity.id);
        dup>=0 ? cart[dup].quantity += 1:
        cart = [...cart, entity];
    }
    sessionStorage.setItem("cart", JSON.stringify(cart));
    sessionStorage.setItem("cart-count", count);
    let cartElement = document.getElementById("cart-btn");
    let cartCounterElement = document.getElementById("cart-counter");
    if (cartCounterElement == null) {
        cartCounterElement = document.createElement("div");
        cartCounterElement.id = "cart-counter";
        cartElement.appendChild(cartCounterElement);
    }
    cartCounterElement.innerHTML = `<small id="cart-count">${count}</small>`
    console.log(cart);
}