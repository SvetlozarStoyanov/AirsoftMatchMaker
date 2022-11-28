const ammoAmountInitial = document.getElementById('ammoAmountInitial');
const ammoAmountFinal = document.getElementById('ammoAmountFinal');
const pricePerUnit = document.getElementById('price');
const quantityToBuy = document.getElementById('quantityToBuy');
const finalPriceForSubmit = document.getElementById('finalPriceForSubmit');
const finalPriceForUi = document.getElementById('finalPriceForUi');

console.log(ammoAmountInitial.value)
calculatePriceAndUpdateAmmoCount();
function UpdatePricePerUnit() {
    var NameValue = pricePerUnit.value;
    // use it
    console.log(NameValue);
    calculatePriceAndUpdateAmmoCount();
}
pricePerUnit.onchange = UpdatePricePerUnit;
pricePerUnit.onblur = UpdatePricePerUnit;

function UpdateQuantityToBuy() {
    var NameValue = quantityToBuy.value;
    // use it
    console.log(NameValue);
    calculatePriceAndUpdateAmmoCount();
}
quantityToBuy.onchange = UpdateQuantityToBuy;
quantityToBuy.onblur = UpdateQuantityToBuy;

function calculatePriceAndUpdateAmmoCount() {
    console.log(pricePerUnit.value);
    console.log(quantityToBuy.value);
    finalPriceForSubmit.value = parseInt(quantityToBuy.value) * parseFloat(pricePerUnit.value);
    finalPriceForUi.value = finalPriceForSubmit.value;
    ammoAmountFinal.value = parseInt(quantityToBuy.value) * parseInt(ammoAmountInitial.value);
    console.log(`Final  price: ${finalPriceForSubmit.value} credits. Ammo count: ${ammoAmountFinal.value}`);
}