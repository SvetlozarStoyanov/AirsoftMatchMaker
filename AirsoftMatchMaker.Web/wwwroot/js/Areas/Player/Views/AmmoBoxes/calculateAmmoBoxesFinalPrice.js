const ammoAmountInitial = document.getElementById('ammoAmountInitial');
const ammoAmountFinal = document.getElementById('ammoAmountFinal');
const pricePerUnit = document.getElementById('pricePerUnit');
const quantityToBuy = document.getElementById('quantityToBuy');
const finalPriceForSubmit = document.getElementById('finalPriceForSubmit');
const finalPriceForUi = document.getElementById('finalPriceForUi');
const userCredits = parseFloat( document.getElementById('userCredits').value);
const buyBtn = document.getElementById('buyBtn');

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

function updateBuyBtnStatus() {
    if (parseFloat(finalPriceForSubmit.value) > userCredits) {
        buyBtn.disabled = true;
    } else {
        buyBtn.disabled = false;
    }
}


function calculatePriceAndUpdateAmmoCount() {
    console.log(pricePerUnit.value);
    console.log(quantityToBuy.value);
    finalPriceForSubmit.value = parseInt(quantityToBuy.value) * parseFloat(pricePerUnit.value);
    finalPriceForUi.value = finalPriceForSubmit.value;
    ammoAmountFinal.value = parseInt(quantityToBuy.value) * parseInt(ammoAmountInitial.value);
    updateBuyBtnStatus();
    console.log(`Final  price: ${finalPriceForSubmit.value} credits. Ammo count: ${ammoAmountFinal.value}`);
}