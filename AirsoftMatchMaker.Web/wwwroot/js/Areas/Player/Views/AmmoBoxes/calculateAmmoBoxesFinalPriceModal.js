const ammoAmountInitial = document.getElementById('ammoAmountInitial');
const ammoAmountFinal = document.getElementById('ammoAmountFinal');
const pricePerUnit = document.getElementById('pricePerUnit');
const quantityToBuy = document.getElementById('quantityToBuy');
const finalPriceForSubmit = document.getElementById('finalPriceForSubmit');
const finalPriceForUi = document.getElementById('finalPriceForUi');
const userCreditsModal = parseFloat( document.getElementById('userCreditsModal').value);
const buyBtnModal = document.getElementById('buyBtnModal');

console.log(ammoAmountInitial.value)
calculatePriceAndUpdateAmmoCount();

function updatePricePerUnit() {
    var NameValue = pricePerUnit.value;
    // use it
    console.log(NameValue);
    calculatePriceAndUpdateAmmoCount();
}
pricePerUnit.onchange = updatePricePerUnit;
pricePerUnit.onblur = updatePricePerUnit;

function updateQuantityToBuy() {
    var NameValue = quantityToBuy.value;
    // use it
    console.log(NameValue);
    calculatePriceAndUpdateAmmoCount();
}

quantityToBuy.onchange = updateQuantityToBuy;
quantityToBuy.onblur = updateQuantityToBuy;

function updateBuyBtnModalStatus() {
    if (parseFloat(finalPriceForSubmit.value) > userCreditsModal) {
        buyBtnModal.disabled = true;
    } else {
        buyBtnModal.disabled = false;
    }
}


function calculatePriceAndUpdateAmmoCount() {
    console.log(pricePerUnit.value);
    console.log(quantityToBuy.value);
    finalPriceForSubmit.value = parseInt(quantityToBuy.value) * parseFloat(pricePerUnit.value);
    finalPriceForUi.value = finalPriceForSubmit.value;
    ammoAmountFinal.value = parseInt(quantityToBuy.value) * parseInt(ammoAmountInitial.value);
    updateBuyBtnModalStatus();
    console.log(`Final  price: ${finalPriceForSubmit.value} credits. Ammo count: ${ammoAmountFinal.value}`);
}